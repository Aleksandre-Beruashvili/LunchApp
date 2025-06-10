using System;
using System.Collections.Generic;

namespace LunchApp.Shared.DTOs
{
    public class OrderDto
    {
        // Properties returned when retrieving an order
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public string QRCode { get; set; }
        public string Slot { get; set; }
        public List<OrderItemDto> Items { get; set; }

        // Properties used when creating an order
        public int UserId { get; set; }
        public List<int> DishIds { get; set; }
        public string PickupTime { get; set; }
        public DateTime Date { get; set; }
    }
}
