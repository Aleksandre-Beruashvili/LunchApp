using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using LunchApp.Shared.DTOs;
using OfficeCafeApp.API.Services;

namespace OfficeCafeApp.API.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (await _authService.UserExists(registerDto.Email))
                return BadRequest("Email already exists");

            var user = await _authService.Register(registerDto);
            return Ok(new { user.Id, user.Email, user.FullName });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var token = await _authService.Login(loginDto);
            if (token == null)
                return Unauthorized("Invalid credentials");

            return Ok(new { token });
        }
    }
}
