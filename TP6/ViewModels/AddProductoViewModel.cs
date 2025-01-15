using Microsoft.AspNetCore.Mvc.Rendering;
using TP6.Models;

namespace TP6.ViewModels
{
    public class AddProductoViewModel
    {
        public int IdPresupuesto { get; set; }
        public int Cantidad { get; set; }
        public List<Producto> Productos { get; set; }
        public SelectList Options { get; set; }
        public int IdProducto { get; set; }

        public AddProductoViewModel(Presupuesto presupuesto, List<Producto> productos, int cantidad)
        {
            IdPresupuesto = presupuesto.IdPresupuesto;
            Productos = productos;
            Cantidad = cantidad;
        }

        public AddProductoViewModel(Presupuesto presupuesto, List<Producto> productos)
        {
            IdPresupuesto = presupuesto.IdPresupuesto;
            Productos = productos;
            Options = new SelectList(items: productos, nameof(Producto.IdProduct), nameof(Producto.Description));
        }

        public AddProductoViewModel()
        {

        }
    }
}
