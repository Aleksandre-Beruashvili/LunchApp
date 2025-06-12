using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OfficeCafeApp.API.Data;
using OfficeCafeApp.API.Models;

namespace OfficeCafeApp.API.Services
{
    public class AdminService : IAdminService
    {
        private readonly AppDbContext _context;

        public AdminService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetOrdersByDateAsync(DateTime date)
        {
            return await _context.Orders
                .Where(o => o.OrderDate.Date == date.Date)
                .Include(o => o.User)
                .Include(o => o.Slot)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Dish)
                .ToListAsync();
        }

        public async Task<IEnumerable<(TimeSpan Slot, int Count)>> GetOrderCountsByDateAsync(DateTime date)
        {
            return await _context.Orders
                .Where(o => o.OrderDate.Date == date.Date)
                .GroupBy(o => o.Slot.StartTime)
                .Select(g => (Slot: g.Key, Count: g.Count()))
                .ToListAsync();
        }

        public async Task<bool> UpdateDishAvailabilityAsync(int dishId, bool isAvailable)
        {
            var dish = await _context.Dishes.FindAsync(dishId);
            if (dish == null) return false;

            dish.IsAvailable = isAvailable;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}