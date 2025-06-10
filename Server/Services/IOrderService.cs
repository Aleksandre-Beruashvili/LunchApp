using System.Collections.Generic;
using System.Threading.Tasks;
using LunchApp.Shared.DTOs;

namespace OfficeCafeApp.API.Services
{
    public interface IOrderService
    {
        Task<OrderResult> CreateOrderAsync(int userId, OrderCreateDto orderDto);
        Task<IEnumerable<OrderDto>> GetUserOrdersAsync(int userId);
        Task<bool> CancelOrderAsync(int userId, Guid orderId);
    }

    // OrderResult is defined in LunchApp.Shared.DTOs
}