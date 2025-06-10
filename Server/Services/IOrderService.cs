using System.Collections.Generic;
using System.Threading.Tasks;
using LunchApp.Shared.DTOs;
using OfficeCafeApp.API.DTOs;

namespace OfficeCafeApp.API.Services
{
    public interface IOrderService
    {
        Task<OrderResult> CreateOrderAsync(int userId, OrderCreateDto orderDto);
        Task<IEnumerable<OrderDto>> GetUserOrdersAsync(int userId);
        Task<bool> CancelOrderAsync(int userId, Guid orderId);
    }

    public class OrderResult
    {
        public Guid OrderId { get; set; }
        public string QRCode { get; set; }
        public string PickupTime { get; set; }
    }
}