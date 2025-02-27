using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace Proyecto2Trimestre
{
    public partial class AgregarProductoControl : UserControl
    {
        public AgregarProductoControl()
        {
            InitializeComponent();
            CargarProductos();
            CargarCategorias();
        }

        // Cargar los productos en el dataGrid
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
                    cmbCategoria.ItemsSource = categorias.DefaultView;
                    cmbCategoria.DisplayMemberPath = "CategoryName"; // Nombre que se mostrará en el ComboBox
                    cmbCategoria.SelectedValuePath = "CategoryID"; // Valor que se usará al seleccionar
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

        // Botón agregar
        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            // Obtener el nombre del producto y la categoría seleccionada
            string nombreProducto = txtProducto.Text;
            int categoriaId = (int) cmbCategoria.SelectedValue;

            if (!string.IsNullOrEmpty(nombreProducto) && categoriaId != 0)
            {
                // Llamar al método para agregar el producto
                bool exito = DatabaseHelper.AgregarProducto(nombreProducto, categoriaId);
                if (exito)
                {
                    MessageBox.Show("Producto agregado correctamente.");
                    // Recargar los productos y categorías después de agregar
                    CargarProductos();
                }
                else
                {
                    MessageBox.Show("Hubo un error al agregar el producto.");
                }
            }
            else
            {
                MessageBox.Show("Por favor, complete todos los campos.");
            }
        }
    }
}
