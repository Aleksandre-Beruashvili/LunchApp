using System;

namespace LunchApp.Shared.DTOs
{
    public class OrderResult
    {
        public Guid OrderId { get; set; }
        public string QRCode { get; set; }
        public string PickupTime { get; set; }
    }
}
