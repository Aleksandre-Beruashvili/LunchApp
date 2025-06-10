using OfficeCafeApp.API.Models;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace OfficeCafeApp.API.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Slots.Any())
            {
                var slots = new[]
                {
                    new Slot { StartTime = new TimeSpan(11, 0, 0), EndTime = new TimeSpan(11, 15, 0) },
                    new Slot { StartTime = new TimeSpan(11, 15, 0), EndTime = new TimeSpan(11, 30, 0) },
                    // Add more slots...
                };
                context.Slots.AddRange(slots);
            }

            if (!context.Users.Any(u => u.Role == "Manager"))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash("admin123", out passwordHash, out passwordSalt);

                context.Users.Add(new User
                {
                    Email = "admin@office.com",
                    FullName = "Admin Manager",
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Role = "Manager"
                });
            }

            context.SaveChanges();
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}