using Microsoft.AspNetCore.Mvc;
using OfficeCafeApp.API.Data;
using OfficeCafeApp.API.DTOs;

namespace LunchApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingController : ControllerBase
    {
        private readonly AppDbContext _context;
        public RatingController(AppDbContext context) { _context = context; }

        [HttpPost]
        public async Task<IActionResult> Submit([FromBody] RatingDto dto)
        {
            var rating = new LunchApp.Shared.Models.Rating
            {
                UserId = dto.UserId,
                DishId = dto.DishId,
                Stars = dto.Stars,
                Comment = dto.Comment
            };
            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();
            return Ok(rating);
        }
    }
}