using TP6.Models;

namespace TP6.Repositorios
{
    public interface IProductoRepository
    {
        void CreateProducto(Producto producto);
        void DeleteProductoById(int id);
        Producto GetProductoById(int id);
        List<Producto> GetProductos();
        void UpdateProducto(int idProducto, Producto producto);
    }
}