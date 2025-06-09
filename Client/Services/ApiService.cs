using System.Net.Http.Json;
using lunchapp.Models;
using lunchapp.Shared.DTOs;

namespace LunchApp.Client.Services
{
    public class ApiService
    {
        private readonly HttpClient _http;
        public ApiService(HttpClient http) { _http = http; }

        public async Task<List<Dish>> GetTodayMenu()
            => await _http.GetFromJsonAsync<List<Dish>>("api/menu/today");

        public async Task<List<TimeSlot>> GetTimeSlots(DateTime date)
            => await _http.GetFromJsonAsync<List<TimeSlot>>($"api/timeslot?date={date:yyyy-MM-dd}");

        public async Task<int> PlaceOrder(OrderDto dto)
        {
            var res = await _http.PostAsJsonAsync("api/order", dto);
            return await res.Content.ReadFromJsonAsync<int>();
        }
    }
}