using Microsoft.JSInterop;

namespace LunchApp.Client.Services
{
    public class TokenStorageService
    {
        private readonly IJSRuntime _js;
        private const string TokenKey = "authToken";

        public TokenStorageService(IJSRuntime js)
        {
            _js = js;
        }

        public ValueTask<string?> GetTokenAsync()
            => _js.InvokeAsync<string?>("localStorage.getItem", TokenKey);

        public ValueTask SetTokenAsync(string token)
            => _js.InvokeVoidAsync("localStorage.setItem", TokenKey, token);

        public ValueTask RemoveTokenAsync()
            => _js.InvokeVoidAsync("localStorage.removeItem", TokenKey);
    }
}
