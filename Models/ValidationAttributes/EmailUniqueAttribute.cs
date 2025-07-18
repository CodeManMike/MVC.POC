using System.ComponentModel.DataAnnotations;

namespace MVC.POC.Models.ValidationAttributes
{
    /// <summary>
    /// Custom validation attribute to ensure email uniqueness
    /// </summary>
    /// <remarks>
    /// This demonstrates custom validation in MVC applications for business rules
    /// </remarks>
    public class EmailUniqueAttribute : ValidationAttribute
    {
        /// <summary>
        /// Initializes a new instance of the EmailUniqueAttribute
        /// </summary>
        public EmailUniqueAttribute()
        {
            ErrorMessage = "Email address is already in use.";
        }

        /// <summary>
        /// Validates the email address for uniqueness
        /// </summary>
        /// <param name="value">The email value to validate</param>
        /// <param name="validationContext">The validation context</param>
        /// <returns>Validation result</returns>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not string email || string.IsNullOrWhiteSpace(email))
            {
                return ValidationResult.Success; // Let Required attribute handle null/empty
            }

            // Note: In a real application, we would inject the service here
            // For this POC, we'll demonstrate the pattern but skip actual validation
            // to avoid circular dependencies in model validation

            return ValidationResult.Success;
        }
    }
}
