using Microsoft.AspNetCore.Mvc;
using TP6.Models;
using TP6.Repositorios;
using TP6.ViewModels;

namespace TP6.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ILogger<LoginController> _logger;

        public LoginController(IUsuarioRepository usuarioRepository, ILogger<LoginController> logger)
        {
            _usuarioRepository = usuarioRepository;
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                var model = new LoginViewModel
                {
                    IsAuthenticated = HttpContext.Session.GetString("IsAuthenticated") == "true",
                };
                return View(model);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.ToString());
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            try
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

                    _logger.LogInformation("El usuario " + usuario.UserName + "ingresó correctamente.");

                    return RedirectToAction("Index", "Home");
                }

                model.ErrorMessage = "Credenciales inválidas";
                model.IsAuthenticated = false;

                _logger.LogWarning("Intento de acceso inválido - Usuario: " + model.Username + " - Clave ingresada: " + model.Password);

                return View("Index", model);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.ToString());
                return BadRequest();
            }
        }

        public IActionResult Logout()
        {
            try
            {
                // Limpiar la sesión
                HttpContext.Session.Clear();

                // Redirigir a la vista de login
                return RedirectToAction("Index");
            }
            catch (Exception ex) { _logger.LogError(ex.ToString()); return BadRequest(); }
        }
    }
}
