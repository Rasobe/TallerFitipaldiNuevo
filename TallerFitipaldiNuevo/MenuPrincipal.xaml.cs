using System.Windows;
using TallerFitipaldiNuevo.Clases;

namespace TallerFitipaldiNuevo
{
    /// <summary>
    /// Lógica de interacción para MenuPrincipal.xaml
    /// </summary>
    public partial class MenuPrincipal : Window
    {

        Cliente clienteActual;

        public MenuPrincipal()
        {

            clienteActual = Sesion.ClienteActual;

            InitializeComponent();
            tb_bienvenida_Copy.Text = clienteActual.Username;

            if (clienteActual.Rol.Equals("USER"))
            {
                bt_vehiculos.Content = "Mis vehículos";
                bt_reparaciones.Content = "Mis reparaciones";
                bt_clientes.Visibility = Visibility.Collapsed;
            }
            else if (clienteActual.Rol.Equals("ADMIN"))
            {
                bt_vehiculos.Content = "Vehículos";
                bt_clientes.Content = "Clientes";
                bt_reparaciones.Content = "Reparaciones";
            } else
            {
                bt_clientes.Visibility= Visibility.Collapsed;
                bt_vehiculos.Content = "Vehículos";
                bt_reparaciones.Content = "Reparaciones";
            }

        }

        private void MiCuenta_Click(object sender, RoutedEventArgs e)
        {
            MiPerfil miPerfil = new MiPerfil();
            miPerfil.Show();
            this.Close();
        }

        private void CerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            Sesion.ClienteActual = null;
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void bt_vehiculos_Click(object sender, RoutedEventArgs e)
        {
            Vehiculos vehiculos = new Vehiculos();
            vehiculos.Show();
        }

        private void bt_reparaciones_Click(object sender, RoutedEventArgs e)
        {
            Reparaciones reparaciones = new Reparaciones();
            reparaciones.Show();
        }

        private void bt_clientes_Click(object sender, RoutedEventArgs e)
        {
            Clientes clientes = new Clientes();
            clientes.Show();
        }
    }
}
