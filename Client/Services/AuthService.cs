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

        // ─── REGISTER ──────────────────────────────────────────────────────────────
        public async Task<(bool Success, string Message)> Register(RegisterDto dto)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/account/register", dto);
                if (response.IsSuccessStatusCode)
                    return (true, "");

                // Read error text from API (e.g. "Email already exists")
                var errorText = await response.Content.ReadAsStringAsync();
                return (false, string.IsNullOrWhiteSpace(errorText)
                    ? "Registration failed."
                    : errorText);
            }
            catch
            {
                return (false, "Could not connect to server.");
            }
        }

        // ─── LOGIN ─────────────────────────────────────────────────────────────────
        public async Task<(bool Success, string Message)> Login(LoginDto dto)
        {
            try
            {
                var res = await _http.PostAsJsonAsync("api/account/login", dto);

                if (!res.IsSuccessStatusCode)
                {
                    var err = await res.Content.ReadAsStringAsync();
                    return (false, string.IsNullOrWhiteSpace(err)
                        ? "Invalid credentials."
                        : err);
                }

                var result = await res.Content.ReadFromJsonAsync<LoginResponse>();
                if (result != null && !string.IsNullOrWhiteSpace(result.Token))
                {
                    // Save and apply token
                    await _authProvider.SetTokenAsync(result.Token);
                    _http.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", result.Token);
                    return (true, "");
                }

                return (false, "Login succeeded but no token returned.");
            }
            catch
            {
                return (false, "Could not connect to server.");
            }
        }

        public Task Logout() => _authProvider.LogoutAsync();

        private class LoginResponse
        {
            public string Token { get; set; }
        }
    }
}
