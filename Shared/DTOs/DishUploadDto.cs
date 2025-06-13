using System.ComponentModel.DataAnnotations;

namespace LunchApp.Shared.DTOs
{
    public class DishUploadDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [Range(0.01, 1000)]
        public decimal Price { get; set; }

        [Required]
        [RegularExpression("^(g|ml|pcs)$", ErrorMessage = "Portion must be 'g', 'ml' or 'pcs'.")]
        public string Portion { get; set; } 

    }
}
