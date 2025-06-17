using System.ComponentModel.DataAnnotations;

namespace LunchApp.Shared.DTOs
{
    public class DishUploadDto
    {
        [Required(ErrorMessage = "Dish name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 1000, ErrorMessage = "Price must be between 0.01 and 1000")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Portion type is required")]
        [RegularExpression("^(g|ml|pcs)$", ErrorMessage = "Portion must be 'g', 'ml' or 'pcs'")]
        public string Portion { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Portion size must be greater than 0")]
        public decimal PortionSize { get; set; } // Actual size
    }
}