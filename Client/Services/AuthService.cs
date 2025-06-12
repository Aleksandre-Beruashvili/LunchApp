using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using LunchApp.Shared.DTOs;
using System.Net.Http.Headers;

namespace LunchApp.Client.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;
        private readonly JwtAuthStateProvider _authProvider;

        public AuthService(HttpClient http, JwtAuthStateProvider authProvider)
        {
            _http = http;
            _authProvider = authProvider;
        }

        // Register user
        public async Task<bool> Register(RegisterDto dto)
        {
            var res = await _http.PostAsJsonAsync("api/account/register", dto);
            return res.IsSuccessStatusCode;
        }

        // Login user
        public async Task<bool> Login(LoginDto dto)
        {
            var res = await _http.PostAsJsonAsync("api/account/login", dto);

            if (!res.IsSuccessStatusCode)
                return false;

            var result = await res.Content.ReadFromJsonAsync<LoginResponse>();

            if (result != null && !string.IsNullOrWhiteSpace(result.Token))
            {
                await _authProvider.SetTokenAsync(result.Token);

                // Optional: Set token in Authorization header
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", result.Token);

                return true;
            }

            return false;
        }

        public Task Logout() => _authProvider.LogoutAsync();

        private class LoginResponse
        {
            public string Token { get; set; }
        }
    }
}
