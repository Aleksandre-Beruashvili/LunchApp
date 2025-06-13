using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OfficeCafeApp.API.Models
{
    public class MenuSchedule
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Dish")]
        public int DishId { get; set; }

        public Dish Dish { get; set; }

        [Required]
        public DateTime Day { get; set; }  // Date scheduled (e.g., 2025-06-13)
    }
}
    