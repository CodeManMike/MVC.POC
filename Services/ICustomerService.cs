using MVC.POC.Models;

namespace MVC.POC.Services
{
    /// <summary>
    /// Defines the contract for customer-related business operations
    /// </summary>
    /// <remarks>
    /// This interface demonstrates customer management service layer abstraction
    /// </remarks>
    public interface ICustomerService
    {
        /// <summary>
        /// Retrieves all active customers
        /// </summary>
        /// <returns>A collection of all active customers</returns>
        Task<IEnumerable<Customer>> GetAllCustomersAsync();

        /// <summary>
        /// Retrieves a customer by their unique identifier
        /// </summary>
        /// <param name="id">The customer identifier</param>
        /// <returns>The customer if found, otherwise null</returns>
        Task<Customer?> GetCustomerByIdAsync(int id);

        /// <summary>
        /// Retrieves a customer by their email address
        /// </summary>
        /// <param name="email">The customer email address</param>
        /// <returns>The customer if found, otherwise null</returns>
        Task<Customer?> GetCustomerByEmailAsync(string email);

        /// <summary>
        /// Creates a new customer
        /// </summary>
        /// <param name="customer">The customer to create</param>
        /// <returns>The created customer with generated ID</returns>
        Task<Customer> CreateCustomerAsync(Customer customer);

        /// <summary>
        /// Updates an existing customer
        /// </summary>
        /// <param name="id">The customer identifier</param>
        /// <param name="customer">The updated customer data</param>
        /// <returns>The updated customer if successful, otherwise null</returns>
        Task<Customer?> UpdateCustomerAsync(int id, Customer customer);

        /// <summary>
        /// Deletes a customer (soft delete by setting IsActive to false)
        /// </summary>
        /// <param name="id">The customer identifier</param>
        /// <returns>True if deletion was successful, otherwise false</returns>
        Task<bool> DeleteCustomerAsync(int id);

        /// <summary>
        /// Searches customers by name or email
        /// </summary>
        /// <param name="searchTerm">The search term</param>
        /// <returns>A collection of matching customers</returns>
        Task<IEnumerable<Customer>> SearchCustomersAsync(string searchTerm);

        /// <summary>
        /// Gets customers by country
        /// </summary>
        /// <param name="country">The country name</param>
        /// <returns>A collection of customers in the specified country</returns>
        Task<IEnumerable<Customer>> GetCustomersByCountryAsync(string country);

        /// <summary>
        /// Validates if an email address is already in use
        /// </summary>
        /// <param name="email">The email address to validate</param>
        /// <param name="excludeCustomerId">Customer ID to exclude from validation (for updates)</param>
        /// <returns>True if email is available, false if already in use</returns>
        Task<bool> IsEmailAvailableAsync(string email, int? excludeCustomerId = null);
    }
}
