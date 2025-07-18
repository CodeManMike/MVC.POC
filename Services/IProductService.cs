using MVC.POC.Models;

namespace MVC.POC.Services
{
    /// <summary>
    /// Defines the contract for product-related business operations
    /// </summary>
    /// <remarks>
    /// This interface demonstrates service layer abstraction for better testability and maintainability
    /// </remarks>
    public interface IProductService
    {
        /// <summary>
        /// Retrieves all active products
        /// </summary>
        /// <returns>A collection of all active products</returns>
        Task<IEnumerable<Product>> GetAllProductsAsync();

        /// <summary>
        /// Retrieves a product by its unique identifier
        /// </summary>
        /// <param name="id">The product identifier</param>
        /// <returns>The product if found, otherwise null</returns>
        Task<Product?> GetProductByIdAsync(int id);

        /// <summary>
        /// Retrieves products by category
        /// </summary>
        /// <param name="category">The product category</param>
        /// <returns>A collection of products in the specified category</returns>
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category);

        /// <summary>
        /// Creates a new product
        /// </summary>
        /// <param name="product">The product to create</param>
        /// <returns>The created product with generated ID</returns>
        Task<Product> CreateProductAsync(Product product);

        /// <summary>
        /// Updates an existing product
        /// </summary>
        /// <param name="id">The product identifier</param>
        /// <param name="product">The updated product data</param>
        /// <returns>The updated product if successful, otherwise null</returns>
        Task<Product?> UpdateProductAsync(int id, Product product);

        /// <summary>
        /// Deletes a product (soft delete by setting IsActive to false)
        /// </summary>
        /// <param name="id">The product identifier</param>
        /// <returns>True if deletion was successful, otherwise false</returns>
        Task<bool> DeleteProductAsync(int id);

        /// <summary>
        /// Searches products by name or description
        /// </summary>
        /// <param name="searchTerm">The search term</param>
        /// <returns>A collection of matching products</returns>
        Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm);

        /// <summary>
        /// Gets products with low stock (below specified threshold)
        /// </summary>
        /// <param name="threshold">The stock threshold (default: 10)</param>
        /// <returns>A collection of products with low stock</returns>
        Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold = 10);
    }
}
