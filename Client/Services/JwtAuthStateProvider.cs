using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using System.Threading.Tasks;

namespace LunchApp.Client.Services
{
    public class JwtAuthStateProvider : AuthenticationStateProvider
    {
        private readonly TokenStorageService _storage;
        private readonly HttpClient _http;

        public JwtAuthStateProvider(TokenStorageService storage, HttpClient http)
        {
            _storage = storage;
            _http = http;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _storage.GetTokenAsync();
            if (!string.IsNullOrWhiteSpace(token))
            {
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                // Parse claims
                var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);
                var identity = new ClaimsIdentity(jwt.Claims, "jwt");
                return new AuthenticationState(new ClaimsPrincipal(identity));
            }

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        public async Task SetTokenAsync(string token)
        {
            await _storage.SetTokenAsync(token);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task LogoutAsync()
        {
            // 1. Remove token
            await _storage.RemoveTokenAsync();
            // 2. Clear header
            _http.DefaultRequestHeaders.Authorization = null;
            // 3. Notify
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
