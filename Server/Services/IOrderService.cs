using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LunchApp.Shared.DTOs;

namespace OfficeCafeApp.API.Services
{
    public interface IOrderService
    {
        Task<OrderResult> CreateOrderAsync(int userId, OrderCreateDto dto);
        Task<IEnumerable<OrderDto>> GetUserOrdersAsync(int userId);
        Task<ServiceResult> CancelOrderAsync(int userId, Guid orderId);
    }
}