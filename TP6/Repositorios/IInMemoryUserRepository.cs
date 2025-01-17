using TP6.Models;

namespace TP6.Repositorios
{
    public interface IInMemoryUserRepository
    {
        User GetUser(string username, string password);
    }
}