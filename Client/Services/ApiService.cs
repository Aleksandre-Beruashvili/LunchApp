using System.Net.Http.Json; 
using LunchApp.Shared.DTOs;

namespace LunchApp.Client.Services
{
    public class ApiService
    {
        private readonly HttpClient _http;
        public ApiService(HttpClient http) { _http = http; }

        public async Task<List<DishDto>> GetTodayMenu()
            => await _http.GetFromJsonAsync<List<DishDto>>("api/menu/today");

        public async Task<List<TimeSlotDto>> GetTimeSlots(DateTime date)
            => await _http.GetFromJsonAsync<List<TimeSlotDto>>($"api/timeslot?date={date:yyyy-MM-dd}");

        public async Task<OrderResult> PlaceOrder(OrderCreateDto dto)
        {
            var res = await _http.PostAsJsonAsync("api/orders", dto);
            return await res.Content.ReadFromJsonAsync<OrderResult>();
        }
    }
}