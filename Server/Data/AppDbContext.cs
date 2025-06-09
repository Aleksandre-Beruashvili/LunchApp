using LunchApp.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LunchApp.Server.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("lunchapp");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityRole>().HasData(
              new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
              new IdentityRole { Name = "User", NormalizedName = "USER" }
            );

        }

        public DbSet<Dish> Dishes { get; set; }
        public DbSet<MenuSchedule> MenuSchedules { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Rating> Ratings { get; set; }
    }
}