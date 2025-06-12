using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OfficeCafeApp.API.Data;
using OfficeCafeApp.API.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1) CORS: allow the hosted Blazor client on the same origin
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy
            .WithOrigins("https://localhost:5001") // ✅ match server URL
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// 2) Data & services
builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IAdminService, AdminService>();

// 3) JWT auth
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(opts =>
  {
      opts.TokenValidationParameters = new TokenValidationParameters
      {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(
          Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
          ValidateIssuer = true,
          ValidIssuer = builder.Configuration["Jwt:Issuer"],
          ValidateAudience = true,
          ValidAudience = builder.Configuration["Jwt:Audience"],
          ValidateLifetime = true,
          ClockSkew = TimeSpan.Zero
      };
  });
builder.Services.AddAuthorization();

// 4) MVC + Swagger + Blazor static
builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 5) Migrate and seed
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    DbInitializer.Initialize(db);
}

// 6) Dev-time Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();   // serve Blazor static files
app.UseStaticFiles();

app.UseRouting();

// 7) CORS **before** auth
app.UseCors("AllowBlazorClient");

app.UseAuthentication();
app.UseAuthorization();

// 8) Endpoints
app.MapControllers();
app.MapRazorPages();
app.MapFallbackToFile("index.html"); // serve Blazor UI

app.Run();
