using EmpRefPortal.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EmpRefPortal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            string username = HttpContext.Request.Cookies["Username"];
            string userRole = HttpContext.Request.Cookies["UserRole"];

            // Pass the cookie data to the view
            ViewBag.Username = username;
            ViewBag.UserRole = userRole;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}