using System.Windows;
using System.Windows.Input;
using TallerFitipaldiNuevo.Clases;

namespace TallerFitipaldiNuevo
{
    /// <summary>
    /// Lógica de interacción para MiPerfil.xaml
    /// </summary>
    public partial class MiPerfil : Window
    {
        Cliente cliente;
        MySqlConnector connector;
        public MiPerfil()
        {
            InitializeComponent();

            cliente = Sesion.ClienteActual;

            tb_apellidos.Text = cliente.Apellidos;
            tb_nombre.Text = cliente.Nombre;
            tb_telefono.Text = cliente.Telefono;
            tb_ubicacion.Text = cliente.Ubicacion;
            tb_username.Text = cliente.Username;
            pb_password.Password = cliente.Password;

            connector = new MySqlConnector("localhost", "TallerFitipaldiV", "root", "root");

        }

        private void bt_guardar_Click(object sender, RoutedEventArgs e)
        {
            bool cambios = false;
            if (!(tb_apellidos.Text == cliente.Apellidos) || !(tb_nombre.Text == cliente.Nombre) || !(tb_telefono.Text == cliente.Telefono) || !(tb_ubicacion.Text == cliente.Ubicacion) || !(tb_username.Text == cliente.Username) || !(pb_password.Password == cliente.Password))
            {
                cambios = true;
            }

            if (cambios)
            {
                MessageBoxResult result = MessageBox.Show("¿Estás seguro de que deseas guardar los cambios?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    connector.EditarClientePorId(cliente.Id, new Cliente(tb_username.Text, pb_password.Password, tb_nombre.Text, tb_apellidos.Text, tb_telefono.Text, tb_ubicacion.Text, cliente.Rol));
                    MessageBox.Show("Cambios realizados con éxito.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    Sesion.ClienteActual = connector.SeleccionarClientePorId(cliente.Id);
                }
            }
            else
            {
                MessageBox.Show("No se ha realizado ningún cambio.", "Sin cambios", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
                new MenuPrincipal().Show();
                this.Close();

        }

        private void bt_atras_Click(object sender, RoutedEventArgs e)
        {
            new MenuPrincipal().Show();
            this.Close();
        }
        private void presionar_enter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                bt_guardar_Click(sender, e);
            }
        }

    }
}
