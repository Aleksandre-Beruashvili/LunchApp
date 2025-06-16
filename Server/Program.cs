using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using OfficeCafeApp.API.Data;
using OfficeCafeApp.API.Services;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// ----------------------------
// 1. Register Services
// ----------------------------

builder.Services.AddDbContext<AppDbContext>(opts =>
    opts.UseSqlServer(config.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

builder.Services.AddControllers();

// JWT Authentication
var jwtKey = config["Jwt:Key"] ?? throw new InvalidOperationException("JWT key is not configured.");
var keyBytes = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                context.HandleResponse();

                var path = context.Request.Path.Value ?? "";

                if (path.StartsWith("/admin") && context.Request.Method == "GET")
                {
                    context.Response.StatusCode = 302;
                    context.Response.Headers.Location = "/admin/admin-login.html";
                }
                else
                {
                    context.Response.StatusCode = 401;
                }

                return Task.CompletedTask;
            }
        };

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// ----------------------------
// 2. Middleware Pipeline
// ----------------------------

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();

// Serve default index.html if exists
app.UseDefaultFiles();

// Serve wwwroot static files (e.g., /admin/admin-login.html)
app.UseStaticFiles();

// Serve /uploads/* (uploaded images)
var uploadPath = Path.Combine(app.Environment.WebRootPath, "uploads");
Directory.CreateDirectory(uploadPath);
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadPath),
    RequestPath = "/uploads"
});

// Serve /admin static files from wwwroot/admin
var adminPath = Path.Combine(app.Environment.WebRootPath, "admin");
if (Directory.Exists(adminPath))
{
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(adminPath),
        RequestPath = "/admin"
    });
}
else
{
    Console.WriteLine("⚠️ Warning: /wwwroot/admin directory is missing.");
}

app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

// ----------------------------
// 3. API & Fallback
// ----------------------------

app.MapControllers();
app.MapFallbackToFile("employee/login.html");

// ----------------------------
// 4. Seed Admin + Slots
// ----------------------------

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DbInitializer.Initialize(db);
}

app.Run();
