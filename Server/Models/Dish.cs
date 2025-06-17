using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OfficeCafeApp.API.Models
{
    public class Dish
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required, Range(0.01, 1000)]
        public decimal Price { get; set; }

        [StringLength(255)]
        public string? ImageUrl { get; set; }

        public bool IsAvailable { get; set; } = true;

        [Required]
        [RegularExpression("^(g|ml|pcs)$", ErrorMessage = "Portion must be 'g', 'ml' or 'pcs'.")]
        public string Portion { get; set; } = "g";

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal PortionSize { get; set; } // Actual size (250, 500, etc.)

        public ICollection<MenuSchedule> MenuSchedules { get; set; } = new List<MenuSchedule>();
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}