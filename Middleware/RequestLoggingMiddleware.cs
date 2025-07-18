using System.Diagnostics;
using System.Text;

namespace MVC.POC.Middleware
{
    /// <summary>
    /// Middleware for logging HTTP requests and responses
    /// </summary>
    /// <remarks>
    /// This middleware demonstrates request/response logging for better observability in MVC applications
    /// </remarks>
    public class RequestLoggingMiddleware
    {
        #region Private Fields

        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the RequestLoggingMiddleware
        /// </summary>
        /// <param name="next">The next middleware in the pipeline</param>
        /// <param name="logger">The logger instance</param>
        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
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
            var stopwatch = Stopwatch.StartNew();
            var requestId = Guid.NewGuid().ToString("N")[..8];

            // Log the incoming request
            await LogRequestAsync(context, requestId);

            // Store original response body stream
            var originalResponseBodyStream = context.Response.Body;

            try
            {
                // Replace response stream with memory stream to capture response
                using var responseBodyStream = new MemoryStream();
                context.Response.Body = responseBodyStream;

                await _next(context);

                stopwatch.Stop();

                // Log the response
                await LogResponseAsync(context, requestId, stopwatch.ElapsedMilliseconds);

                // Copy response back to original stream
                responseBodyStream.Seek(0, SeekOrigin.Begin);
                await responseBodyStream.CopyToAsync(originalResponseBodyStream);
            }
            finally
            {
                context.Response.Body = originalResponseBodyStream;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Logs the incoming request details
        /// </summary>
        /// <param name="context">The HTTP context</param>
        /// <param name="requestId">The request identifier</param>
        /// <returns>A task representing the asynchronous operation</returns>
        private async Task LogRequestAsync(HttpContext context, string requestId)
        {
            try
            {
                var request = context.Request;
                var requestBody = string.Empty;

                // Read request body if it exists and is not too large
                if (request.Body != null && request.Body.CanRead && HasJsonContentType(request))
                {
                    request.EnableBuffering();

                    using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
                    requestBody = await reader.ReadToEndAsync();
                    request.Body.Position = 0;

                    // Truncate if too long
                    if (requestBody.Length > 1000)
                    {
                        requestBody = requestBody[..1000] + "... (truncated)";
                    }
                }

                _logger.LogInformation(
                    "Incoming Request [{RequestId}]: {Method} {Path}{QueryString} | Content-Type: {ContentType} | User-Agent: {UserAgent} | Body: {RequestBody}",
                    requestId,
                    request.Method,
                    request.Path,
                    request.QueryString,
                    request.ContentType ?? "none",
                    request.Headers.UserAgent.ToString() ?? "unknown",
                    string.IsNullOrEmpty(requestBody) ? "none" : requestBody
                );
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to log request details for request ID: {RequestId}", requestId);
            }
        }

        /// <summary>
        /// Logs the response details
        /// </summary>
        /// <param name="context">The HTTP context</param>
        /// <param name="requestId">The request identifier</param>
        /// <param name="elapsedMilliseconds">The elapsed time in milliseconds</param>
        /// <returns>A task representing the asynchronous operation</returns>
        private async Task LogResponseAsync(HttpContext context, string requestId, long elapsedMilliseconds)
        {
            try
            {
                var response = context.Response;
                var responseBody = string.Empty;

                // Read response body if it exists and is JSON
                if (response.Body != null && response.Body.CanRead && HasJsonContentType(response))
                {
                    response.Body.Seek(0, SeekOrigin.Begin);

                    using var reader = new StreamReader(response.Body, Encoding.UTF8, leaveOpen: true);
                    responseBody = await reader.ReadToEndAsync();

                    // Truncate if too long
                    if (responseBody.Length > 1000)
                    {
                        responseBody = responseBody[..1000] + "... (truncated)";
                    }

                    response.Body.Seek(0, SeekOrigin.Begin);
                }

                var logLevel = response.StatusCode >= 400 ? LogLevel.Warning : LogLevel.Information;

                _logger.Log(logLevel,
                    "Outgoing Response [{RequestId}]: {StatusCode} | Content-Type: {ContentType} | Duration: {Duration}ms | Body: {ResponseBody}",
                    requestId,
                    response.StatusCode,
                    response.ContentType ?? "none",
                    elapsedMilliseconds,
                    string.IsNullOrEmpty(responseBody) ? "none" : responseBody
                );
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to log response details for request ID: {RequestId}", requestId);
            }
        }

        /// <summary>
        /// Checks if the request/response has JSON content type
        /// </summary>
        /// <param name="request">The HTTP request</param>
        /// <returns>True if content type is JSON</returns>
        private static bool HasJsonContentType(HttpRequest request)
        {
            return request.ContentType?.Contains("application/json", StringComparison.OrdinalIgnoreCase) == true;
        }

        /// <summary>
        /// Checks if the response has JSON content type
        /// </summary>
        /// <param name="response">The HTTP response</param>
        /// <returns>True if content type is JSON</returns>
        private static bool HasJsonContentType(HttpResponse response)
        {
            return response.ContentType?.Contains("application/json", StringComparison.OrdinalIgnoreCase) == true;
        }

        #endregion
    }
}
