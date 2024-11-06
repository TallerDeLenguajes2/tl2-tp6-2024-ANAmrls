using Microsoft.Data.Sqlite;
using TP6.Models;

namespace TP6.Repositorios
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly string connectionString = "Data Source=db/Tienda.db";

        public ProductoRepository() { }
        public void CreateProducto(Producto producto)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var consulta = "INSERT INTO Productos (Descripcion, Precio) "
                               + "VALUES (@Descripcion, @Precio);";
                SqliteCommand command = new(consulta, connection);
                command.Parameters.Add(new SqliteParameter("@Descripcion", producto.Description));
                command.Parameters.Add(new SqliteParameter("@Precio", producto.Price));
                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void UpdateProducto(int idProducto, Producto producto)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var consulta = "UPDATE Productos "
                               + "SET Descripcion = (@NuevaDescripcion), Precio = (@NuevoPrecio) "
                               + "WHERE idProducto = (@idProducto);";
                SqliteCommand command = new(consulta, connection);
                command.Parameters.Add(new SqliteParameter("@NuevaDescripcion", producto.Description));
                command.Parameters.Add(new SqliteParameter("@NuevoPrecio", producto.Price));
                command.Parameters.Add(new SqliteParameter("@idProducto", idProducto));

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public List<Producto> GetProductos()
        {
            List<Producto> productos = new List<Producto>();

            using (var sqlitecon = new SqliteConnection(connectionString))
            {
                sqlitecon.Open();

                var consulta = @"SELECT * FROM Productos;";
                SqliteCommand command = new SqliteCommand(consulta, sqlitecon);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var producto = new Producto
                    {
                        IdProduct = Convert.ToInt32(reader["idProducto"]),
                        Description = reader["Descripcion"].ToString(),
                        Price = Convert.ToInt32(reader["Precio"])
                    };

                    productos.Add(producto);
                }
                sqlitecon.Close();
            }

            return productos;
        }
        public Producto GetProductoById(int idProducto)
        {
            Producto producto = new();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var consulta = "SELECT * FROM Productos WHERE idProducto = (@idProducto);";
                SqliteCommand command = new SqliteCommand(consulta, connection);
                command.Parameters.Add(new SqliteParameter("@idProducto", idProducto));
                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    producto.Description = reader["Descripcion"].ToString();
                    producto.Price = Convert.ToInt32(reader["Precio"]);
                }

                connection.Close();
            }

            return producto;
        }

        public void DeleteProductoById(int idProducto)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var consulta = "DELETE FROM Productos WHERE idProducto = (@idProducto);";

                SqliteCommand command = new SqliteCommand(consulta, connection);
                command.Parameters.Add(new SqliteParameter("@idProducto", idProducto));
                command.ExecuteNonQuery();

                connection.Close();
            }
        }
    }
}
