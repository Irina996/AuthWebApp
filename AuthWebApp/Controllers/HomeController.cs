using AuthWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AuthWebApp.Controllers
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
            if (User.Identity.IsAuthenticated) 
            {
                return RedirectToAction("Index", "Users");
            }
            return View();
        }

        public IActionResult Login()
        {
            return RedirectToAction("Login", "Users");
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