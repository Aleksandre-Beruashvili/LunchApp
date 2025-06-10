using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OfficeCafeApp.API.Models;

namespace OfficeCafeApp.API.Services
{
    /// <summary>
    /// Service used for administrative operations.
    /// </summary>
    public interface IAdminService
    {
        /// <summary>Gets all orders for a specific date.</summary>
        Task<IEnumerable<Order>> GetOrdersByDateAsync(DateTime date);

        /// <summary>Updates availability of a dish.</summary>
        Task<bool> UpdateDishAvailabilityAsync(int dishId, bool isAvailable);
    }
}
