using MVC.POC.Extensions;
using MVC.POC.Middleware;

var builder = WebApplication.CreateBuilder(args);

#region Service Configuration

// Add MVC services for traditional web pages
builder.Services.AddControllersWithViews();

// Configure API services using extension methods
builder.Services.AddApiServices();

// Register business services
builder.Services.AddBusinessServices();

// Configure CORS
builder.Services.AddCorsConfiguration();

// Configure logging
builder.Services.AddLoggingConfiguration();

#endregion

var app = builder.Build();

#region Middleware Pipeline Configuration

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    // In development, use our custom error handling middleware for API endpoints
    app.UseWhen(context => context.Request.Path.StartsWithSegments("/api"),
        appBuilder => appBuilder.UseMiddleware<ErrorHandlingMiddleware>());
}

// Add custom middleware for request logging (only for API endpoints to avoid noise)
app.UseWhen(context => context.Request.Path.StartsWithSegments("/api"),
    appBuilder => appBuilder.UseMiddleware<RequestLoggingMiddleware>());

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Enable CORS
app.UseCors("AllowAll");

app.UseAuthorization();

#endregion

#region Route Configuration

// Configure MVC routes for traditional web pages
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Map API controllers for REST endpoints
app.MapControllers();

#endregion

#region Application Startup Information

// Log startup information
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("=== MVC POC Application Starting ===");
logger.LogInformation("Environment: {Environment}", app.Environment.EnvironmentName);
logger.LogInformation("Available API endpoints:");
logger.LogInformation("  Products API: /api/products");
logger.LogInformation("    GET    /api/products               - Get all products");
logger.LogInformation("    GET    /api/products/{{id}}          - Get product by ID");
logger.LogInformation("    GET    /api/products/category/{{name}} - Get products by category");
logger.LogInformation("    GET    /api/products/search?searchTerm={{term}} - Search products");
logger.LogInformation("    GET    /api/products/low-stock     - Get low stock products");
logger.LogInformation("    POST   /api/products               - Create new product");
logger.LogInformation("    PUT    /api/products/{{id}}          - Update product");
logger.LogInformation("    DELETE /api/products/{{id}}          - Delete product");
logger.LogInformation("  Customers API: /api/customers");
logger.LogInformation("    GET    /api/customers              - Get all customers");
logger.LogInformation("    GET    /api/customers/{{id}}         - Get customer by ID");
logger.LogInformation("    GET    /api/customers/email/{{email}} - Get customer by email");
logger.LogInformation("    GET    /api/customers/country/{{name}} - Get customers by country");
logger.LogInformation("    GET    /api/customers/search?searchTerm={{term}} - Search customers");
logger.LogInformation("    GET    /api/customers/validate-email?email={{email}} - Validate email availability");
logger.LogInformation("    POST   /api/customers              - Create new customer");
logger.LogInformation("    PUT    /api/customers/{{id}}         - Update customer");
logger.LogInformation("    DELETE /api/customers/{{id}}         - Delete customer");
logger.LogInformation("Traditional MVC pages are also available at the root URL");
logger.LogInformation("==========================================");

#endregion

app.Run();
