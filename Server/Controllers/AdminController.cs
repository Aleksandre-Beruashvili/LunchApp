using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeCafeApp.API.Services;

namespace OfficeCafeApp.API.Controllers
{
    [Authorize(Roles = "Manager")]
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

        [HttpPatch("dishes/{id}/availability")]
        public async Task<IActionResult> UpdateDishAvailability(int id, [FromBody] bool isAvailable)
        {
            var result = await _adminService.UpdateDishAvailabilityAsync(id, isAvailable);
            if (!result) return NotFound();
            return Ok();
        }
    }
}
