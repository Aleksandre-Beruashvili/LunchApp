using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeCafeApp.API.Data;
using OfficeCafeApp.API.DTOs;
using OfficeCafeApp.API.Models;
using OfficeCafeApp.API.Services;

namespace OfficeCafeApp.API.Controllers
{
    [Authorize(Roles = "Manager")]
    [ApiController]
    [Route("api/admin/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IAdminService _adminService;

        public DashboardController(AppDbContext context, IAdminService adminService)
        {
            _context = context;
            _adminService = adminService;
        }

        [HttpGet("orders")]
        public async Task<IActionResult> GetOrdersByDate([FromQuery] DateTime date)
        {
            var orders = await _context.Orders
                .Where(o => o.OrderDate.Date == date.Date)
                .Include(o => o.User)
                .Include(o => o.Slot)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Dish)
                .ToListAsync();

            return Ok(orders);
        }

        [HttpPatch("dishes/{id}/availability")]
        public async Task<IActionResult> UpdateDishAvailability(int id, [FromBody] bool isAvailable)
        {
            var dish = await _context.Dishes.FindAsync(id);
            if (dish == null) return NotFound();

            dish.IsAvailable = isAvailable;
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}