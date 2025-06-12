using Microsoft.AspNetCore.Mvc;
using OfficeCafeApp.API.Data;
using LunchApp.Shared.DTOs;
using OfficeCafeApp.API.Models;
using System.Threading.Tasks;

namespace OfficeCafeApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RatingController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Submit([FromBody] RatingDto dto)
        {
            if (dto.Stars < 1 || dto.Stars > 5)
            {
                return BadRequest("Rating must be between 1 and 5.");
            }

            var rating = new Rating
            {
                UserId = dto.UserId,
                DishId = dto.DishId,
                Score = dto.Stars,  // Map Stars (DTO) to Score (Model)
                Comment = dto.Comment
            };

            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();

            return Ok(rating);
        }
    }
}
