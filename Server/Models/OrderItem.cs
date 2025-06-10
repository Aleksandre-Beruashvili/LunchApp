using System.ComponentModel.DataAnnotations;

namespace OfficeCafeApp.API.Models
{
    public class OrderItem
    {
        [Required]
        public Guid OrderId { get; set; }
        public Order Order { get; set; }

        [Required]
        public int DishId { get; set; }
        public Dish Dish { get; set; }

        public int Quantity { get; set; } = 1;
    }
}