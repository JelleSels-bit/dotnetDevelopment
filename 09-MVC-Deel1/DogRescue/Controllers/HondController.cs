using System.Diagnostics;
using DogRescue.Models;
using Microsoft.AspNetCore.Mvc;

namespace DogRescue.Controllers
{
    public class HondController : Controller
    {
        private readonly ILogger<HondController> _logger;

        public HondController(ILogger<HondController> logger)
        {
            _logger = logger;
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
