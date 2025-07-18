using Microsoft.AspNetCore.Mvc;
using MVC.POC.Models;
using MVC.POC.Services;
using System.ComponentModel.DataAnnotations;

namespace MVC.POC.Controllers.Api
{
    /// <summary>
    /// API controller for managing products
    /// </summary>
    /// <remarks>
    /// This controller demonstrates comprehensive REST API functionality using MVC pattern
    /// </remarks>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ProductsController : ControllerBase
    {
        #region Private Fields

        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the ProductsController
        /// </summary>
        /// <param name="productService">The product service</param>
        /// <param name="logger">The logger instance</param>
        public ProductsController(IProductService productService, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        #endregion

        #region GET Methods

        /// <summary>
        /// Retrieves all products
        /// </summary>
        /// <returns>A collection of all products</returns>
        /// <response code="200">Returns the list of products</response>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<Product>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<Product>>>> GetAllProducts()
        {
            try
            {
                _logger.LogInformation("API request to get all products");
                var products = await _productService.GetAllProductsAsync();
                return Ok(ApiResponse<IEnumerable<Product>>.SuccessResponse(products, "Products retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all products");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<IEnumerable<Product>>.ErrorResponse("An error occurred while retrieving products"));
            }
        }

        /// <summary>
        /// Retrieves a product by ID
        /// </summary>
        /// <param name="id">The product ID</param>
        /// <returns>The product if found</returns>
        /// <response code="200">Returns the product</response>
        /// <response code="404">Product not found</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<Product>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<Product>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<Product>>> GetProduct(int id)
        {
            try
            {
                _logger.LogInformation("API request to get product with ID: {ProductId}", id);
                var product = await _productService.GetProductByIdAsync(id);

                if (product == null)
                {
                    return NotFound(ApiResponse<Product>.ErrorResponse($"Product with ID {id} not found"));
                }

                return Ok(ApiResponse<Product>.SuccessResponse(product, "Product retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving product with ID: {ProductId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<Product>.ErrorResponse("An error occurred while retrieving the product"));
            }
        }

        /// <summary>
        /// Retrieves products by category
        /// </summary>
        /// <param name="category">The product category</param>
        /// <returns>A collection of products in the specified category</returns>
        /// <response code="200">Returns the list of products in the category</response>
        [HttpGet("category/{category}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<Product>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<Product>>>> GetProductsByCategory(string category)
        {
            try
            {
                _logger.LogInformation("API request to get products in category: {Category}", category);
                var products = await _productService.GetProductsByCategoryAsync(category);
                return Ok(ApiResponse<IEnumerable<Product>>.SuccessResponse(products,
                    $"Products in category '{category}' retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving products in category: {Category}", category);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<IEnumerable<Product>>.ErrorResponse("An error occurred while retrieving products"));
            }
        }

        /// <summary>
        /// Searches products by name or description
        /// </summary>
        /// <param name="searchTerm">The search term</param>
        /// <returns>A collection of matching products</returns>
        /// <response code="200">Returns the list of matching products</response>
        [HttpGet("search")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<Product>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<Product>>>> SearchProducts([FromQuery] string searchTerm)
        {
            try
            {
                _logger.LogInformation("API request to search products with term: {SearchTerm}", searchTerm);
                var products = await _productService.SearchProductsAsync(searchTerm);
                return Ok(ApiResponse<IEnumerable<Product>>.SuccessResponse(products, "Product search completed successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching products with term: {SearchTerm}", searchTerm);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<IEnumerable<Product>>.ErrorResponse("An error occurred while searching products"));
            }
        }

        /// <summary>
        /// Retrieves products with low stock
        /// </summary>
        /// <param name="threshold">The stock threshold (default: 10)</param>
        /// <returns>A collection of products with low stock</returns>
        /// <response code="200">Returns the list of products with low stock</response>
        [HttpGet("low-stock")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<Product>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<Product>>>> GetLowStockProducts([FromQuery] int threshold = 10)
        {
            try
            {
                _logger.LogInformation("API request to get products with stock below: {Threshold}", threshold);
                var products = await _productService.GetLowStockProductsAsync(threshold);
                return Ok(ApiResponse<IEnumerable<Product>>.SuccessResponse(products,
                    $"Products with stock below {threshold} retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving low stock products with threshold: {Threshold}", threshold);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<IEnumerable<Product>>.ErrorResponse("An error occurred while retrieving low stock products"));
            }
        }

        #endregion

        #region POST Methods

        /// <summary>
        /// Creates a new product
        /// </summary>
        /// <param name="product">The product to create</param>
        /// <returns>The created product</returns>
        /// <response code="201">Product created successfully</response>
        /// <response code="400">Invalid product data</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<Product>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<Product>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<Product>>> CreateProduct([FromBody] Product product)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return BadRequest(ApiResponse<Product>.ErrorResponse("Invalid product data", errors));
                }

                _logger.LogInformation("API request to create product: {ProductName}", product.Name);
                var createdProduct = await _productService.CreateProductAsync(product);

                return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id },
                    ApiResponse<Product>.SuccessResponse(createdProduct, "Product created successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product: {ProductName}", product?.Name);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<Product>.ErrorResponse("An error occurred while creating the product"));
            }
        }

        #endregion

        #region PUT Methods

        /// <summary>
        /// Updates an existing product
        /// </summary>
        /// <param name="id">The product ID</param>
        /// <param name="product">The updated product data</param>
        /// <returns>The updated product</returns>
        /// <response code="200">Product updated successfully</response>
        /// <response code="400">Invalid product data</response>
        /// <response code="404">Product not found</response>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<Product>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<Product>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<Product>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<Product>>> UpdateProduct(int id, [FromBody] Product product)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return BadRequest(ApiResponse<Product>.ErrorResponse("Invalid product data", errors));
                }

                _logger.LogInformation("API request to update product with ID: {ProductId}", id);
                var updatedProduct = await _productService.UpdateProductAsync(id, product);

                if (updatedProduct == null)
                {
                    return NotFound(ApiResponse<Product>.ErrorResponse($"Product with ID {id} not found"));
                }

                return Ok(ApiResponse<Product>.SuccessResponse(updatedProduct, "Product updated successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product with ID: {ProductId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<Product>.ErrorResponse("An error occurred while updating the product"));
            }
        }

        #endregion

        #region DELETE Methods

        /// <summary>
        /// Deletes a product
        /// </summary>
        /// <param name="id">The product ID</param>
        /// <returns>Success message</returns>
        /// <response code="200">Product deleted successfully</response>
        /// <response code="404">Product not found</response>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> DeleteProduct(int id)
        {
            try
            {
                _logger.LogInformation("API request to delete product with ID: {ProductId}", id);
                var result = await _productService.DeleteProductAsync(id);

                if (!result)
                {
                    return NotFound(ApiResponse.ErrorResponse($"Product with ID {id} not found"));
                }

                return Ok(ApiResponse.SuccessResult("Product deleted successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product with ID: {ProductId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse.ErrorResponse("An error occurred while deleting the product"));
            }
        }

        #endregion
    }
}
