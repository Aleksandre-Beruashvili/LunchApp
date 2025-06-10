using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LunchApp.Shared.Models;

namespace OfficeCafeApp.API.Models
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public int SlotId { get; set; }
        public Slot Slot { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [StringLength(20)]
        public string Status { get; set; } = "Preparing"; // Preparing, Ready, PickedUp, Cancelled

        [StringLength(100)]
        public string QRCode { get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }
}