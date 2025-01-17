using Microsoft.AspNetCore.Mvc;
using TP6.Models;
using TP6.Repositorios;
using TP6.ViewModels;

namespace TP6.Controllers
{
    public class LoginController : Controller
    {
        //private readonly IInMemoryUserRepository _userRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public LoginController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public IActionResult Index()
        {
            var model = new LoginViewModel
            {
                IsAuthenticated = HttpContext.Session.GetString("IsAuthenticated") == "true",
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                model.ErrorMessage = "Por favor ingrese su nombre de usuario y contraseña.";
                return View("Index", model);
            }

            User usuario = _usuarioRepository.GetUser(model.Username, model.Password);

            if (usuario.UserName != null)
            {
                HttpContext.Session.SetString("IsAuthenticated", "true");
                HttpContext.Session.SetString("User", usuario.UserName);
                HttpContext.Session.SetString("AccessLevel", usuario.AccessLevel.ToString());

                return RedirectToAction("Index", "Home");
            }

            model.ErrorMessage = "Credenciales inválidas";
            model.IsAuthenticated = false;
            return View("Index", model);
        }

        public IActionResult Logout()
        {
            // Limpiar la sesión
            HttpContext.Session.Clear();

            // Redirigir a la vista de login
            return RedirectToAction("Index");
        }
    }
}
