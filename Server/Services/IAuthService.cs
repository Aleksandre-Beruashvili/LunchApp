using System.Threading.Tasks;
using OfficeCafeApp.API.DTOs;
using OfficeCafeApp.API.Models;

namespace OfficeCafeApp.API.Services
{
    public interface IAuthService
    {
        Task<bool> UserExists(string email);
        Task<User> Register(RegisterDto registerDto);
        Task<string> Login(LoginDto loginDto);
    }
}