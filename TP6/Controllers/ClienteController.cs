using Microsoft.AspNetCore.Mvc;
using TP6.Models;
using TP6.Repositorios;
using TP6.ViewModels;

namespace TP6.Controllers
{
    public class ClienteController : Controller
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly ILogger<ClienteController> _logger;

        public ClienteController(IClienteRepository clienteRepository, ILogger<ClienteController> logger)
        {
            _clienteRepository = clienteRepository;
            _logger = logger;
        }

        public ActionResult Index()
        {
            try
            {
                var productos = _clienteRepository.GetClientes();
                return View(productos);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.ToString());
                return BadRequest();
            }
        }

        [HttpGet]
        public ActionResult CreateCliente()
        {
            try
            {
                return View();
            }
            catch (Exception ex) { _logger.LogError(ex.ToString()); return BadRequest(); }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCliente(ClienteViewModel clienteVM)
        {
            try
            {
                if (!ModelState.IsValid) return RedirectToAction("Index");

                var cliente = new Cliente(clienteVM);
                _clienteRepository.CreateCliente(cliente);
                return RedirectToAction("Index");
            }
            catch (Exception ex) { _logger.LogError(ex.ToString()); return BadRequest(); }
        }

        [HttpGet]
        public ActionResult EditCliente(int idCliente)
        {
            try
            {
                return View(_clienteRepository.GetClienteById(idCliente));
            }
            catch (Exception ex) { _logger.LogError(ex.ToString()); return BadRequest(); }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCliente(Cliente cliente)
        {
            try
            {
                _clienteRepository.UpdateCliente(cliente.IdCliente, cliente);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.ToString());
                return BadRequest();
            }
        }

        [HttpGet]
        public ActionResult DeleteCliente(int idCliente)
        {
            try
            {
                return View(_clienteRepository.GetClienteById(idCliente));
            }
            catch (Exception ex) { _logger.LogError(ex.ToString()); return BadRequest(); }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int idCliente)
        {
            try
            {
                _clienteRepository.DeleteClienteById(idCliente);
                return RedirectToAction("Index");
            }
            catch (Exception ex) { _logger.LogError(ex.ToString()); return BadRequest(); }
        }
    }
}
