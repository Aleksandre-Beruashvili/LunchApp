using System.Collections.Generic;

namespace OfficeCafeApp.API.DTOs
{
    public class OrderCreateDto
    {
        public int SlotId { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }

    public class OrderItemDto
    {
        public int DishId { get; set; }
        public int Quantity { get; set; } = 1;
    }
}