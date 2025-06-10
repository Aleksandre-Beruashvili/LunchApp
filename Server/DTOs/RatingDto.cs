namespace OfficeCafeApp.API.DTOs
{
    public class RatingDto
    {
        public int UserId { get; set; }
        public int DishId { get; set; }
        public int Stars { get; set; }
        public string Comment { get; set; }
    }
}
