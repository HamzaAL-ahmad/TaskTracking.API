using System.ComponentModel.DataAnnotations;

namespace TaskTracking.Domain.Entites.User.Dtos
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "User Name is required")]
        public string? Username { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

        public string? FullName { get; set; }
        public string? Department { get; set; }
    }
}


