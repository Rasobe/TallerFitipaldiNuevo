using System.Windows;
using System.Windows.Input;
using TallerFitipaldiNuevo.Clases;

namespace TallerFitipaldiNuevo
{
    /// <summary>
    /// Lógica de interacción para Registro.xaml
    /// </summary>
    public partial class Registro : Window
    {

        MySqlConnector mySqlConnector;
        public Registro()
        {
            InitializeComponent();
            mySqlConnector = new MySqlConnector("localhost", "TallerFitipaldiV", "root", "root");
        }

        private void lbl_resgister_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void bt_registrar_Click(object sender, RoutedEventArgs e)
        {
            if (tb_username.Text.Length == 0 || pb_password.Password.Length == 0 || pb_password2.Password.Length == 0 || tb_nombre.Text.Length == 0 || tb_apellidos.Text.Length == 0 || tb_telefono.Text.Length == 0 || tb_ubicacion.Text.Length == 0)
            {
                MessageBox.Show("Deben estar TODOS los campos rellenados.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                if (pb_password.Password.Equals(pb_password2.Password))
                {
                    mySqlConnector.Connect();
                    bool registerSucces = mySqlConnector.Register(new Cliente(tb_username.Text, pb_password.Password, tb_nombre.Text, tb_apellidos.Text, tb_telefono.Text, tb_ubicacion.Text));
                    mySqlConnector.Disconnect();

                    if (registerSucces)
                    {
                        MainWindow main = new MainWindow();
                        this.Close();
                        main.Show();
                        MessageBox.Show("Te has registrado de forma exitosa.", "Registro exitoso", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Las contraseñas no coinciden. Escríbelas de nuevo.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
    }
}
