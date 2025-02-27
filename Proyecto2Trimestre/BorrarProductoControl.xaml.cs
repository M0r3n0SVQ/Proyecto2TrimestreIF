using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace Proyecto2Trimestre
{
    public partial class BorrarProductoControl : UserControl
    {
        public BorrarProductoControl()
        {
            InitializeComponent();
            CargarProductos();
        }

        private void CargarProductos()
        {
            try
            {
                // Obtener los productos desde la base de datos
                DataTable productosConCategorias = DatabaseHelper.ObtenerProductosConCategorias();

                if (productosConCategorias != null)
                {
                    dgProductos.ItemsSource = productosConCategorias.DefaultView;
                }
                else
                {
                    MessageBox.Show("No se pudieron cargar los productos.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los productos: " + ex.Message);
            }
        }

        private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {
            string nombreProducto = txtProductoBorrar.Text.Trim();

            if (string.IsNullOrWhiteSpace(nombreProducto))
            {
                MessageBox.Show("Por favor, ingrese el nombre del producto a borrar.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Obtener el ID del producto basándonos en el nombre
            int productoId = DatabaseHelper.ObtenerProductoIdPorNombre(nombreProducto);

            if (productoId == -1)
            {
                MessageBox.Show("No se encontró un producto con ese nombre.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Confirmación antes de borrar
            MessageBoxResult resultado = MessageBox.Show($"¿Seguro que desea borrar el producto '{nombreProducto}'?",
                                                          "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (resultado == MessageBoxResult.Yes)
            {
                bool exito = DatabaseHelper.BorrarProducto(productoId);
                if (exito)
                {
                    MessageBox.Show("Producto eliminado con éxito.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    CargarProductos();  // Recargar la lista
                    txtProductoBorrar.Clear();  // Limpiar el textBox
                }
                else
                {
                    MessageBox.Show("Error al eliminar el producto. Intente nuevamente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        public static int ObtenerProductoIdPorNombre(string nombreProducto)
        {
            using (MySqlConnection conexion = DatabaseHelper.GetConnection())
            {
                if (conexion == null)
                {
                    throw new Exception("La conexión a la base de datos no se pudo establecer.");
                }

                try
                {
                    string query = "SELECT ProductID FROM products WHERE ProductName = @ProductName";
                    MySqlCommand comando = new MySqlCommand(query, conexion);
                    comando.Parameters.AddWithValue("@ProductName", nombreProducto);

                    object resultado = comando.ExecuteScalar();
                    return resultado != null ? Convert.ToInt32(resultado) : -1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al buscar producto: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return -1;
                }
            }
        }
    }
}
