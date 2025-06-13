using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using OfficeCafeApp.API.Data;
using OfficeCafeApp.API.Services;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var env = builder.Environment;
var services = builder.Services;

// 1. Register Services
// ---------------------
services.AddDbContext<AppDbContext>(opts =>
    opts.UseSqlServer(config.GetConnectionString("DefaultConnection")));

services.AddScoped<IAuthService, AuthService>();
services.AddScoped<IAdminService, AdminService>();
services.AddScoped<IOrderService, OrderService>();

services.AddCors(options => options.AddPolicy("AllowAll",
    builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

services.AddControllers();

var jwtKey = config["Jwt:Key"];
if (string.IsNullOrWhiteSpace(jwtKey))
    throw new InvalidOperationException("JWT key is not configured.");

var keyBytes = Encoding.UTF8.GetBytes(jwtKey);
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                context.HandleResponse(); // Stop default 401 response
                context.Response.Redirect("/admin/admin-login.html"); // Redirect to login page
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

services.AddAuthorization();

var app = builder.Build();

// 2. Middleware Setup
// ---------------------
if (env.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHsts();  // good practice in production
}

app.UseHttpsRedirection();
app.UseStaticFiles();     // for wwwroot/
app.UseDefaultFiles();    // for index.html under wwwroot/

app.UseRouting();         // MUST be before auth for routing to work

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

// 3. Admin UI static file serving
// ---------------------
var adminPath = Path.Combine(env.WebRootPath, "manager");
if (Directory.Exists(adminPath))
{
    var adminFiles = new PhysicalFileProvider(adminPath);

    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = adminFiles,
        RequestPath = "/admin"
    });
}
else
{
    // Optional: Log warning if manager folder is missing
    Console.WriteLine("Warning: 'wwwroot/manager' directory not found. Admin UI won't be served.");
}

// 4. API & Fallback Routes
// ---------------------
app.MapControllers();

// Fallback to employee login if path doesn't match anything
app.MapFallbackToFile("employee/login.html");

app.Run();
