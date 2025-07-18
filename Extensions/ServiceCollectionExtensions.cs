using MVC.POC.Services;

namespace MVC.POC.Extensions
{
    /// <summary>
    /// Extension methods for service collection configuration
    /// </summary>
    /// <remarks>
    /// This demonstrates organizing service registration in MVC applications
    /// </remarks>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers business services for the POC application
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            // Register service interfaces with their implementations
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICustomerService, CustomerService>();

            return services;
        }

        /// <summary>
        /// Configures API-related services
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            // Configure controllers with JSON options
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.WriteIndented = true;
                });

            // Add API explorer for tooling support
            services.AddEndpointsApiExplorer();

            return services;
        }

        /// <summary>
        /// Configures CORS policies for the application
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddCorsConfiguration(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });

                options.AddPolicy("RestrictedCors", policy =>
                {
                    policy.WithOrigins("https://localhost:3000", "https://example.com")
                          .WithMethods("GET", "POST", "PUT", "DELETE")
                          .WithHeaders("Content-Type", "Authorization");
                });
            });

            return services;
        }

        /// <summary>
        /// Configures logging for the application
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddLoggingConfiguration(this IServiceCollection services)
        {
            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddConsole();
                builder.AddDebug();

                // Configure log levels
                builder.SetMinimumLevel(LogLevel.Information);
            });

            return services;
        }
    }
}
