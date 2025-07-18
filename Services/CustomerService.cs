using MVC.POC.Models;

namespace MVC.POC.Services
{
    /// <summary>
    /// Implementation of customer-related business operations
    /// </summary>
    /// <remarks>
    /// This service uses in-memory storage for POC purposes. In production, we would use a proper database
    /// </remarks>
    public class CustomerService : ICustomerService
    {
        #region Private Fields

        private readonly List<Customer> _customers;
        private readonly ILogger<CustomerService> _logger;
        private int _nextId;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the CustomerService
        /// </summary>
        /// <param name="logger">The logger instance</param>
        public CustomerService(ILogger<CustomerService> logger)
        {
            _logger = logger;
            _nextId = 1;
            _customers = new List<Customer>();

            // Seed some sample data for the POC
            SeedData();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Retrieves all active customers
        /// </summary>
        /// <returns>A collection of all active customers</returns>
        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            _logger.LogInformation("Retrieving all active customers");
            return await Task.FromResult(_customers.Where(c => c.IsActive).ToList());
        }

        /// <summary>
        /// Retrieves a customer by their unique identifier
        /// </summary>
        /// <param name="id">The customer identifier</param>
        /// <returns>The customer if found, otherwise null</returns>
        public async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            _logger.LogInformation("Retrieving customer with ID: {CustomerId}", id);
            return await Task.FromResult(_customers.FirstOrDefault(c => c.Id == id && c.IsActive));
        }

        /// <summary>
        /// Retrieves a customer by their email address
        /// </summary>
        /// <param name="email">The customer email address</param>
        /// <returns>The customer if found, otherwise null</returns>
        public async Task<Customer?> GetCustomerByEmailAsync(string email)
        {
            _logger.LogInformation("Retrieving customer with email: {Email}", email);
            return await Task.FromResult(_customers.FirstOrDefault(c => c.IsActive &&
                string.Equals(c.Email, email, StringComparison.OrdinalIgnoreCase)));
        }

        /// <summary>
        /// Creates a new customer
        /// </summary>
        /// <param name="customer">The customer to create</param>
        /// <returns>The created customer with generated ID</returns>
        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            _logger.LogInformation("Creating new customer: {CustomerName}", customer.FullName);

            customer.Id = _nextId++;
            customer.CreatedAt = DateTime.UtcNow;
            customer.UpdatedAt = DateTime.UtcNow;

            _customers.Add(customer);

            return await Task.FromResult(customer);
        }

        /// <summary>
        /// Updates an existing customer
        /// </summary>
        /// <param name="id">The customer identifier</param>
        /// <param name="customer">The updated customer data</param>
        /// <returns>The updated customer if successful, otherwise null</returns>
        public async Task<Customer?> UpdateCustomerAsync(int id, Customer customer)
        {
            _logger.LogInformation("Updating customer with ID: {CustomerId}", id);

            var existingCustomer = _customers.FirstOrDefault(c => c.Id == id && c.IsActive);
            if (existingCustomer == null)
            {
                return await Task.FromResult<Customer?>(null);
            }

            existingCustomer.FirstName = customer.FirstName;
            existingCustomer.LastName = customer.LastName;
            existingCustomer.Email = customer.Email;
            existingCustomer.PhoneNumber = customer.PhoneNumber;
            existingCustomer.Address = customer.Address;
            existingCustomer.City = customer.City;
            existingCustomer.Country = customer.Country;
            existingCustomer.IsActive = customer.IsActive;
            existingCustomer.UpdatedAt = DateTime.UtcNow;

            return await Task.FromResult(existingCustomer);
        }

        /// <summary>
        /// Deletes a customer (soft delete by setting IsActive to false)
        /// </summary>
        /// <param name="id">The customer identifier</param>
        /// <returns>True if deletion was successful, otherwise false</returns>
        public async Task<bool> DeleteCustomerAsync(int id)
        {
            _logger.LogInformation("Deleting customer with ID: {CustomerId}", id);

            var customer = _customers.FirstOrDefault(c => c.Id == id);
            if (customer == null)
            {
                return await Task.FromResult(false);
            }

            customer.IsActive = false;
            customer.UpdatedAt = DateTime.UtcNow;

            return await Task.FromResult(true);
        }

        /// <summary>
        /// Searches customers by name or email
        /// </summary>
        /// <param name="searchTerm">The search term</param>
        /// <returns>A collection of matching customers</returns>
        public async Task<IEnumerable<Customer>> SearchCustomersAsync(string searchTerm)
        {
            _logger.LogInformation("Searching customers with term: {SearchTerm}", searchTerm);

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetAllCustomersAsync();
            }

            var results = _customers.Where(c => c.IsActive &&
                (c.FirstName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                 c.LastName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                 c.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))).ToList();

            return await Task.FromResult(results);
        }

        /// <summary>
        /// Gets customers by country
        /// </summary>
        /// <param name="country">The country name</param>
        /// <returns>A collection of customers in the specified country</returns>
        public async Task<IEnumerable<Customer>> GetCustomersByCountryAsync(string country)
        {
            _logger.LogInformation("Retrieving customers in country: {Country}", country);

            return await Task.FromResult(_customers.Where(c => c.IsActive &&
                string.Equals(c.Country, country, StringComparison.OrdinalIgnoreCase)).ToList());
        }

        /// <summary>
        /// Validates if an email address is already in use
        /// </summary>
        /// <param name="email">The email address to validate</param>
        /// <param name="excludeCustomerId">Customer ID to exclude from validation (for updates)</param>
        /// <returns>True if email is available, false if already in use</returns>
        public async Task<bool> IsEmailAvailableAsync(string email, int? excludeCustomerId = null)
        {
            _logger.LogInformation("Checking email availability: {Email}", email);

            var existingCustomer = _customers.FirstOrDefault(c => c.IsActive &&
                string.Equals(c.Email, email, StringComparison.OrdinalIgnoreCase));

            if (existingCustomer == null)
            {
                return await Task.FromResult(true);
            }

            // If we're excluding a customer ID (for updates), check if it's the same customer
            if (excludeCustomerId.HasValue && existingCustomer.Id == excludeCustomerId.Value)
            {
                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Seeds sample data for the POC
        /// </summary>
        private void SeedData()
        {
            var sampleCustomers = new List<Customer>
            {
                new Customer { Id = _nextId++, FirstName = "John", LastName = "Doe", Email = "john.doe@email.com", PhoneNumber = "+1234567890", Address = "123 Main St", City = "New York", Country = "USA" },
                new Customer { Id = _nextId++, FirstName = "Jane", LastName = "Smith", Email = "jane.smith@email.com", PhoneNumber = "+1234567891", Address = "456 Oak Ave", City = "Los Angeles", Country = "USA" },
                new Customer { Id = _nextId++, FirstName = "Mike", LastName = "Johnson", Email = "mike.johnson@email.com", PhoneNumber = "+1234567892", Address = "789 Pine Rd", City = "Chicago", Country = "USA" },
                new Customer { Id = _nextId++, FirstName = "Sarah", LastName = "Williams", Email = "sarah.williams@email.com", PhoneNumber = "+1234567893", Address = "321 Elm St", City = "Houston", Country = "USA" },
                new Customer { Id = _nextId++, FirstName = "David", LastName = "Brown", Email = "david.brown@email.com", PhoneNumber = "+1234567894", Address = "654 Maple Dr", City = "Phoenix", Country = "USA" }
            };

            _customers.AddRange(sampleCustomers);
        }

        #endregion
    }
}
