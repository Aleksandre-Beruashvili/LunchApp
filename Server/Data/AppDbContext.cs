using Microsoft.EntityFrameworkCore;
using OfficeCafeApp.API.Models;

namespace OfficeCafeApp.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Dish> Dishes => Set<Dish>();
        public DbSet<Slot> Slots => Set<Slot>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<Rating> Ratings => Set<Rating>();
        public DbSet<MenuSchedule> MenuSchedules => Set<MenuSchedule>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("lunchapp");

            modelBuilder.Entity<OrderItem>()
                .HasKey(oi => new { oi.OrderId, oi.DishId });

            modelBuilder.Entity<MenuSchedule>()
                .HasOne(ms => ms.Dish)
                .WithMany(d => d.MenuSchedules)
                .HasForeignKey(ms => ms.DishId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}