using Microsoft.AspNetCore.Mvc;
using TP6.Models;
using TP6.Repositorios;

namespace TP6.Controllers
{
    public class ClienteController : Controller
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteController(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public ActionResult Index()
        {
            var productos = _clienteRepository.GetClientes();
            return View(productos);
        }

        [HttpGet]
        public ActionResult CreateCliente()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCliente(Cliente cliente)
        {
            _clienteRepository.CreateCliente(cliente);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult EditCliente(int idCliente)
        {
            return View(_clienteRepository.GetClienteById(idCliente));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCliente(Cliente cliente)
        {
            _clienteRepository.UpdateCliente(cliente.IdCliente, cliente);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult DeleteCliente(int idCliente)
        {
            return View(_clienteRepository.GetClienteById(idCliente));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int idCliente)
        {
            _clienteRepository.DeleteClienteById(idCliente);
            return RedirectToAction("Index");
        }
    }
}
