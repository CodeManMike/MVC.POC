namespace MVC.POC.Models
{
    /// <summary>
    /// Represents a standardized API response wrapper
    /// </summary>
    /// <typeparam name="T">The type of data being returned</typeparam>
    /// <remarks>
    /// This provides consistent response formatting across all API endpoints
    /// </remarks>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Gets or sets whether the operation was successful
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the response message
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the response data
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// Gets or sets any errors that occurred
        /// </summary>
        public List<string>? Errors { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of the response
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Creates a successful response with data
        /// </summary>
        /// <param name="data">The data to return</param>
        /// <param name="message">Optional success message</param>
        /// <returns>A successful API response</returns>
        public static ApiResponse<T> SuccessResponse(T data, string message = "Operation completed successfully")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                Timestamp = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Creates an error response
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="errors">Optional list of detailed errors</param>
        /// <returns>An error API response</returns>
        public static ApiResponse<T> ErrorResponse(string message, List<string>? errors = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Errors = errors,
                Timestamp = DateTime.UtcNow
            };
        }
    }

    /// <summary>
    /// Non-generic API response for operations that don't return data
    /// </summary>
    public class ApiResponse
    {
        /// <summary>
        /// Gets or sets whether the operation was successful
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the response message
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets any errors that occurred
        /// </summary>
        public List<string>? Errors { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of the response
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Creates a simple success response without data
        /// </summary>
        /// <param name="message">Success message</param>
        /// <returns>A successful API response</returns>
        public static ApiResponse SuccessResult(string message = "Operation completed successfully")
        {
            return new ApiResponse
            {
                Success = true,
                Message = message,
                Timestamp = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Creates a simple error response without data
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="errors">Optional list of detailed errors</param>
        /// <returns>An error API response</returns>
        public static ApiResponse ErrorResponse(string message, List<string>? errors = null)
        {
            return new ApiResponse
            {
                Success = false,
                Message = message,
                Errors = errors,
                Timestamp = DateTime.UtcNow
            };
        }
    }
}
