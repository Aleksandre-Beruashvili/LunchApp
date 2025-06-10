using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeCafeApp.API.Data;
using LunchApp.Shared.DTOs;

namespace OfficeCafeApp.API.Controllers
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
                .Where(o => o.OrderDate.Date == date.Date)
                .Include(o => o.Slot)
                .GroupBy(o => new { o.Slot.StartTime, o.Slot.EndTime })
                .Select(g => new TimeSlotDto
                {
                    Time = $"{g.Key.StartTime:hh\\:mm}-{g.Key.EndTime:hh\\:mm}",
                    Count = g.Count()
                })
                .ToListAsync();
            return Ok(slots);
        }
    }
}