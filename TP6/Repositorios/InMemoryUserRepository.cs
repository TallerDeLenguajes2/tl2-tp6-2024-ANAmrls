using TP6.Models;

namespace TP6.Repositorios
{
    public class InMemoryUserRepository : IInMemoryUserRepository
    {
        private readonly List<User> _users;

        public InMemoryUserRepository()
        {
           _users = [new User(1, "nombre1", "1234", AccessLevel.Administrador, "lala"), new User(2, "nombre2", "1234",
               AccessLevel.Administrador, "lala"), new User(3, "nombre3", "1234", AccessLevel.Cliente, "lala"), new User(4, "nombre4", "1234", AccessLevel.Cliente, "lala")];
        }

        public User GetUser(string username, string password)
        {
            return _users
            .Where(u => u.UserName.Equals(username, StringComparison.OrdinalIgnoreCase) && u.Password.Equals(password, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
        }
    }
}

