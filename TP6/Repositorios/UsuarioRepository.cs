using Microsoft.Data.Sqlite;
using TP6.Models;

namespace TP6.Repositorios
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly string connectionString = "Data source=db/Tienda.db";

        public UsuarioRepository() { }

        public User GetUser(string userName, string password)
        {
            User user = new();

             using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var consulta = "SELECT Usuario, Rol FROM Usuario WHERE Usuario = (@Usuario) AND Password = (@Password);";
                SqliteCommand command = new(consulta, connection);
                command.Parameters.Add(new SqliteParameter("@Usuario", userName));
                command.Parameters.Add(new SqliteParameter("@Password", password));
                var reader = command.ExecuteReader();

                if (reader.Read())
                {                    
                    user.UserName = reader["Usuario"].ToString();

                    if (!Enum.TryParse(reader["Rol"].ToString(), out AccessLevel rol))
                    {
                        rol = AccessLevel.NoLogueado;
                    }
                    
                    user.AccessLevel = rol;

                }

                connection.Close();
            }

            return user;
        }
    }
}
