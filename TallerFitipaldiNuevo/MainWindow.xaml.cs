using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TallerFitipaldiNuevo.Clases;

namespace TallerFitipaldiNuevo
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        MySqlConnector mySqlConnector;

        public MainWindow()
        {
            CrearBBDD crearBBDD = new CrearBBDD();
            InitializeComponent();
            mySqlConnector = new MySqlConnector("localhost", "TallerFitipaldiV", "root", "root");
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void bt_iniciar_sesion_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(tb_username.Text + " - " + pb_password.Password);
            mySqlConnector.Connect();
            bool loginSucces = mySqlConnector.Login(tb_username.Text, pb_password.Password);
            mySqlConnector.Disconnect();

            if (loginSucces)
            {
                MenuPrincipal menu = new MenuPrincipal();
                this.Close();
                menu.Show();
            }
            else
            {
                MessageBox.Show("El cliente y/o contraseña no son correctos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void lbl_resgister_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Registro registro = new Registro();
            registro.Show();
            this.Close();
        }

        private void tb_username_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tb_username.Text))
            {
                MessageBox.Show("El usuario es obligatorio", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    tb_username.Focus();
                }), System.Windows.Threading.DispatcherPriority.Background);
            }
        }

        private void pb_password_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tb_username.Text))
            {
                MessageBox.Show("La contraseña es obligatoria", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    tb_username.Focus();
                }), System.Windows.Threading.DispatcherPriority.Background);
            }
        }

        private void bt_iniciar_sesion_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                IniciarSesion();
            }
        }

        private void IniciarSesion()
        {
            MenuPrincipal menuPrincipal = new MenuPrincipal();
            menuPrincipal.Show();
            this.Close();
        }

    }
}
