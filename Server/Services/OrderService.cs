using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LunchApp.Core.Enums;
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

        public async Task<OrderResult> CreateOrderAsync(int userId, OrderCreateDto dto)
        {
            using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                var slot = await _context.Slots.FindAsync(dto.SlotId)
                    ?? throw new KeyNotFoundException("Invalid slot");

                var order = new Order
                {
                    UserId = userId,
                    SlotId = dto.SlotId,
                    QRCode = Guid.NewGuid().ToString(),
                    Status = OrderStatus.Preparing.ToString()
                };
                _context.Orders.Add(order);

                foreach (var item in dto.Items)
                {
                    var dish = await _context.Dishes.FindAsync(item.DishId)
                               ?? throw new KeyNotFoundException($"Dish {item.DishId} not found");
                    if (!dish.IsAvailable)
                        throw new InvalidOperationException($"Dish {dish.Name} is sold out");

                    _context.OrderItems.Add(new OrderItem
                    {
                        OrderId = order.Id,
                        DishId = dish.Id,
                        Quantity = item.Quantity
                    });
                }

                slot.CurrentCount++;
                await _context.SaveChangesAsync();
                await tx.CommitAsync();

                return new OrderResult
                {
                    OrderId = order.Id,
                    QRCode = order.QRCode,
                    PickupTime = $"{slot.StartTime:hh\\:mm}-{slot.EndTime:hh\\:mm}"
                };
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        public async Task<IEnumerable<OrderDto>> GetUserOrdersAsync(int userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
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

        public async Task<ServiceResult> CancelOrderAsync(int userId, Guid orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null || order.UserId != userId)
                return new ServiceResult { Success = false, Error = "Order not found." };

            var slot = await _context.Slots.FindAsync(order.SlotId);
            var now = DateTime.UtcNow;
            var slotStart = now.Date + slot.StartTime;
            if ((slotStart - now).TotalMinutes < 30)
                return new ServiceResult { Success = false, Error = "Too late to cancel." };

            order.Status = OrderStatus.Cancelled.ToString();
            slot.CurrentCount--;
            await _context.SaveChangesAsync();
            return new ServiceResult { Success = true };
        }
    }
}