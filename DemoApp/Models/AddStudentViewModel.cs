using DemoApp.Attributes;
using System.ComponentModel.DataAnnotations;

namespace DemoApp.Models
{
    public class AddStudentViewModel
    {

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; }

        //[Required(ErrorMessage = "Phone number is required.")]
        [ValidPhone]
        public string Phone { get; set; }

        [Required(ErrorMessage = "DOB is required.")]
        [DataType(DataType.Date)]
        public DateOnly? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "State is required.")]
        [StringLength(50, ErrorMessage = "State cannot exceed 50 characters.")]
        public string State { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        [StringLength(50, ErrorMessage = "Country cannot exceed 50 characters.")]
        public string Country { get; set; }
    }
}
