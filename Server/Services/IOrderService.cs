using System.Collections.Generic;
using System.Threading.Tasks;
using LunchApp.Shared.DTOs;

namespace OfficeCafeApp.API.Services
{
    /// <summary>
    /// Service for handling order operations.
    /// </summary>
    public interface IOrderService
    {
        /// <summary>Creates a new order for the specified user.</summary>
        Task<OrderResult> CreateOrderAsync(int userId, OrderCreateDto orderDto);

        /// <summary>Retrieves all orders for a user.</summary>
        Task<IEnumerable<OrderDto>> GetUserOrdersAsync(int userId);

        /// <summary>Cancels an existing order.</summary>
        Task<bool> CancelOrderAsync(int userId, Guid orderId);
    }

    // OrderResult is defined in LunchApp.Shared.DTOs
}