using Microsoft.AspNetCore.Mvc;
using TP6.Models;
using TP6.Repositorios;
using TP6.ViewModels;

namespace TP6.Controllers
{
    public class ProductoController : Controller
    {
        private readonly IProductoRepository _productoRepository;
        private readonly ILogger<ProductoController> _logger;

        public ProductoController(IProductoRepository productoRepository, ILogger<ProductoController> logger)
        {
            _productoRepository = productoRepository;
            _logger = logger;
        }

        public ActionResult Index()
        {
            try
            {
                var productos = _productoRepository.GetProductos();
                return View(productos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest();
            }
        }

        [HttpGet]
        public ActionResult CreateProducto()
        {
            try
            {
                try
                {
                    return View();
                }
                catch (Exception ex) { _logger.LogError(ex.ToString()); return BadRequest(); }
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.ToString());
                return BadRequest();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProducto(ProductoViewModel productoVM)
        {
            try
            {
                if (!ModelState.IsValid) return RedirectToAction("Index");

                Producto producto = new()
                {
                    Description = productoVM.Descripcion ?? "",
                    Price = productoVM.Precio
                };
                _productoRepository.CreateProducto(producto);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.ToString());
                return BadRequest();
            }
        }

        [HttpGet]
        public ActionResult EditProducto(int idProducto)
        {
            try
            {
                try
                {
                    return View(_productoRepository.GetProductoById(idProducto));
                }
                catch (Exception ex) { _logger.LogError(ex.ToString()); return BadRequest(); }
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.ToString());
                return BadRequest();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProducto(Producto producto)
        {
            try
            {
                _productoRepository.UpdateProducto(producto.IdProduct, producto);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.ToString());
                return BadRequest();
            }
        }

        [HttpGet]
        public ActionResult DeleteProducto(int idProducto)
        {
            try
            {
                return View(_productoRepository.GetProductoById(idProducto));
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.ToString()); return BadRequest(); 
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int idProducto)
        {
            try
            {
                _productoRepository.DeleteProductoById(idProducto);
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
