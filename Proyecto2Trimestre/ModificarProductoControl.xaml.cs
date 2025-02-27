using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace Proyecto2Trimestre
{
    public partial class ModificarProductoControl : UserControl
    {
        public ModificarProductoControl()
        {
            InitializeComponent();
            CargarProductos();   // Cargar productos en el dataGrid
            CargarCategorias();
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
        private void CargarCategorias()
        {
            try
            {
                DataTable categorias = DatabaseHelper.ObtenerCategorias();
                if (categorias != null)
                {
                    cmbCategorias.ItemsSource = categorias.DefaultView;
                    cmbCategorias.DisplayMemberPath = "CategoryName"; // Nombre que se mostrará en el ComboBox
                    cmbCategorias.SelectedValuePath = "CategoryID"; // Valor que se usará al seleccionar
                }
                else
                {
                    MessageBox.Show("No se pudieron cargar las categorías.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar las categorías: " + ex.Message);
            }
        }

        // Botón modificar
        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validar si los campos están vacíos
                if (string.IsNullOrWhiteSpace(txtProductoAntiguo.Text) || string.IsNullOrWhiteSpace(txtProductoNuevo.Text) || cmbCategorias.SelectedItem == null)
                {
                    MessageBox.Show("Por favor, complete todos los campos.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Obtener el nombre del producto antiguo y nuevo, y la categoría seleccionada
                string productoAntiguo = txtProductoAntiguo.Text;
                string nuevoNombre = txtProductoNuevo.Text;
                var categoriaSeleccionada = cmbCategorias.SelectedItem as DataRowView;
                string categoriaNombre = categoriaSeleccionada["CategoryName"].ToString();

                // Obtener el ProductID del producto antiguo
                int productoId = DatabaseHelper.ObtenerProductoIdPorNombre(productoAntiguo);
                if (productoId == -1)
                {
                    MessageBox.Show("Producto antiguo no encontrado.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Obtener el CategoryID de la categoría seleccionada
                int categoriaId = ObtenerCategoriaIdPorNombre(categoriaNombre);
                if (categoriaId == -1)
                {
                    MessageBox.Show("Categoría no encontrada.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Modificar el producto
                bool resultado = DatabaseHelper.ModificarProducto(productoId, nuevoNombre, categoriaId);
                if (resultado)
                {
                    MessageBox.Show("Producto modificado con éxito.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    LimpiarCampos();
                    CargarProductos();  // Recargar los productos
                }
                else
                {
                    MessageBox.Show("Error al modificar el producto. Intente nuevamente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Método para obtener el CategoryID de una categoría con su nombre
        private int ObtenerCategoriaIdPorNombre(string categoriaNombre)
        {
            using (MySqlConnection connection = DatabaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT CategoryID FROM Categories WHERE CategoryName = @CategoryName";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@CategoryName", categoriaNombre);

                    object result = command.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : -1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al buscar categoría: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return -1;
                }
            }
        }

        // Método para limpiar los campos de entrada
        private void LimpiarCampos()
        {
            // Limpiar los campos de texto y el comboBox
            txtProductoAntiguo.Clear();
            txtProductoNuevo.Clear();
            cmbCategorias.SelectedIndex = -1;
        }
    }
}
