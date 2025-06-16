using System;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using OfficeCafeApp.API.Models;

namespace OfficeCafeApp.API.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {

            context.Database.Migrate();

            // Seed slots if none exist
            if (!context.Slots.Any())
            {
                var slots = new[]
                {
                    new Slot { StartTime = new TimeSpan(11, 0, 0), EndTime = new TimeSpan(11, 15, 0) },
                    new Slot { StartTime = new TimeSpan(11, 15, 0), EndTime = new TimeSpan(11, 30, 0) }
                };
                context.Slots.AddRange(slots);
            }

            // Seed admin user if not exists
            if (!context.Users.Any(u => u.Role == "Admin"))
            {
                CreatePasswordHash("AdminCR@123", out byte[] passwordHash, out byte[] passwordSalt);

                context.Users.Add(new User
                {
                    Email = "admin@credo.ge",
                    FullName = "Admin Manager",
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Role = "Admin"
                });
            }

            context.SaveChanges();
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
}
