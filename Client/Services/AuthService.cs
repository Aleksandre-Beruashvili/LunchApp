using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using LunchApp.Shared.DTOs;

namespace LunchApp.Client.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;
        private readonly JwtAuthStateProvider _authProv;

        public AuthService(HttpClient http, JwtAuthStateProvider authProv)
        {
            _http = http;
            _authProv = authProv;
        }

        public async Task<(bool Success, string Message)> Login(LoginDto dto)
        {
            var res = await _http.PostAsJsonAsync("api/account/login", dto);
            if (!res.IsSuccessStatusCode)
            {
                var err = await res.Content.ReadAsStringAsync();
                return (false,
                    string.IsNullOrWhiteSpace(err)
                      ? "Invalid credentials."
                      : err);
            }

            var data = await res.Content.ReadFromJsonAsync<LoginResponse>();
            if (data?.Token == null)
                return (false, "No token returned.");

            await _authProv.SetTokenAsync(data.Token);
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", data.Token);

            return (true, "");
        }

        public async Task<(bool Success, string Message)> Register(RegisterDto dto)
        {
            var res = await _http.PostAsJsonAsync("api/account/register", dto);
            if (res.IsSuccessStatusCode)
                return (true, "");

            var err = await res.Content.ReadAsStringAsync();
            return (false,
                string.IsNullOrWhiteSpace(err)
                  ? "Registration failed."
                  : err);
        }

        public Task Logout() => _authProv.LogoutAsync();

        private class LoginResponse { public string Token { get; set; } }
    }
}
