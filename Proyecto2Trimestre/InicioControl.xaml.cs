using System.Windows;
using System.Data;
using System.Windows.Controls;
using System;

namespace Proyecto2Trimestre
{
    public partial class InicioControl : UserControl
    {
        public InicioControl()
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
    }
}
