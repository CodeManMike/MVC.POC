using Microsoft.AspNetCore.Mvc;

namespace MVC.POC.Controllers
{
    /// <summary>
    /// MVC controller for Customers web interface
    /// </summary>
    /// <remarks>
    /// This controller serves HTML views and interacts with the Customers API
    /// </remarks>
    public class CustomersWebController : Controller
    {
        #region Private Fields

        private readonly ILogger<CustomersWebController> _logger;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the CustomersWebController
        /// </summary>
        /// <param name="logger">The logger instance</param>
        public CustomersWebController(ILogger<CustomersWebController> logger)
        {
            _logger = logger;
        }

        #endregion

        #region Views

        /// <summary>
        /// Displays the customers index page
        /// </summary>
        /// <returns>The customers index view</returns>
        public IActionResult Index()
        {
            _logger.LogInformation("Displaying customers index page");
            return View();
        }

        /// <summary>
        /// Displays the create customer page
        /// </summary>
        /// <returns>The create customer view</returns>
        public IActionResult Create()
        {
            _logger.LogInformation("Displaying create customer page");
            return View();
        }

        /// <summary>
        /// Displays the edit customer page
        /// </summary>
        /// <param name="id">The customer ID to edit</param>
        /// <returns>The edit customer view</returns>
        public IActionResult Edit(int id)
        {
            _logger.LogInformation("Displaying edit customer page for ID: {CustomerId}", id);
            ViewBag.CustomerId = id;
            return View();
        }

        /// <summary>
        /// Displays the customer details page
        /// </summary>
        /// <param name="id">The customer ID to view</param>
        /// <returns>The customer details view</returns>
        public IActionResult Details(int id)
        {
            _logger.LogInformation("Displaying customer details page for ID: {CustomerId}", id);
            ViewBag.CustomerId = id;
            return View();
        }

        #endregion
    }
}
