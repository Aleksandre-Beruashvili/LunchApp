using System.Security.Claims;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

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
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var identity = new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwt");
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
            await _storage.RemoveTokenAsync();
            _http.DefaultRequestHeaders.Authorization = null;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
