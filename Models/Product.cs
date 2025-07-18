using System.ComponentModel.DataAnnotations;

namespace MVC.POC.Models
{
    /// <summary>
    /// Represents a product entity in the system
    /// </summary>
    /// <remarks>
    /// This model demonstrates typical business entity structure with validation
    /// </remarks>
    public class Product
    {
        /// <summary>
        /// Gets or sets the unique identifier for the product
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the product name
        /// </summary>
        [Required(ErrorMessage = "Product name is required")]
        [StringLength(100, ErrorMessage = "Product name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the product description
        /// </summary>
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the product price
        /// </summary>
        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the product category
        /// </summary>
        [Required(ErrorMessage = "Category is required")]
        [StringLength(50, ErrorMessage = "Category cannot exceed 50 characters")]
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the stock quantity
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative")]
        public int StockQuantity { get; set; }

        /// <summary>
        /// Gets or sets whether the product is active
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets the creation date
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the last update date
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
