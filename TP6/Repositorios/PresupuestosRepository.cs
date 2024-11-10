using Microsoft.Data.Sqlite;
using TP6.Models;

namespace TP6.Repositorios
{
    public class PresupuestosRepository : IPresupuestosRepository
    {
        private readonly string connectionString = "Data Source=db/Tienda.db";

        public void CreatePresupuesto(Presupuesto presupuesto) 
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var consulta = "INSERT INTO Presupuestos (NombreDestinatario, FechaCreacion)"
                               + " VALUES (@NombreDestinatario, @FechaCreacion);";

                SqliteCommand command = new(consulta, connection);
                command.Parameters.Add(new SqliteParameter("@NombreDestinatario", presupuesto.NombreDestinatario));
                command.Parameters.Add(new SqliteParameter("@FechaCreacion", presupuesto.FechaCreacion));
                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public List<Presupuesto> GetPresupuestos() 
        {
            List<Presupuesto> presupuestos = new();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var consulta = @"SELECT * FROM Presupuestos;";
                SqliteCommand command = new SqliteCommand(consulta, connection);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    
                    DateOnly fecha = new();
                    DateOnly.TryParseExact((string?)reader["FechaCreacion"], "yyyy-MM-dd", out fecha);
                    var presupuesto = new Presupuesto
                    {
                        IdPresupuesto = Convert.ToInt32(reader["idPresupuesto"]),
                        NombreDestinatario = reader["NombreDestinatario"].ToString(),
                        FechaCreacion = fecha
                    };

                    presupuestos.Add(presupuesto);
                }
                connection.Close();
            }

            return presupuestos;
        }


        public void AddProducto(int idPresupuesto, PresupuestoDetalle detalle)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var consulta = "INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad) "
                               + "VALUES (@idPresupuesto, @idProducto, @Cantidad);";

                SqliteCommand command = new(consulta, connection);
                command.Parameters.Add(new SqliteParameter("@idPresupuesto", idPresupuesto));
                command.Parameters.Add(new SqliteParameter("@idProducto", detalle.Producto.IdProduct));
                command.Parameters.Add(new SqliteParameter("@Cantidad", detalle.Cantidad));
                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void DeletePresupuestoById(int idPresupuesto)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var consulta = "DELETE FROM Presupuestos WHERE idPresupuesto = (@idPresupuesto);";

                SqliteCommand command = new SqliteCommand(consulta, connection);
                command.Parameters.Add(new SqliteParameter("@idPresupuesto", idPresupuesto));
                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public Presupuesto GetPresupuestoById(int idPresupuesto)
        {
            Presupuesto presupuesto = new();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var consulta = "SELECT * FROM Presupuestos WHERE idPresupuesto = (@idPresupuesto);";

                SqliteCommand command = new SqliteCommand(consulta, connection);
                command.Parameters.Add(new SqliteParameter("@idPresupuesto", idPresupuesto));
                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    DateOnly fecha = new();
                    DateOnly.TryParseExact((string?)reader["FechaCreacion"], "yyyy-MM-dd", out fecha);
                    presupuesto.IdPresupuesto = Convert.ToInt32(reader["idPresupuesto"]);
                    presupuesto.NombreDestinatario = reader["NombreDestinatario"].ToString();
                    presupuesto.FechaCreacion = fecha;
                }

                connection.Close();
            }

            return presupuesto;
        }

        public Presupuesto GetDetallePresupuestoById(int idPresupuesto)
        {
            Presupuesto presupuesto = new();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var consulta = "SELECT P.idPresupuesto, P.NombreDestinatario, P.FechaCreacion, PR.idProducto, PR.Descripcion "
                               + "AS Producto, PR.Precio, PD.Cantidad, (PR.Precio * PD.Cantidad) AS Subtotal "
                               + "FROM Presupuestos P "
                               + "JOIN PresupuestosDetalle PD ON P.idPresupuesto = PD.idPresupuesto "
                               + "JOIN Productos PR ON PD.idProducto = PR.idProducto "
                               + "WHERE P.idPresupuesto = (@idPresupuesto);";

                SqliteCommand command = new SqliteCommand(consulta, connection);
                command.Parameters.Add(new SqliteParameter("@idPresupuesto", idPresupuesto));
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    if (presupuesto.NombreDestinatario == null || presupuesto.FechaCreacion == new DateOnly(1, 1, 1))
                    {
                        DateOnly fecha = new();
                        DateOnly.TryParseExact((string?)reader["FechaCreacion"], "yyyy-MM-dd", out fecha);
                        presupuesto.IdPresupuesto = Convert.ToInt32(reader["idPresupuesto"]);
                        presupuesto.NombreDestinatario = reader["NombreDestinatario"].ToString();
                        presupuesto.FechaCreacion = fecha;
                        presupuesto.Detalle = new List<PresupuestoDetalle>();
                    }

                    var producto = new Producto
                    {
                        IdProduct = Convert.ToInt32(reader["idProducto"]),
                        Description = reader["Producto"].ToString(),
                        Price = Convert.ToInt32(reader["Precio"])
                    };

                    var detalle = new PresupuestoDetalle
                    {
                        Producto = producto,
                        Cantidad = Convert.ToInt32(reader["Cantidad"]),
                    };

                    presupuesto.Detalle.Add(detalle);
                }

                connection.Close();
            }

            return presupuesto;
        }

        public void UpdatePresupuesto(int idPresupuesto, Presupuesto presupuesto)
        {
            using(var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var consulta = "UPDATE Presupuestos "
                               + "SET NombreDestinatario = (@NuevoDestinatario) "
                               + "WHERE idPresupuesto = (@idPresupuesto);";
                SqliteCommand command =new SqliteCommand(consulta, connection);
                command.Parameters.Add(new SqliteParameter("@NuevoDestinatario", presupuesto.NombreDestinatario));
                command.Parameters.Add(new SqliteParameter("@idPresupuesto", idPresupuesto));

                command.ExecuteNonQuery();

                connection.Close();
            }
        }
    }
}
