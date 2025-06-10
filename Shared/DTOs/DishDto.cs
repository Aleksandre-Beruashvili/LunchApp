namespace LunchApp.Shared.DTOs
{
    public class DishDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public bool Available { get; set; }
    }
}
