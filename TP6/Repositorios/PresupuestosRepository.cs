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

                var consulta = "INSERT INTO Presupuestos (idCliente, FechaCreacion)"
                               + " VALUES (@idCliente, @FechaCreacion);";

                SqliteCommand command = new(consulta, connection);
                command.Parameters.Add(new SqliteParameter("@idCliente", presupuesto.Cliente.IdCliente));
                command.Parameters.Add(new SqliteParameter("@FechaCreacion", DateOnly.FromDateTime(presupuesto.FechaCreacion)));
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
                    var presupuesto = new Presupuesto
                    {
                        IdPresupuesto = Convert.ToInt32(reader["idPresupuesto"]),
                        FechaCreacion = DateTime.Parse((string)reader["FechaCreacion"]),
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
                string consulta;

                int cantidad = GetCantidadProducto(idPresupuesto, detalle.Producto.IdProduct);

                if (cantidad > 0)
                {
                    consulta = "UPDATE PresupuestosDetalle "
                               + "SET Cantidad = (@Cantidad) "
                               + "WHERE idPresupuesto = (@idPresupuesto) AND idProducto = (@idProducto);";
                }
                else
                {
                    consulta = "INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad) "
                               + "VALUES (@idPresupuesto, @idProducto, @Cantidad);";
                }

                cantidad += detalle.Cantidad;

                connection.Open();
                SqliteCommand command = new(consulta, connection);
                command.Parameters.Add(new SqliteParameter("@idPresupuesto", idPresupuesto));
                command.Parameters.Add(new SqliteParameter("@idProducto", detalle.Producto.IdProduct));
                command.Parameters.Add(new SqliteParameter("@Cantidad", cantidad));
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
                    presupuesto.IdPresupuesto = Convert.ToInt32(reader["idPresupuesto"]);
                    presupuesto.FechaCreacion = DateTime.Parse((string)reader["FechaCreacion"]);
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

                var consulta = "SELECT P.idPresupuesto, P.FechaCreacion, C.idCliente, C.Nombre, C.Email, C.Telefono, PR.idProducto, PR.Descripcion "
                               + "AS Producto, PR.Precio, PD.Cantidad, (PR.Precio * PD.Cantidad) AS Subtotal "
                               + "FROM Presupuestos P JOIN PresupuestosDetalle PD ON P.idPresupuesto = PD.idPresupuesto "
                               + "JOIN Productos PR ON PD.idProducto = PR.idProducto "
                               + "JOIN Cliente C ON P.idPresupuesto = C.idCliente "
                               + "WHERE P.idPresupuesto = (@idPresupuesto);";

                SqliteCommand command = new SqliteCommand(consulta, connection);
                command.Parameters.Add(new SqliteParameter("@idPresupuesto", idPresupuesto));
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    if (presupuesto.FechaCreacion == new DateTime())
                    {
                        presupuesto.IdPresupuesto = Convert.ToInt32(reader["idPresupuesto"]);
                        presupuesto.FechaCreacion = DateTime.Parse((string)reader["FechaCreacion"]);
                        presupuesto.Detalle = new List<PresupuestoDetalle>();
                        presupuesto.Cliente = new(Convert.ToInt32(reader["idCliente"]), reader["Nombre"].ToString(), reader["Email"].ToString(), reader["Telefono"].ToString());
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

        //public void UpdatePresupuesto(int idPresupuesto, Presupuesto presupuesto)
        //{
        //    using(var connection = new SqliteConnection(connectionString))
        //    {
        //        connection.Open();

        //        var consulta = "UPDATE Presupuestos "
        //                       + "SET NombreDestinatario = (@NuevoDestinatario) "
        //                       + "WHERE idPresupuesto = (@idPresupuesto);";
        //        SqliteCommand command =new SqliteCommand(consulta, connection);
        //        command.Parameters.Add(new SqliteParameter("@NuevoDestinatario", presupuesto.NombreDestinatario));
        //        command.Parameters.Add(new SqliteParameter("@idPresupuesto", idPresupuesto));

        //        command.ExecuteNonQuery();

        //        connection.Close();
        //    }
        //}

        public void QuitarProducto(int idPresupuesto, int idProducto)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var consulta = "DELETE FROM PresupuestosDetalle "
                               + "WHERE idPresupuesto = (@idPresupuesto) AND idProducto = (@idProducto);";

                SqliteCommand command = new SqliteCommand(consulta, connection);
                command.Parameters.Add(new SqliteParameter("@idPresupuesto", idPresupuesto));
                command.Parameters.Add(new SqliteParameter("@idProducto", idProducto));
                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public int GetCantidadProducto(int idPresupuesto, int idProducto)
        {
            int cantidad = 0;

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var consulta = "SELECT cantidad "
                               + "FROM PresupuestosDetalle "
                               + "WHERE idPresupuesto = (@idPresupuesto) AND idProducto = (@idProducto);";

                SqliteCommand command = new SqliteCommand(consulta, connection);
                command.Parameters.Add(new SqliteParameter("@idPresupuesto", idPresupuesto));
                command.Parameters.Add(new SqliteParameter("@idProducto", idProducto));
                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    cantidad = Convert.ToInt32(reader["Cantidad"]);
                }

                connection.Close();
            }

            return cantidad;
        }
    }
}
