using System.Net.Http.Json;
using lunchapp.Shared.DTOs;

namespace LunchApp.Client.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;
        public AuthService(HttpClient http) { _http = http; }

        public async Task<bool> Register(RegisterDto dto)
        {
            var res = await _http.PostAsJsonAsync("api/account/register", dto);
            return res.IsSuccessStatusCode;
        }
        public async Task<bool> Login(LoginDto dto)
        {
            var res = await _http.PostAsJsonAsync("api/account/login", dto);
            return res.IsSuccessStatusCode;
        }
    }
}