using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeCafeApp.API.Data;
using OfficeCafeApp.API.Models;
using LunchApp.Shared.DTOs;

namespace OfficeCafeApp.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class DishesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly string _uploadPath;

        public DishesController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
            _uploadPath = Path.Combine(env.WebRootPath, "uploads");
            Directory.CreateDirectory(_uploadPath);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var dishes = await _context.Dishes
                .Select(d => new DishDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    Price = d.Price,
                    ImageUrl = d.ImageUrl,
                    Portion = d.Portion,
                    Available = d.IsAvailable
                })
                .ToListAsync();

            return Ok(dishes);
        }

        [HttpGet("today")]
        public async Task<IActionResult> GetToday()
        {
            try
            {
                var today = DateTime.Today;

                var dishes = await _context.MenuSchedules
                    .Where(m => m.Day.Date == today && m.Dish.IsAvailable)
                    .Include(m => m.Dish)
                    .Select(m => new DishDto
                    {
                        Id = m.Dish.Id,
                        Name = m.Dish.Name,
                        Description = m.Dish.Description,
                        Price = m.Dish.Price,
                        ImageUrl = m.Dish.ImageUrl,
                        Portion = m.Dish.Portion,
                        Available = m.Dish.IsAvailable
                    })
                    .ToListAsync();

                return Ok(dishes);
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddDish([FromForm] DishUploadDto dto, [FromForm] IFormFile? image)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                string? imageUrl = null;

                if (image != null)
                {
                    var fileName = $"{Guid.NewGuid():N}{Path.GetExtension(image.FileName)}";
                    var filePath = Path.Combine(_uploadPath, fileName);

                    using var stream = new FileStream(filePath, FileMode.Create);
                    await image.CopyToAsync(stream);

                    imageUrl = $"/uploads/{fileName}";
                }

                var dish = new Dish
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    Price = dto.Price,
                    Portion = dto.Portion,
                    ImageUrl = imageUrl,
                    IsAvailable = true
                };

                _context.Dishes.Add(dish);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetAll), new { id = dish.Id }, dish);
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDish(int id, [FromForm] DishUploadDto dto, [FromForm] IFormFile? image)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var dish = await _context.Dishes.FindAsync(id);
                if (dish == null) return NotFound();

                if (image != null)
                {
                    // Delete old image
                    if (!string.IsNullOrEmpty(dish.ImageUrl))
                    {
                        var oldPath = Path.Combine(_env.WebRootPath, dish.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldPath))
                            System.IO.File.Delete(oldPath);
                    }

                    var fileName = $"{Guid.NewGuid():N}{Path.GetExtension(image.FileName)}";
                    var filePath = Path.Combine(_uploadPath, fileName);

                    using var stream = new FileStream(filePath, FileMode.Create);
                    await image.CopyToAsync(stream);

                    dish.ImageUrl = $"/uploads/{fileName}";
                }

                dish.Name = dto.Name;
                dish.Description = dto.Description;
                dish.Price = dto.Price;
                dish.Portion = dto.Portion;

                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDish(int id)
        {
            try
            {
                var dish = await _context.Dishes
                    .Include(d => d.MenuSchedules)
                    .FirstOrDefaultAsync(d => d.Id == id);

                if (dish == null) return NotFound();

                if (!string.IsNullOrEmpty(dish.ImageUrl))
                {
                    var filePath = Path.Combine(_env.WebRootPath, dish.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);
                }

                _context.Dishes.Remove(dish);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPatch("{id}/availability")]
        public async Task<IActionResult> ToggleAvailability(int id)
        {
            var dish = await _context.Dishes.FindAsync(id);
            if (dish == null) return NotFound();

            dish.IsAvailable = !dish.IsAvailable;
            await _context.SaveChangesAsync();

            return Ok(new { isAvailable = dish.IsAvailable });
        }
    }
}