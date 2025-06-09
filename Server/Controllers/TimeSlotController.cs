using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LunchApp.Server.Data;

namespace LunchApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimeSlotController : ControllerBase
    {
        private readonly AppDbContext _context;
        public TimeSlotController(AppDbContext context) { _context = context; }

        [HttpGet]
        public async Task<IActionResult> GetSlots([FromQuery] DateTime date)
        {
            var slots = await _context.Orders
                .Where(o => o.Date == date)
                .GroupBy(o => o.PickupTime)
                .Select(g => new TimeSlotDto { Time = g.Key, Count = g.Count() })
                .ToListAsync();
            return Ok(slots);
        }
    }
}