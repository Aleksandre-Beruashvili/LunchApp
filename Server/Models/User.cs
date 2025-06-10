using System.ComponentModel.DataAnnotations;

namespace OfficeCafeApp.API.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public byte[] PasswordHash { get; set; }

        [Required]
        public byte[] PasswordSalt { get; set; }

        [Required, StringLength(100)]
        public string FullName { get; set; }

        [StringLength(20)]
        public string Role { get; set; } = "Employee";
    }
}