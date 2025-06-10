using System;
using System.Collections.Generic;

namespace LunchApp.Shared.DTOs
{
    public class OrderDto
    {
        public int UserId { get; set; }
        public List<int> DishIds { get; set; }
        public string PickupTime { get; set; }
        public DateTime Date { get; set; }
    }
}