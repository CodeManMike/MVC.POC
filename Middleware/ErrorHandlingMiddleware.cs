using MVC.POC.Models;
using System.Net;
using System.Text.Json;

namespace MVC.POC.Middleware
{
    /// <summary>
    /// Middleware for global error handling and consistent error responses
    /// </summary>
    /// <remarks>
    /// This middleware demonstrates centralized exception handling in MVC applications
    /// </remarks>
    public class ErrorHandlingMiddleware
    {
        #region Private Fields

        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the ErrorHandlingMiddleware
        /// </summary>
        /// <param name="next">The next middleware in the pipeline</param>
        /// <param name="logger">The logger instance</param>
        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Invokes the middleware
        /// </summary>
        /// <param name="context">The HTTP context</param>
        /// <returns>A task representing the asynchronous operation</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred while processing request {RequestPath}", context.Request.Path);
                await HandleExceptionAsync(context, ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Handles exceptions and creates appropriate responses
        /// </summary>
        /// <param name="context">The HTTP context</param>
        /// <param name="exception">The exception that occurred</param>
        /// <returns>A task representing the asynchronous operation</returns>
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            var apiResponse = exception switch
            {
                ArgumentException ex => CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid argument", ex.Message),
                UnauthorizedAccessException ex => CreateErrorResponse(HttpStatusCode.Unauthorized, "Unauthorized", ex.Message),
                FileNotFoundException ex => CreateErrorResponse(HttpStatusCode.NotFound, "Resource not found", ex.Message),
                InvalidOperationException ex => CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid operation", ex.Message),
                NotImplementedException ex => CreateErrorResponse(HttpStatusCode.NotImplemented, "Feature not implemented", ex.Message),
                TimeoutException ex => CreateErrorResponse(HttpStatusCode.RequestTimeout, "Request timeout", ex.Message),
                _ => CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal server error",
                    "An unexpected error occurred. Please try again later.")
            };

            response.StatusCode = (int)GetStatusCode(exception);

            var jsonResponse = JsonSerializer.Serialize(apiResponse, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });

            await response.WriteAsync(jsonResponse);
        }

        /// <summary>
        /// Creates an error response object
        /// </summary>
        /// <param name="statusCode">The HTTP status code</param>
        /// <param name="title">The error title</param>
        /// <param name="message">The error message</param>
        /// <returns>An API response object</returns>
        private static ApiResponse CreateErrorResponse(HttpStatusCode statusCode, string title, string message)
        {
            return new ApiResponse
            {
                Success = false,
                Message = title,
                Errors = new List<string> { message },
                Timestamp = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Gets the appropriate HTTP status code for an exception
        /// </summary>
        /// <param name="exception">The exception</param>
        /// <returns>The HTTP status code</returns>
        private static HttpStatusCode GetStatusCode(Exception exception)
        {
            return exception switch
            {
                ArgumentException => HttpStatusCode.BadRequest,
                UnauthorizedAccessException => HttpStatusCode.Unauthorized,
                FileNotFoundException => HttpStatusCode.NotFound,
                InvalidOperationException => HttpStatusCode.BadRequest,
                NotImplementedException => HttpStatusCode.NotImplemented,
                TimeoutException => HttpStatusCode.RequestTimeout,
                _ => HttpStatusCode.InternalServerError
            };
        }

        #endregion
    }
}
