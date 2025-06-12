using System;
using System.ComponentModel.DataAnnotations;

namespace OfficeCafeApp.API.Models
{
    public class Rating
    {
        public int Id { get; set; }

        [Range(1, 5)]
        public int Score { get; set; }  // Score between 1 and 5

        public string Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign Keys
        public int UserId { get; set; }
        public User User { get; set; }

        public int DishId { get; set; }
        public Dish Dish { get; set; }
    }
}
