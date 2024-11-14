using Microsoft.AspNetCore.Mvc;
using TP6.Models;
using TP6.Repositorios;

namespace TP6.Controllers
{
    public class PresupuestoController : Controller
    {
        private readonly IPresupuestosRepository _presupuestoRepository;
        private readonly IProductoRepository _productoRepository;

        public PresupuestoController(IPresupuestosRepository presupuestoRepository, IProductoRepository productoRepository)
        {
            _presupuestoRepository = presupuestoRepository;
            _productoRepository = productoRepository;
        }

        public ActionResult Index()
        {
            var presupuestos = _presupuestoRepository.GetPresupuestos();
            return View(presupuestos);
        }

        [HttpGet]
        public ActionResult CreatePresupuesto()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePresupuesto(Presupuesto presupuesto)
        {
            _presupuestoRepository.CreatePresupuesto(presupuesto);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult EditPresupuesto(int idPresupuesto)
        {
            return View(_presupuestoRepository.GetPresupuestoById(idPresupuesto));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPresupuesto(Presupuesto presupuesto)
        {
            _presupuestoRepository.UpdatePresupuesto(presupuesto.IdPresupuesto, presupuesto);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult AddProducto(int idPresupuesto)
        {
            return View(_presupuestoRepository.GetPresupuestoById(idPresupuesto));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProducto(int idPresupuesto, [FromForm] int cantidad, [FromForm] int idProducto)
        {
            var producto = _productoRepository.GetProductoById(idProducto);
            PresupuestoDetalle detalle = new(producto, cantidad);            
            _presupuestoRepository.AddProducto(idPresupuesto, detalle);

            return RedirectToAction("AddProducto", _presupuestoRepository.GetPresupuestoById(idPresupuesto));
        }

        [HttpGet]
        public ActionResult GetDetalle(int idPresupuesto)
        {
            var presupuesto = _presupuestoRepository.GetDetallePresupuestoById(idPresupuesto);

            if (presupuesto.IdPresupuesto == 0 || presupuesto.NombreDestinatario == null)
            {
                return View(_presupuestoRepository.GetPresupuestoById(idPresupuesto));
            }
            return View(presupuesto);
        }

        [HttpGet]
        public ActionResult DeletePresupuesto(int idPresupuesto)
        {
            return View(_presupuestoRepository.GetPresupuestoById(idPresupuesto));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int idPresupuesto)
        {
            _presupuestoRepository.DeletePresupuestoById(idPresupuesto);
            return RedirectToAction("Index");
        }
    }
}
