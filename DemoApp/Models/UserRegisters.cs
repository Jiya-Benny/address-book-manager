using DemoApp.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace DemoApp.Models
{
    public class UserRegisters
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime? CreatedAt { get; set; }

    }
}
