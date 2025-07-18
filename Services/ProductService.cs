using MVC.POC.Models;

namespace MVC.POC.Services
{
    /// <summary>
    /// Implementation of product-related business operations
    /// </summary>
    /// <remarks>
    /// This service uses in-memory storage for POC purposes. In production, we would use a proper database
    /// </remarks>
    public class ProductService : IProductService
    {
        #region Private Fields

        private readonly List<Product> _products;
        private readonly ILogger<ProductService> _logger;
        private int _nextId;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the ProductService
        /// </summary>
        /// <param name="logger">The logger instance</param>
        public ProductService(ILogger<ProductService> logger)
        {
            _logger = logger;
            _nextId = 1;
            _products = new List<Product>();

            // Seed some sample data for the POC
            SeedData();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Retrieves all active products
        /// </summary>
        /// <returns>A collection of all active products</returns>
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            _logger.LogInformation("Retrieving all active products");
            return await Task.FromResult(_products.Where(p => p.IsActive).ToList());
        }

        /// <summary>
        /// Retrieves a product by its unique identifier
        /// </summary>
        /// <param name="id">The product identifier</param>
        /// <returns>The product if found, otherwise null</returns>
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            _logger.LogInformation("Retrieving product with ID: {ProductId}", id);
            return await Task.FromResult(_products.FirstOrDefault(p => p.Id == id && p.IsActive));
        }

        /// <summary>
        /// Retrieves products by category
        /// </summary>
        /// <param name="category">The product category</param>
        /// <returns>A collection of products in the specified category</returns>
        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category)
        {
            _logger.LogInformation("Retrieving products in category: {Category}", category);
            return await Task.FromResult(_products.Where(p => p.IsActive &&
                string.Equals(p.Category, category, StringComparison.OrdinalIgnoreCase)).ToList());
        }

        /// <summary>
        /// Creates a new product
        /// </summary>
        /// <param name="product">The product to create</param>
        /// <returns>The created product with generated ID</returns>
        public async Task<Product> CreateProductAsync(Product product)
        {
            _logger.LogInformation("Creating new product: {ProductName}", product.Name);

            product.Id = _nextId++;
            product.CreatedAt = DateTime.UtcNow;
            product.UpdatedAt = DateTime.UtcNow;

            _products.Add(product);

            return await Task.FromResult(product);
        }

        /// <summary>
        /// Updates an existing product
        /// </summary>
        /// <param name="id">The product identifier</param>
        /// <param name="product">The updated product data</param>
        /// <returns>The updated product if successful, otherwise null</returns>
        public async Task<Product?> UpdateProductAsync(int id, Product product)
        {
            _logger.LogInformation("Updating product with ID: {ProductId}", id);

            var existingProduct = _products.FirstOrDefault(p => p.Id == id && p.IsActive);
            if (existingProduct == null)
            {
                return await Task.FromResult<Product?>(null);
            }

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.Category = product.Category;
            existingProduct.StockQuantity = product.StockQuantity;
            existingProduct.IsActive = product.IsActive;
            existingProduct.UpdatedAt = DateTime.UtcNow;

            return await Task.FromResult(existingProduct);
        }

        /// <summary>
        /// Deletes a product (soft delete by setting IsActive to false)
        /// </summary>
        /// <param name="id">The product identifier</param>
        /// <returns>True if deletion was successful, otherwise false</returns>
        public async Task<bool> DeleteProductAsync(int id)
        {
            _logger.LogInformation("Deleting product with ID: {ProductId}", id);

            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return await Task.FromResult(false);
            }

            product.IsActive = false;
            product.UpdatedAt = DateTime.UtcNow;

            return await Task.FromResult(true);
        }

        /// <summary>
        /// Searches products by name or description
        /// </summary>
        /// <param name="searchTerm">The search term</param>
        /// <returns>A collection of matching products</returns>
        public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
        {
            _logger.LogInformation("Searching products with term: {SearchTerm}", searchTerm);

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetAllProductsAsync();
            }

            var results = _products.Where(p => p.IsActive &&
                (p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                 (p.Description?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ?? false))).ToList();

            return await Task.FromResult(results);
        }

        /// <summary>
        /// Gets products with low stock (below specified threshold)
        /// </summary>
        /// <param name="threshold">The stock threshold (default: 10)</param>
        /// <returns>A collection of products with low stock</returns>
        public async Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold = 10)
        {
            _logger.LogInformation("Retrieving products with stock below: {Threshold}", threshold);

            return await Task.FromResult(_products.Where(p => p.IsActive && p.StockQuantity < threshold).ToList());
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Seeds sample data for the POC
        /// </summary>
        private void SeedData()
        {
            var sampleProducts = new List<Product>
            {
                new Product { Id = _nextId++, Name = "Laptop Pro", Description = "High-performance laptop for professionals", Price = 1299.99m, Category = "Electronics", StockQuantity = 15 },
                new Product { Id = _nextId++, Name = "Wireless Mouse", Description = "Ergonomic wireless mouse with long battery life", Price = 29.99m, Category = "Electronics", StockQuantity = 50 },
                new Product { Id = _nextId++, Name = "Office Chair", Description = "Comfortable ergonomic office chair", Price = 199.99m, Category = "Furniture", StockQuantity = 8 },
                new Product { Id = _nextId++, Name = "Coffee Maker", Description = "Premium automatic coffee maker", Price = 89.99m, Category = "Appliances", StockQuantity = 25 },
                new Product { Id = _nextId++, Name = "Smartphone", Description = "Latest smartphone with advanced features", Price = 799.99m, Category = "Electronics", StockQuantity = 30 }
            };

            _products.AddRange(sampleProducts);
        }

        #endregion
    }
}
