using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeCafeApp.API.Data;
using OfficeCafeApp.API.Models;

namespace OfficeCafeApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuController : ControllerBase
    {
        private readonly AppDbContext _context;
        public MenuController(AppDbContext context) { _context = context; }

        [HttpGet("today")]
        public async Task<IActionResult> GetToday()
        {
            var today = DateTime.Today;
            var menu = await _context.MenuSchedules
                .Where(m => m.Day == today)
                .Include(m => m.Dish)
                .Select(m => m.Dish)
                .ToListAsync();
            return Ok(menu);
        }
    }
}