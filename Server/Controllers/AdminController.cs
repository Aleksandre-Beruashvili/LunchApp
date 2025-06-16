using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeCafeApp.API.Services;

namespace OfficeCafeApp.API.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    [ApiController]
    [Route("api/admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("orders")]
        public async Task<IActionResult> GetOrdersByDate([FromQuery] DateTime date)
        {
            var orders = await _adminService.GetOrdersByDateAsync(date);
            return Ok(orders);
        }

        [HttpGet("orders/counts")]
        public async Task<IActionResult> GetOrderCountsByDate([FromQuery] DateTime date)
        {
            var result = await _adminService.GetOrderCountsByDateAsync(date);
            return Ok(result);
        }

        [HttpPatch("dishes/{id}/availability")]
        public async Task<IActionResult> UpdateDishAvailability(int id, [FromBody] bool isAvailable)
        {
            var result = await _adminService.UpdateDishAvailabilityAsync(id, isAvailable);
            if (!result) return NotFound();
            return Ok();
        }
    }
}
