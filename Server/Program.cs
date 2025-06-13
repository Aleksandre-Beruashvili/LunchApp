using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OfficeCafeApp.API.Data;
using OfficeCafeApp.API.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1) Db & DI
builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IAdminService, AdminService>();

// 2) JWT Auth
builder.Services
  .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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

// 3) Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 4) Migrate & seed
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    DbInitializer.Initialize(db);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 5) Serve login.html by default & static files
var defaultFiles = new DefaultFilesOptions();
defaultFiles.DefaultFileNames.Clear();
defaultFiles.DefaultFileNames.Add("login.html");
app.UseDefaultFiles(defaultFiles);
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// 6) Map API controllers
app.MapControllers();

// 7) Fallback any unknown route to login.html
app.MapFallbackToFile("login.html");

app.Run();
