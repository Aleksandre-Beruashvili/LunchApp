using System;
using System.Collections.Generic;
using lunchapp.Models;

namespace LunchApp.Shared.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string PickupTime { get; set; }
        public DateTime Date { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<OrderItem> Items { get; set; }
    }
}