using Microsoft.AspNetCore.Mvc;

namespace MVC.POC.Controllers
{
    /// <summary>
    /// MVC controller for Products web interface
    /// </summary>
    /// <remarks>
    /// This controller serves HTML views and interacts with the Products API
    /// </remarks>
    public class ProductsWebController : Controller
    {
        #region Private Fields

        private readonly ILogger<ProductsWebController> _logger;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the ProductsWebController
        /// </summary>
        /// <param name="logger">The logger instance</param>
        public ProductsWebController(ILogger<ProductsWebController> logger)
        {
            _logger = logger;
        }

        #endregion

        #region Views

        /// <summary>
        /// Displays the products index page
        /// </summary>
        /// <returns>The products index view</returns>
        public IActionResult Index()
        {
            _logger.LogInformation("Displaying products index page");
            return View();
        }

        /// <summary>
        /// Displays the create product page
        /// </summary>
        /// <returns>The create product view</returns>
        public IActionResult Create()
        {
            _logger.LogInformation("Displaying create product page");
            return View();
        }

        /// <summary>
        /// Displays the edit product page
        /// </summary>
        /// <param name="id">The product ID to edit</param>
        /// <returns>The edit product view</returns>
        public IActionResult Edit(int id)
        {
            _logger.LogInformation("Displaying edit product page for ID: {ProductId}", id);
            ViewBag.ProductId = id;
            return View();
        }

        /// <summary>
        /// Displays the product details page
        /// </summary>
        /// <param name="id">The product ID to view</param>
        /// <returns>The product details view</returns>
        public IActionResult Details(int id)
        {
            _logger.LogInformation("Displaying product details page for ID: {ProductId}", id);
            ViewBag.ProductId = id;
            return View();
        }

        #endregion
    }
}
