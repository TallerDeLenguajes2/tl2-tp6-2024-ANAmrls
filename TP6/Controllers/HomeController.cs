using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TP6.Models;

namespace TP6.Controllers
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
            try
            {
                var userName = Request.Cookies["AuthCookie"];
                if (userName == null)
                    return RedirectToAction("Index", "Login");
                return View();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.ToString());
                return BadRequest();
            }
        }

        public IActionResult Privacy()
        {
            try
            {
                return View();
            }
            catch (Exception ex) { _logger.LogError(ex.ToString()); return BadRequest(); }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
