using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Proyecto2Trimestre
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            contenidoPrincipal.Content = new InicioControl();
            this.Closing += MainWindow_Close;
        }
        private void btnInicio_Click(object sender, RoutedEventArgs e)
        {
            // Cargar el UserControl de inicio
            contenidoPrincipal.Content = new InicioControl();
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            // Cargar el UserControl de agregar
            contenidoPrincipal.Content = new AgregarProductoControl();
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            // Cargar el UserControl de modificar
            contenidoPrincipal.Content = new ModificarProductoControl();
        }

        private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {
            // Cargar el UserControl de borrar
            contenidoPrincipal.Content = new BorrarProductoControl();
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            DatabaseHelper.cerrarconexion(DatabaseHelper.GetConnection());
            MessageBox.Show("Conexion cerrada.");
            Login login = new Login();
            login.Show();
            this.Close();
        }
        private void MainWindow_Close(object sender, CancelEventArgs e)
        {
            DatabaseHelper.cerrarconexion(DatabaseHelper.GetConnection());
        }
    }
}
