using Microsoft.Data.Sqlite;
using TP6.Models;

namespace TP6.Repositorios
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly string connectionString = "Data Source=db/Tienda.db";

        public void CreateCliente(Cliente cliente)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var consulta = "INSERT INTO Cliente (Nombre, Email, Telefono) "
                               + "VALUES (@Nombre, @Email, @Telefono);";
                SqliteCommand command = new(consulta, connection);
                command.Parameters.Add(new SqliteParameter("@Nombre", cliente.Nombre));
                command.Parameters.Add(new SqliteParameter("@Email", cliente.Email));
                command.Parameters.Add(new SqliteParameter("@Telefono", cliente.Telefono));
                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void DeleteClienteById(int idCliente)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var consulta = "DELETE FROM Cliente WHERE idCliente= (@idCliente);";

                SqliteCommand command = new SqliteCommand(consulta, connection);
                command.Parameters.Add(new SqliteParameter("@idCliente", idCliente));
                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public Cliente GetClienteById(int idCliente)
        {
            Cliente cliente = new();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var consulta = "SELECT * FROM Cliente WHERE idCliente= (@idCliente);";
                SqliteCommand command = new SqliteCommand(consulta, connection);
                command.Parameters.Add(new SqliteParameter("@idCliente", idCliente));
                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    cliente.IdCliente = Convert.ToInt32(reader["idCliente"]);
                    cliente.Nombre = reader["Nombre"].ToString();
                    cliente.Email = reader["Email"].ToString();
                    cliente.Telefono = reader["Telefono"].ToString();
                }

                connection.Close();
            }

            return cliente;
        }

        public List<Cliente> GetClientes()
        {
            List<Cliente> clientes = new();

            using (var sqlitecon = new SqliteConnection(connectionString))
            {
                sqlitecon.Open();

                var consulta = @"SELECT * FROM Cliente;";
                SqliteCommand command = new SqliteCommand(consulta, sqlitecon);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var cliente = new Cliente
                    {
                        IdCliente = Convert.ToInt32(reader["idCliente"]),
                        Nombre = reader["Nombre"].ToString(),
                        Email = reader["Email"].ToString(),
                        Telefono = reader["Telefono"].ToString(),
                    };

                    clientes.Add(cliente);
                }
                sqlitecon.Close();
            }

            return clientes;
        }

        public void UpdateCliente(int idCliente, Cliente cliente)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var consulta = "UPDATE Cliente "
                               + "SET Nombre = (@NuevoNombre), Email = (@NuevoEmail), Telefono = (@NuevoTelefono) "
                               + "WHERE idCliente= (@idCliente);";
                SqliteCommand command = new(consulta, connection);
                command.Parameters.Add(new SqliteParameter("@NuevoNombre", cliente.Nombre));
                command.Parameters.Add(new SqliteParameter("@NuevoEmail", cliente.Email));
                command.Parameters.Add(new SqliteParameter("@NuevoTelefono", cliente.Telefono));
                command.Parameters.Add(new SqliteParameter("@idCliente", idCliente));

                command.ExecuteNonQuery();

                connection.Close();
            }
        }
    }
}
