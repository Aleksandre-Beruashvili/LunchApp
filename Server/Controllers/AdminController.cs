using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LunchApp.Server.Data;
using LunchApp.Shared.DTOs;

namespace LunchApp.Server.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _context;
        public AdminController(AppDbContext context) { _context = context; }

        [HttpPost("dish")]
        public async Task<IActionResult> AddDish([FromBody] DishDto dto)
        {
            var dish = new LunchApp.Shared.Models.Dish
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                ImageUrl = dto.ImageUrl,
                Available = dto.Available
            };
            _context.Dishes.Add(dish);
            await _context.SaveChangesAsync();
            return Ok(dish);
        }

        [HttpPut("dish/{id}")]
        public async Task<IActionResult> UpdateDish(int id, [FromBody] DishDto dto)
        {
            var dish = await _context.Dishes.FindAsync(id);
            if (dish == null) return NotFound();
            dish.Name = dto.Name;
            dish.Description = dto.Description;
            dish.Price = dto.Price;
            dish.ImageUrl = dto.ImageUrl;
            dish.Available = dto.Available;
            await _context.SaveChangesAsync();
            return Ok(dish);
        }

        [HttpGet("orders")]
        public async Task<IActionResult> GetOrders([FromQuery] DateTime date)
        {
            var orders = await _context.Orders
                .Where(o => o.Date == date)
                .Include(o => o.Items)
                    .ThenInclude(i => i.Dish)
                .ToListAsync();
            return Ok(orders);
        }
    }
}
