using System.ComponentModel.DataAnnotations;

namespace DemoApp.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Username field is required.")]
        [StringLength(100, MinimumLength = 4)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
