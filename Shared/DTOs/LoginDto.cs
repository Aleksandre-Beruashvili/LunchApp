using System.ComponentModel.DataAnnotations;

namespace LunchApp.Shared.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email (or username) is required.")] 
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
