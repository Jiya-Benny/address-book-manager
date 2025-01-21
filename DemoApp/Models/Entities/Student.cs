using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoApp.Models.Entities
{
    public class Student
    {
        [Key]
        public int UserId { get; set; }  // Foreign key to UserRegister table
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }

        // Navigation property to UserRegister (assumes one-to-one or many-to-one relationship)
        [ForeignKey("UserId")]
        public UserRegisters UserRegister { get; set; }
    }
}
