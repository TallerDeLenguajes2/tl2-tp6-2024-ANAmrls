using Microsoft.AspNetCore.Mvc;
using TP6.Filters;
using TP6.Models;
using TP6.Repositorios;
using TP6.ViewModels;

namespace TP6.Controllers
{
    public class PresupuestoController : Controller
    {
        private readonly IPresupuestosRepository _presupuestoRepository;
        private readonly IProductoRepository _productoRepository;
        private readonly IClienteRepository _clienteRepository;

        public PresupuestoController(IPresupuestosRepository presupuestoRepository, IProductoRepository productoRepository, IClienteRepository clienteRepository)
        {
            _presupuestoRepository = presupuestoRepository;
            _productoRepository = productoRepository;
            _clienteRepository = clienteRepository;
        }

        [AccessLevelAuthorize("Administrador", "Cliente")]
        public ActionResult Index()
        {
            var presupuestos = _presupuestoRepository.GetPresupuestos();
            return View(presupuestos);
        }

        [AccessLevelAuthorize("Administrador")]
        [HttpGet]
        public ActionResult CreatePresupuesto()
        {
            Presupuesto presupuesto = new();
            CreatePresupuestoViewModel presupuestoVM = new(presupuesto, _clienteRepository.GetClientes());
            return View(presupuestoVM);
        }

        [AccessLevelAuthorize("Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePresupuesto(CreatePresupuestoViewModel presupuestoVM)
        {
            Presupuesto presupuesto = new(presupuestoVM.FechaCreacion, _clienteRepository.GetClienteById(presupuestoVM.IdCliente));
            _presupuestoRepository.CreatePresupuesto(presupuesto);
            return RedirectToAction("Index");
        }

        [AccessLevelAuthorize("Administrador")]
        [HttpGet]
        public ActionResult EditPresupuesto(int idPresupuesto)
        {
            Presupuesto presupuesto = _presupuestoRepository.GetDetallePresupuestoById(idPresupuesto);
            EditPresupuestoViewModel presupuestoVM = new(presupuesto, _clienteRepository.GetClientes());
            return View(presupuestoVM);
        }

        [AccessLevelAuthorize("Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPresupuesto(EditPresupuestoViewModel presupuestoVM)
        {
            Presupuesto presupuesto = new(presupuestoVM.FechaCreacion, _clienteRepository.GetClienteById(presupuestoVM.IdCliente))
            {
                IdPresupuesto = presupuestoVM.IdPresupuesto
            };
            _presupuestoRepository.UpdatePresupuesto(presupuesto);
            return RedirectToAction("Index");
        }

        [AccessLevelAuthorize("Administrador")]
        [HttpGet]
        public ActionResult AddProducto(int idPresupuesto)
        {
            AddProductoViewModel productoVM = new(_presupuestoRepository.GetPresupuestoById(idPresupuesto), _productoRepository.GetProductos());
            return View(productoVM);
        }

        [AccessLevelAuthorize("Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProducto(AddProductoViewModel addProductoVM)
        {
            var producto = _productoRepository.GetProductoById(addProductoVM.IdProducto);
            PresupuestoDetalle detalle = new(producto, addProductoVM.Cantidad);            
            _presupuestoRepository.AddProducto(addProductoVM.IdPresupuesto, detalle);

            return RedirectToAction("AddProducto", _presupuestoRepository.GetPresupuestoById(addProductoVM.IdPresupuesto));
        }

        [AccessLevelAuthorize("Administrador", "Cliente")]
        [HttpGet]
        public ActionResult GetDetalle(int idPresupuesto)
        {
            var presupuesto = _presupuestoRepository.GetDetallePresupuestoById(idPresupuesto);

            if (presupuesto.IdPresupuesto == 0)
            {
                return View(_presupuestoRepository.GetPresupuestoByIdConCliente(idPresupuesto));
            }
            return View(presupuesto);
        }

        [AccessLevelAuthorize("Administrador")]
        [HttpGet]
        public ActionResult DeletePresupuesto(int idPresupuesto)
        {
            return View(_presupuestoRepository.GetPresupuestoById(idPresupuesto));
        }

        [AccessLevelAuthorize("Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int idPresupuesto)
        {
            _presupuestoRepository.DeletePresupuestoById(idPresupuesto);
            return RedirectToAction("Index");
        }
    }
}
