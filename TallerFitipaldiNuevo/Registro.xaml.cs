using System;
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
            RegistrarUsuario();
        }

        private void RegistrarUsuario()
        {
            if (tb_username.Text.Length == 0 || pb_password.Password.Length == 0 || pb_password2.Password.Length == 0 || tb_nombre.Text.Length == 0 || tb_apellidos.Text.Length == 0 || tb_telefono.Text.Length == 0 || tb_ubicacion.Text.Length == 0)
            {
                MessageBox.Show("Deben estar TODOS los campos rellenados.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                if (pb_password.Password.Equals(pb_password2.Password))
                {
                    bool registerSucces = mySqlConnector.Register(new Cliente(tb_username.Text, pb_password.Password, tb_nombre.Text, tb_apellidos.Text, tb_telefono.Text, tb_ubicacion.Text));

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

        private void presionar_enter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                RegistrarUsuario();
            }
        }

        private void tb_username_LostFocus(object sender, RoutedEventArgs e)
        {
            bool usuarioEncontrado = mySqlConnector.existeClientePorUsername(tb_username.Text);

            if (usuarioEncontrado)
            {
                MessageBox.Show("Este nombre de usuairo ya está en uso. Escribe uno diferente.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                tb_username.Text = "";
                tb_username.Dispatcher.BeginInvoke(new Action(() => tb_username.Focus()));
            }
        }
    }
}
