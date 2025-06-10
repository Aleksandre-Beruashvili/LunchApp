using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using OfficeCafeApp.API.DTOs;
using OfficeCafeApp.API.Services;

namespace OfficeCafeApp.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDto orderDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var result = await _orderService.CreateOrderAsync(userId, orderDto);
            return Ok(result);
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetUserOrders()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var orders = await _orderService.GetUserOrdersAsync(userId);
            return Ok(orders);
        }

        [HttpPatch("{orderId}/cancel")]
        public async Task<IActionResult> CancelOrder(Guid orderId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var result = await _orderService.CancelOrderAsync(userId, orderId);
            return result ? Ok() : BadRequest("Cancellation failed");
        }
    }
}