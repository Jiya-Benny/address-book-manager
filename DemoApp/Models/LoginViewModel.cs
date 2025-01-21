using System.ComponentModel.DataAnnotations;

namespace DemoApp.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
