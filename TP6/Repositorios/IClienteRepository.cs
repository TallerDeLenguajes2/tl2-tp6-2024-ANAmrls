using TP6.Models;

namespace TP6.Repositorios
{
    public interface IClienteRepository
    {
        List<Cliente> GetClientes();
        Cliente GetClienteById(int idCliente);
        void CreateCliente(Cliente cliente);
        void DeleteClienteById(int idCliente);
        void UpdateCliente(int idCliente, Cliente cliente);
    }
}
