using ETL.Models;
using ETL.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ETL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IETLDataService _iETLDataService;

        public HomeController(ILogger<HomeController> logger, IETLDataService iETLDataService)
        {
            _logger = logger;
            _iETLDataService = iETLDataService;
        }

        public IActionResult Index()
        {
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
