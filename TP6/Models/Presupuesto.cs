using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace TP6.Models
{
    public class Presupuesto
    {
        private const float Iva = 1.21f;
        private int idPresupuesto;
        private string nombreDestinatario;
        List<PresupuestoDetalle> detalle;
        private DateTime fechaCreacion;

        public Presupuesto(string nombreDestinatario, DateTime fechaCreacion)
        {
            this.nombreDestinatario = nombreDestinatario;
            this.FechaCreacion = fechaCreacion;
            this.detalle = new();
        }

        public Presupuesto()
        {
            this.detalle = new();
        }

        public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto = value; }
        public string NombreDestinatario { get => nombreDestinatario; set => nombreDestinatario = value; }
        public List<PresupuestoDetalle> Detalle { get => detalle; set => detalle = value; }
        public DateTime FechaCreacion { get => fechaCreacion; set => fechaCreacion = value; }
        public float MontoPresupuesto()
        {
            float monto = 0f;

            if (detalle == null)
            {
                return monto;
            }

            foreach (var det in detalle)
            {
                monto += det.Producto.Price * det.Cantidad;
            }

            return monto;
        }

        public float MontoPresupuestoConIva()
        {
            return MontoPresupuesto() * Iva;
        }

        public int CantidadProductos()
        {
            return detalle.Count;
        }
    }
}
