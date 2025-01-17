using TP6.Models;

namespace TP6.Repositorios
{
    public interface IUsuarioRepository
    {
        User GetUser(string userName, string password);
    }
}
