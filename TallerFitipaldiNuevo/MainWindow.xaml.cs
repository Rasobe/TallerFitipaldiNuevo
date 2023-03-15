using System.Windows;
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
            tb_username.Focus();
        }

        private void bt_iniciar_sesion_Click(object sender, RoutedEventArgs e)
        {
            IniciarSesion();
        }

        private void lbl_resgister_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Registro registro = new Registro();
            registro.Show();
            this.Close();
        }

        private void presionar_enter(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                IniciarSesion();
            }
        }

        private void IniciarSesion()
        {
            if (tb_username.Text.Length > 0 && pb_password.Password.Length > 0)
            {
                bool loginSucces = mySqlConnector.Login(tb_username.Text, pb_password.Password);

                if (loginSucces)
                {
                    Sesion.ClienteActual = mySqlConnector.SeleccionarClientePorUsername(tb_username.Text);
                    MenuPrincipal menu = new MenuPrincipal();
                    this.Close();
                    menu.Show();
                }
                else
                {
                    MessageBox.Show("El cliente y/o contraseña no son correctos.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Debes rellenar los dos campos para iniciar sesión.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

    }
}
