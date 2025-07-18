using System.ComponentModel.DataAnnotations;

namespace MVC.POC.Models
{
    /// <summary>
    /// Represents a customer entity in the system
    /// </summary>
    /// <remarks>
    /// This model demonstrates customer management functionality for the POC
    /// </remarks>
    public class Customer
    {
        /// <summary>
        /// Gets or sets the unique identifier for the customer
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the customer's first name
        /// </summary>
        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the customer's last name
        /// </summary>
        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the customer's email address
        /// </summary>
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid email address format")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the customer's phone number
        /// </summary>
        [Phone(ErrorMessage = "Invalid phone number format")]
        [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the customer's address
        /// </summary>
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
        public string? Address { get; set; }

        /// <summary>
        /// Gets or sets the customer's city
        /// </summary>
        [StringLength(50, ErrorMessage = "City cannot exceed 50 characters")]
        public string? City { get; set; }

        /// <summary>
        /// Gets or sets the customer's country
        /// </summary>
        [StringLength(50, ErrorMessage = "Country cannot exceed 50 characters")]
        public string? Country { get; set; }

        /// <summary>
        /// Gets or sets whether the customer is active
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets the customer registration date
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the last update date
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets the customer's full name
        /// </summary>
        public string FullName => $"{FirstName} {LastName}";
    }
}
