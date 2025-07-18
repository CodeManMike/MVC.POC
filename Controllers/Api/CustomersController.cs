using Microsoft.AspNetCore.Mvc;
using MVC.POC.Models;
using MVC.POC.Services;

namespace MVC.POC.Controllers.Api
{
    /// <summary>
    /// API controller for managing customers
    /// </summary>
    /// <remarks>
    /// This controller demonstrates comprehensive customer management API functionality using MVC pattern
    /// </remarks>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CustomersController : ControllerBase
    {
        #region Private Fields

        private readonly ICustomerService _customerService;
        private readonly ILogger<CustomersController> _logger;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the CustomersController
        /// </summary>
        /// <param name="customerService">The customer service</param>
        /// <param name="logger">The logger instance</param>
        public CustomersController(ICustomerService customerService, ILogger<CustomersController> logger)
        {
            _customerService = customerService;
            _logger = logger;
        }

        #endregion

        #region GET Methods

        /// <summary>
        /// Retrieves all customers
        /// </summary>
        /// <returns>A collection of all customers</returns>
        /// <response code="200">Returns the list of customers</response>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<Customer>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<Customer>>>> GetAllCustomers()
        {
            try
            {
                _logger.LogInformation("API request to get all customers");
                var customers = await _customerService.GetAllCustomersAsync();
                return Ok(ApiResponse<IEnumerable<Customer>>.SuccessResponse(customers, "Customers retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all customers");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<IEnumerable<Customer>>.ErrorResponse("An error occurred while retrieving customers"));
            }
        }

        /// <summary>
        /// Retrieves a customer by ID
        /// </summary>
        /// <param name="id">The customer ID</param>
        /// <returns>The customer if found</returns>
        /// <response code="200">Returns the customer</response>
        /// <response code="404">Customer not found</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<Customer>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<Customer>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<Customer>>> GetCustomer(int id)
        {
            try
            {
                _logger.LogInformation("API request to get customer with ID: {CustomerId}", id);
                var customer = await _customerService.GetCustomerByIdAsync(id);

                if (customer == null)
                {
                    return NotFound(ApiResponse<Customer>.ErrorResponse($"Customer with ID {id} not found"));
                }

                return Ok(ApiResponse<Customer>.SuccessResponse(customer, "Customer retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving customer with ID: {CustomerId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<Customer>.ErrorResponse("An error occurred while retrieving the customer"));
            }
        }

        /// <summary>
        /// Retrieves a customer by email address
        /// </summary>
        /// <param name="email">The customer email address</param>
        /// <returns>The customer if found</returns>
        /// <response code="200">Returns the customer</response>
        /// <response code="404">Customer not found</response>
        [HttpGet("email/{email}")]
        [ProducesResponseType(typeof(ApiResponse<Customer>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<Customer>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<Customer>>> GetCustomerByEmail(string email)
        {
            try
            {
                _logger.LogInformation("API request to get customer with email: {Email}", email);
                var customer = await _customerService.GetCustomerByEmailAsync(email);

                if (customer == null)
                {
                    return NotFound(ApiResponse<Customer>.ErrorResponse($"Customer with email {email} not found"));
                }

                return Ok(ApiResponse<Customer>.SuccessResponse(customer, "Customer retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving customer with email: {Email}", email);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<Customer>.ErrorResponse("An error occurred while retrieving the customer"));
            }
        }

        /// <summary>
        /// Retrieves customers by country
        /// </summary>
        /// <param name="country">The country name</param>
        /// <returns>A collection of customers in the specified country</returns>
        /// <response code="200">Returns the list of customers in the country</response>
        [HttpGet("country/{country}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<Customer>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<Customer>>>> GetCustomersByCountry(string country)
        {
            try
            {
                _logger.LogInformation("API request to get customers in country: {Country}", country);
                var customers = await _customerService.GetCustomersByCountryAsync(country);
                return Ok(ApiResponse<IEnumerable<Customer>>.SuccessResponse(customers,
                    $"Customers in country '{country}' retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving customers in country: {Country}", country);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<IEnumerable<Customer>>.ErrorResponse("An error occurred while retrieving customers"));
            }
        }

        /// <summary>
        /// Searches customers by name or email
        /// </summary>
        /// <param name="searchTerm">The search term</param>
        /// <returns>A collection of matching customers</returns>
        /// <response code="200">Returns the list of matching customers</response>
        [HttpGet("search")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<Customer>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<Customer>>>> SearchCustomers([FromQuery] string searchTerm)
        {
            try
            {
                _logger.LogInformation("API request to search customers with term: {SearchTerm}", searchTerm);
                var customers = await _customerService.SearchCustomersAsync(searchTerm);
                return Ok(ApiResponse<IEnumerable<Customer>>.SuccessResponse(customers, "Customer search completed successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching customers with term: {SearchTerm}", searchTerm);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<IEnumerable<Customer>>.ErrorResponse("An error occurred while searching customers"));
            }
        }

        #endregion

        #region POST Methods

        /// <summary>
        /// Creates a new customer
        /// </summary>
        /// <param name="customer">The customer to create</param>
        /// <returns>The created customer</returns>
        /// <response code="201">Customer created successfully</response>
        /// <response code="400">Invalid customer data or email already exists</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<Customer>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<Customer>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<Customer>>> CreateCustomer([FromBody] Customer customer)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return BadRequest(ApiResponse<Customer>.ErrorResponse("Invalid customer data", errors));
                }

                // Check if email is already in use
                var emailAvailable = await _customerService.IsEmailAvailableAsync(customer.Email);
                if (!emailAvailable)
                {
                    return BadRequest(ApiResponse<Customer>.ErrorResponse("Email address is already in use"));
                }

                _logger.LogInformation("API request to create customer: {CustomerName}", customer.FullName);
                var createdCustomer = await _customerService.CreateCustomerAsync(customer);

                return CreatedAtAction(nameof(GetCustomer), new { id = createdCustomer.Id },
                    ApiResponse<Customer>.SuccessResponse(createdCustomer, "Customer created successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating customer: {CustomerName}", customer?.FullName);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<Customer>.ErrorResponse("An error occurred while creating the customer"));
            }
        }

        #endregion

        #region PUT Methods

        /// <summary>
        /// Updates an existing customer
        /// </summary>
        /// <param name="id">The customer ID</param>
        /// <param name="customer">The updated customer data</param>
        /// <returns>The updated customer</returns>
        /// <response code="200">Customer updated successfully</response>
        /// <response code="400">Invalid customer data or email already exists</response>
        /// <response code="404">Customer not found</response>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<Customer>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<Customer>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<Customer>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<Customer>>> UpdateCustomer(int id, [FromBody] Customer customer)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return BadRequest(ApiResponse<Customer>.ErrorResponse("Invalid customer data", errors));
                }

                // Check if email is available (excluding current customer)
                var emailAvailable = await _customerService.IsEmailAvailableAsync(customer.Email, id);
                if (!emailAvailable)
                {
                    return BadRequest(ApiResponse<Customer>.ErrorResponse("Email address is already in use"));
                }

                _logger.LogInformation("API request to update customer with ID: {CustomerId}", id);
                var updatedCustomer = await _customerService.UpdateCustomerAsync(id, customer);

                if (updatedCustomer == null)
                {
                    return NotFound(ApiResponse<Customer>.ErrorResponse($"Customer with ID {id} not found"));
                }

                return Ok(ApiResponse<Customer>.SuccessResponse(updatedCustomer, "Customer updated successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating customer with ID: {CustomerId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<Customer>.ErrorResponse("An error occurred while updating the customer"));
            }
        }

        #endregion

        #region DELETE Methods

        /// <summary>
        /// Deletes a customer
        /// </summary>
        /// <param name="id">The customer ID</param>
        /// <returns>Success message</returns>
        /// <response code="200">Customer deleted successfully</response>
        /// <response code="404">Customer not found</response>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> DeleteCustomer(int id)
        {
            try
            {
                _logger.LogInformation("API request to delete customer with ID: {CustomerId}", id);
                var result = await _customerService.DeleteCustomerAsync(id);

                if (!result)
                {
                    return NotFound(ApiResponse.ErrorResponse($"Customer with ID {id} not found"));
                }

                return Ok(ApiResponse.SuccessResult("Customer deleted successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting customer with ID: {CustomerId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse.ErrorResponse("An error occurred while deleting the customer"));
            }
        }

        #endregion

        #region Validation Methods

        /// <summary>
        /// Validates if an email address is available
        /// </summary>
        /// <param name="email">The email address to validate</param>
        /// <param name="excludeCustomerId">Customer ID to exclude from validation (optional)</param>
        /// <returns>Availability status</returns>
        /// <response code="200">Returns email availability status</response>
        [HttpGet("validate-email")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<bool>>> ValidateEmail([FromQuery] string email, [FromQuery] int? excludeCustomerId = null)
        {
            try
            {
                _logger.LogInformation("API request to validate email availability: {Email}", email);
                var isAvailable = await _customerService.IsEmailAvailableAsync(email, excludeCustomerId);

                return Ok(ApiResponse<bool>.SuccessResponse(isAvailable,
                    isAvailable ? "Email is available" : "Email is already in use"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating email: {Email}", email);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<bool>.ErrorResponse("An error occurred while validating the email"));
            }
        }

        #endregion
    }
}
