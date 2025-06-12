namespace LunchApp.Shared.DTOs
{
    public class OrderItemDto
    {
        public int DishId { get; set; }
        public string DishName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}