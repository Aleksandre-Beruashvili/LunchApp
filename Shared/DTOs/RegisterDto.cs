using System.ComponentModel.DataAnnotations;

namespace LunchApp.Shared.DTOs
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [RegularExpression(@"^[^@\s]+@credo\.ge$", ErrorMessage = "Only @credo.ge emails are allowed.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Full name is required.")]
        [MinLength(2, ErrorMessage = "Full name must be at least 2 characters.")]
        public string FullName { get; set; }
    }
}
