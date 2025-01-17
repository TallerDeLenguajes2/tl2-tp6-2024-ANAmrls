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
        private readonly ILogger<PresupuestoController> _logger;

        public PresupuestoController(IPresupuestosRepository presupuestoRepository,
                                     IProductoRepository productoRepository,
                                     IClienteRepository clienteRepository,
                                     ILogger<PresupuestoController> logger)
        {
            _presupuestoRepository = presupuestoRepository;
            _productoRepository = productoRepository;
            _clienteRepository = clienteRepository;
            _logger = logger;
        }

        [AccessLevelAuthorize("Administrador", "Cliente")]
        public ActionResult Index()
        {
            try
            {
                var presupuestos = _presupuestoRepository.GetPresupuestos();
                return View(presupuestos);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.ToString());
                return BadRequest();
            }
        }

        [AccessLevelAuthorize("Administrador")]
        [HttpGet]
        public ActionResult CreatePresupuesto()
        {
            try
            {
                Presupuesto presupuesto = new();
                CreatePresupuestoViewModel presupuestoVM = new(presupuesto, _clienteRepository.GetClientes());
                return View(presupuestoVM);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.ToString());
                return BadRequest();
            }
        }

        [AccessLevelAuthorize("Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePresupuesto(CreatePresupuestoViewModel presupuestoVM)
        {
            try
            {
                Presupuesto presupuesto = new(presupuestoVM.FechaCreacion, _clienteRepository.GetClienteById(presupuestoVM.IdCliente));
                _presupuestoRepository.CreatePresupuesto(presupuesto);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.ToString());
                return BadRequest();
            }
        }

        [AccessLevelAuthorize("Administrador")]
        [HttpGet]
        public ActionResult EditPresupuesto(int idPresupuesto)
        {
            try
            {
                Presupuesto presupuesto = _presupuestoRepository.GetDetallePresupuestoById(idPresupuesto);
                EditPresupuestoViewModel presupuestoVM = new(presupuesto, _clienteRepository.GetClientes());
                return View(presupuestoVM);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.ToString());
                return BadRequest();
            }
        }

        [AccessLevelAuthorize("Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPresupuesto(EditPresupuestoViewModel presupuestoVM)
        {
            try
            {
                Presupuesto presupuesto = new(presupuestoVM.FechaCreacion, _clienteRepository.GetClienteById(presupuestoVM.IdCliente))
                {
                    IdPresupuesto = presupuestoVM.IdPresupuesto
                };
                _presupuestoRepository.UpdatePresupuesto(presupuesto);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.ToString());
                return BadRequest();
            }
        }

        [AccessLevelAuthorize("Administrador")]
        [HttpGet]
        public ActionResult AddProducto(int idPresupuesto)
        {
            try
            {
                AddProductoViewModel productoVM = new(_presupuestoRepository.GetPresupuestoById(idPresupuesto), _productoRepository.GetProductos());
                return View(productoVM);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.ToString());
                return BadRequest();
            }
        }

        [AccessLevelAuthorize("Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProducto(AddProductoViewModel addProductoVM)
        {
            try
            {
                var producto = _productoRepository.GetProductoById(addProductoVM.IdProducto);
                PresupuestoDetalle detalle = new(producto, addProductoVM.Cantidad);
                _presupuestoRepository.AddProducto(addProductoVM.IdPresupuesto, detalle);

                return RedirectToAction("AddProducto", _presupuestoRepository.GetPresupuestoById(addProductoVM.IdPresupuesto));
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.ToString());
                return BadRequest();
            }
        }

        [AccessLevelAuthorize("Administrador", "Cliente")]
        [HttpGet]
        public ActionResult GetDetalle(int idPresupuesto)
        {
            try
            {
                var presupuesto = _presupuestoRepository.GetDetallePresupuestoById(idPresupuesto);

                if (presupuesto.IdPresupuesto == 0)
                {
                    return View(_presupuestoRepository.GetPresupuestoByIdConCliente(idPresupuesto));
                }
                return View(presupuesto);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.ToString());
                return BadRequest();
            }
        }

        [AccessLevelAuthorize("Administrador")]
        [HttpGet]
        public ActionResult DeletePresupuesto(int idPresupuesto)
        {
            try
            {
                return View(_presupuestoRepository.GetPresupuestoById(idPresupuesto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest();
            }
        }

        [AccessLevelAuthorize("Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int idPresupuesto)
        {
            try
            {
                _presupuestoRepository.DeletePresupuestoById(idPresupuesto);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.ToString());
                return BadRequest();
            }
        }
    }
}
