namespace LunchApp.Shared.DTOs
{
    public class RatingDto
    {
        public int UserId { get; set; }
        public int DishId { get; set; }
        public int Stars { get; set; }  // This maps to Score in the model
        public string Comment { get; set; }
    }
}
