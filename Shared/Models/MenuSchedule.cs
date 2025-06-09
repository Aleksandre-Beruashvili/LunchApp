using System;

namespace LunchApp.Shared.Models
{
    public class MenuSchedule
    {
        public int Id { get; set; }
        public int DishId { get; set; }
        public Dish Dish { get; set; }
        public DateTime Day { get; set; }
    }
}