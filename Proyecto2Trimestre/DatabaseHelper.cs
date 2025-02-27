using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows;

namespace Proyecto2Trimestre
{
    public static class DatabaseHelper
    {
        private static string conexionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(conexionString);
        }

        public static void abrirconexion(MySqlConnection conexion)
        {
            if (conexion.State == ConnectionState.Closed)
            {
                conexion.Open();
            }
        }

        public static void cerrarconexion(MySqlConnection conexion)
        {
            if (conexion.State == ConnectionState.Open)
            {
                conexion.Close();
            }
        }

        public static DataTable ObtenerCategorias()
        {
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    abrirconexion(conn);
                    string query = "SELECT CategoryID, CategoryName FROM Categories";
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn))
                    {
                        DataTable categorias = new DataTable();
                        adapter.Fill(categorias);
                        return categorias;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener categorías: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public static DataTable ObtenerProductosConCategorias()
        {
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    abrirconexion(conn);
                    string query = @"
                        SELECT p.ProductID, p.ProductName, c.CategoryName 
                        FROM Products p
                        JOIN Categories c ON p.CategoryID = c.CategoryID";
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn))
                    {
                        DataTable productosConCategorias = new DataTable();
                        adapter.Fill(productosConCategorias);
                        return productosConCategorias;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener productos con categorías: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public static bool AgregarProducto(string nombreProducto, int categoriaId)
        {
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    abrirconexion(conn);
                    string query = "INSERT INTO Products (ProductName, CategoryID) VALUES (@nombre, @categoriaId)";
                    using (MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@nombre", nombreProducto);
                        command.Parameters.AddWithValue("@categoriaId", categoriaId);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar producto: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public static bool ModificarProducto(int productoId, string nuevoNombre, int nuevaCategoriaId)
        {
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    abrirconexion(conn);
                    string query = "UPDATE Products SET ProductName = @nuevoNombre, CategoryID = @nuevaCategoriaId WHERE ProductID = @productoId";
                    using (MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@nuevoNombre", nuevoNombre);
                        command.Parameters.AddWithValue("@nuevaCategoriaId", nuevaCategoriaId);
                        command.Parameters.AddWithValue("@productoId", productoId);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar producto: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public static bool BorrarProducto(int productoId)
        {
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    abrirconexion(conn);
                    string query = "DELETE FROM Products WHERE ProductID = @productoId";
                    using (MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@productoId", productoId);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al borrar producto: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public static int ObtenerProductoIdPorNombre(string nombreProducto)
        {
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    abrirconexion(conn);
                    string query = "SELECT ProductID FROM Products WHERE ProductName = @ProductName";
                    using (MySqlCommand comando = new MySqlCommand(query, conn))
                    {
                        comando.Parameters.AddWithValue("@ProductName", nombreProducto);
                        object resultado = comando.ExecuteScalar();
                        return resultado != null ? Convert.ToInt32(resultado) : -1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar producto: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1;
            }
        }
    }
}
