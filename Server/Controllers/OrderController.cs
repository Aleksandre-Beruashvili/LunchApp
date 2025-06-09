using lunchapp.Shared.DTOs;
using LunchApp.Server.Data; 
using Microsoft.AspNetCore.Mvc;

namespace LunchApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _context;
        public OrderController(AppDbContext context) { _context = context; }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder([FromBody] OrderDto dto)
        {
            var order = new LunchApp.Shared.Models.Order
            {
                UserId = dto.UserId,
                PickupTime = dto.PickupTime,
                Date = dto.Date
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            foreach (var dishId in dto.DishIds)
            {
                _context.OrderItems.Add(new LunchApp.Shared.Models.OrderItem
                {
                    OrderId = order.Id,
                    DishId = dishId
                });
            }
            await _context.SaveChangesAsync();
            return Ok(order.Id);
        }
    }
}