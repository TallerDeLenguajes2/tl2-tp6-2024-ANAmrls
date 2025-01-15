using System.ComponentModel.DataAnnotations;
using TP6.Models;

namespace TP6.ViewModels
{
    public class ClienteViewModel
    {
        [Required]
        public string Nombre { get; set; }
        [Required]
        [EmailAddress (ErrorMessage = "Ingrese un email válido")]
        public string Email { get; set; }
        [Phone]
        public string Telefono { get; set; }

        public ClienteViewModel(Cliente cliente)
        {
            Nombre = cliente.Nombre;
            Email = cliente.Email;
            Telefono = cliente.Telefono;
        }

        public ClienteViewModel() { }
    }
}
