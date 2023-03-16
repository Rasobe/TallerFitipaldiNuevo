using System.Windows;
using TallerFitipaldiNuevo.Clases;

namespace TallerFitipaldiNuevo
{
    /// <summary>
    /// Lógica de interacción para MenuPrincipal.xaml
    /// </summary>
    public partial class MenuPrincipal : Window
    {

        Cliente clienteActual = Sesion.ClienteActual;

        public MenuPrincipal()
        {
            InitializeComponent();
            tb_bienvenida_Copy.Text = clienteActual.Username;

            if (clienteActual.Rol.Equals("USER"))
            {
                bt_vehiculos.Content = "Mis vehículos";
                bt_reparaciones.Visibility = Visibility.Collapsed;
                bt_clientes.Visibility = Visibility.Collapsed;
            }
            else if (clienteActual.Rol.Equals("ADMIN"))
            {
                bt_vehiculos.Content = "Vehículos";
                bt_clientes.Content = "Clientes";
                bt_reparaciones.Content = "Reparaciones";
            }

        }

        private void bt_vehiculos_Click(object sender, RoutedEventArgs e)
        {
            Vehiculos vehiculos = new Vehiculos();
            vehiculos.Show();
        }

        private void MiCuenta_Click(object sender, RoutedEventArgs e)
        {
            // Acción para la opción "Mis vehículos"
        }

        private void CerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            Sesion.ClienteActual = null;
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }


    }
}
