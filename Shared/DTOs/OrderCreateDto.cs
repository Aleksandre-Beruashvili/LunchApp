using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LunchApp.Shared.DTOs
{
    public class OrderCreateDto
    {
        [Required]
        public int SlotId { get; set; }

        [Required]
        public List<OrderItemDto> Items { get; set; }
    }
}