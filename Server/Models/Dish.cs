using System.ComponentModel.DataAnnotations;

namespace OfficeCafeApp.API.Models
{
    public class Dish
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required, Range(0.01, 1000)]
        public decimal Price { get; set; }

        [StringLength(255)]
        public string ImageUrl { get; set; }

        public bool IsAvailable { get; set; } = true;

        [StringLength(100)]
        public string ScheduleDays { get; set; } // Comma-separated: Mon,Tue,Wed
    }
}