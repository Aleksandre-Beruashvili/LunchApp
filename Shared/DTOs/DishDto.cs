﻿namespace LunchApp.Shared.DTOs
{
    public class DishDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public bool Available { get; set; }
        public string Portion { get; set; }
        public decimal PortionSize { get; set; }
        public int Leftover { get; set; }

        public bool IsAvailable
        {
            get => Available;
            set => Available = value;
        }
    }
}
