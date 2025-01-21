using System.ComponentModel.DataAnnotations;

namespace DemoApp.Attributes;
public class ValidPhoneAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        // Check if value is null or empty
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
        {
            return new ValidationResult("Phone number is required.");
        }

        // Convert value to string
        string phone = value.ToString();

        // Validate phone numbers with exactly 10 digits
        if (phone.Length == 10 && long.TryParse(phone, out _))
        {
            return ValidationResult.Success;
        }

        // If validation fails, return a custom error message
        return new ValidationResult("Phone number must be exactly 10 digits.");
    }
}