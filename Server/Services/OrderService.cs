using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LunchApp.Shared.DTOs;
using Microsoft.EntityFrameworkCore;
using OfficeCafeApp.API.Data;
using OfficeCafeApp.API.Models;

namespace OfficeCafeApp.API.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OrderResult> CreateOrderAsync(int userId, OrderCreateDto orderDto)
        {
            var slot = await _context.Slots.FindAsync(orderDto.SlotId);
            if (slot == null) throw new Exception("Invalid slot");

            var order = new Order
            {
                UserId = userId,
                SlotId = orderDto.SlotId,
                Status = "Preparing",
                QRCode = Guid.NewGuid().ToString()
            };

            _context.Orders.Add(order);

            foreach (var item in orderDto.Items)
            {
                var dish = await _context.Dishes.FindAsync(item.DishId);
                if (dish == null) throw new Exception($"Dish {item.DishId} not found");

                _context.OrderItems.Add(new OrderItem
                {
                    OrderId = order.Id,
                    DishId = item.DishId,
                    Quantity = item.Quantity
                });
            }

            await _context.SaveChangesAsync();

            return new OrderResult
            {
                OrderId = order.Id,
                QRCode = order.QRCode,
                PickupTime = $"{slot.StartTime:hh\\:mm} - {slot.EndTime:hh\\:mm}"
            };
        }

        public async Task<IEnumerable<OrderDto>> GetUserOrdersAsync(int userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.Slot)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Dish)
                .OrderByDescending(o => o.OrderDate)
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    Status = o.Status,
                    QRCode = o.QRCode,
                    Slot = $"{o.Slot.StartTime:hh\\:mm}-{o.Slot.EndTime:hh\\:mm}",
                    Items = o.OrderItems.Select(oi => new OrderItemDto
                    {
                        DishId = oi.DishId,
                        DishName = oi.Dish.Name,
                        Quantity = oi.Quantity,
                        Price = oi.Dish.Price
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<bool> CancelOrderAsync(int userId, Guid orderId)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

            if (order == null) return false;

            // Check if cancellation is allowed (30 mins before slot)
            var slot = await _context.Slots.FindAsync(order.SlotId);
            var now = DateTime.UtcNow;
            var slotStart = now.Date + slot.StartTime;

            if ((slotStart - now).TotalMinutes < 30) return false;

            order.Status = "Cancelled";
            await _context.SaveChangesAsync();
            return true;
        }
    }
}