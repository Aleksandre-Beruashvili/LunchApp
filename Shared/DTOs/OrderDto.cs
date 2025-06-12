using System;
using System.Collections.Generic;

namespace LunchApp.Shared.DTOs
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public string QRCode { get; set; }
        public string Slot { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }
}