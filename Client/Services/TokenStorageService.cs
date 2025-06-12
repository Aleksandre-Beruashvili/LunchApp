using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace LunchApp.Client.Services
{
    public class TokenStorageService
    {
        private const string KEY = "authToken";
        private readonly IJSRuntime _js;
        public TokenStorageService(IJSRuntime js) => _js = js;

        public ValueTask SetTokenAsync(string token) =>
            _js.InvokeVoidAsync("localStorage.setItem", KEY, token);

        public ValueTask<string> GetTokenAsync() =>
            _js.InvokeAsync<string>("localStorage.getItem", KEY);

        public ValueTask RemoveTokenAsync() =>
            _js.InvokeVoidAsync("localStorage.removeItem", KEY);
    }
}
