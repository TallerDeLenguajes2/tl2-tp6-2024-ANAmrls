using Microsoft.AspNetCore.Mvc.Rendering;
using TP6.Models;

namespace TP6.ViewModels
{
    public class EditPresupuestoViewModel
    {
        public DateTime FechaCreacion { get; set; }
        public List<Cliente> Clientes { get; set; }
        public SelectList Options { get; set; }
        public int IdCliente { get; set; }
        public int IdPresupuesto { get; set; }

        public EditPresupuestoViewModel(Presupuesto presupuesto, List<Cliente> clientes)
        {
            FechaCreacion = presupuesto.FechaCreacion;
            Clientes = clientes;
            Options = new SelectList(clientes, nameof(Cliente.IdCliente), nameof(Cliente.Nombre));
            IdPresupuesto = presupuesto.IdPresupuesto;
        }

        public EditPresupuestoViewModel()
        {

        }
    }
}
