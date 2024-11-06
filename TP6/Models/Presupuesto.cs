namespace TP6.Models
{
    public class Presupuesto
    {
        private const float Iva = 1.21f;
        private int idPresupuesto;
        private string nombreDestinatario;
        List<PresupuestoDetalle> detalle;
        private DateOnly fechaCreacion = new(1, 1, 1);

        public Presupuesto(string nombreDestinatario, DateOnly fechaCreacion)
        {
            this.nombreDestinatario = nombreDestinatario;
            this.FechaCreacion = fechaCreacion;
            this.detalle = new();
        }

        public Presupuesto()
        {
        }

        public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto = value; }
        public string NombreDestinatario { get => nombreDestinatario; set => nombreDestinatario = value; }
        public List<PresupuestoDetalle> Detalle { get => detalle; set => detalle = value; }
        public DateOnly FechaCreacion { get => fechaCreacion; set => fechaCreacion = value; }
        public float MontoPresupuesto()
        {
            float monto = 0f;

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
