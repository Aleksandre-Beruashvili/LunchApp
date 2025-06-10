using System.ComponentModel.DataAnnotations;

namespace OfficeCafeApp.API.Models
{
    public class Slot
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }
    }
}