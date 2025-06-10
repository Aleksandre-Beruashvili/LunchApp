using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OfficeCafeApp.API.Models;

namespace OfficeCafeApp.API.Services
{
    public interface IAdminService
    {
        Task<IEnumerable<Order>> GetOrdersByDateAsync(DateTime date);
        Task<bool> UpdateDishAvailabilityAsync(int dishId, bool isAvailable);
    }
}
