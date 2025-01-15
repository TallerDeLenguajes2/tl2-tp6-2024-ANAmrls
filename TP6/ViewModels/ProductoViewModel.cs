using System.ComponentModel.DataAnnotations;
using TP6.Models;

namespace TP6.ViewModels
{
    public class ProductoViewModel
    {
        [StringLength(250)]
        public string? Descripcion { get; set; }
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "El valor debe mayor que cero")]
        public decimal Precio { get; set; }

        public ProductoViewModel(Producto producto)
        {
            Descripcion = producto.Description;
            Precio = producto.Price;

        }

        public ProductoViewModel()
        {

        }
        
    }
}
