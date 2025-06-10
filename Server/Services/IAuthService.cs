using System.Threading.Tasks;
using LunchApp.Shared.DTOs;
using OfficeCafeApp.API.Models;

namespace OfficeCafeApp.API.Services
{
    /// <summary>
    /// Service providing authentication operations.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>Checks if a user with the given email exists.</summary>
        Task<bool> UserExists(string email);

        /// <summary>Registers a new user.</summary>
        Task<User> Register(RegisterDto registerDto);

        /// <summary>Logs a user in and returns a JWT token when successful.</summary>
        Task<string> Login(LoginDto loginDto);
    }
}