using Microsoft.AspNetCore.Mvc;

namespace TP6.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Error403()
        {
            return View();
        }
    }
}
