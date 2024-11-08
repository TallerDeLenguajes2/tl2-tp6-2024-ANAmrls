using Microsoft.AspNetCore.Mvc;
using TP6.Models;
using TP6.Repositorios;

namespace TP6.Controllers
{
    public class ProductoController : Controller
    {
        private readonly IProductoRepository _productoRepository;

        public ProductoController(IProductoRepository productoRepository)
        {
            _productoRepository = productoRepository;
        }

        public ActionResult Index()
        {
            var productos = _productoRepository.GetProductos();
            return View(productos);
        }

        [HttpGet]
        public ActionResult CreateProducto()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProducto(Producto producto)
        {
            _productoRepository.CreateProducto(producto);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult EditProducto(int idProducto)
        {
            return View(_productoRepository.GetProductoById(idProducto));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProducto(int idProducto, Producto producto)
        {
            _productoRepository.UpdateProducto(idProducto, producto);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult DeleteProducto(int idProducto)
        {
            return View(_productoRepository.GetProductoById(idProducto));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int idProducto)
        {
            _productoRepository.DeleteProductoById(idProducto);
            return RedirectToAction("Index");
        }
    }
}
