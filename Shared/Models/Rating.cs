namespace LunchApp.Shared.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int DishId { get; set; }
        public int Stars { get; set; }
        public string Comment { get; set; }
    }
}