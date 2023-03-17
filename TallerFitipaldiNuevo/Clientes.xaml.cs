using System;
using System.Windows;
using System.Windows.Input;
using TallerFitipaldiNuevo.Clases;

namespace TallerFitipaldiNuevo
{

    public partial class Clientes : Window
    {

        MySqlConnector connector;

        public Clientes()
        {
            connector = new MySqlConnector("localhost", "TallerFitipaldiV", "root", "root");
            InitializeComponent();
            actualizarDataGrid();
            tb_apellidos.MaxLength = 50;
            tb_nombre.MaxLength = 50;
            tb_telefono.MaxLength = 20;
            tb_ubicacion.MaxLength = 100;
            tb_username.MaxLength = 50;
            pb_password.MaxLength = 50;
            cb_roles.Items.Add("USER");
            cb_roles.Items.Add("MECANICO");
            cb_roles.Items.Add("ADMIN");

        }

        private void MenuItemReparaciones_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItemVehiculos_Click(object sender, RoutedEventArgs e)
        {
            var selectedRow = ClientesDataGrid.SelectedItem;

            if (selectedRow != null)
            {
                Cliente clieeteSeleccionado = (Cliente)selectedRow;
                ClienteVer.clienteVerVehiculosReparaciones = connector.SeleccionarClientePorUsername(clieeteSeleccionado.Username);
                VehiculoDeCliente vehiculoDeCliente = new VehiculoDeCliente();
                vehiculoDeCliente.Show();
            }
        }

        private void MenuItemEditar_Click(object sender, RoutedEventArgs e)
        {
            var selectedRow = ClientesDataGrid.SelectedItem;

            if (selectedRow != null)
            {
                Cliente clienteSeleccionado = (Cliente)selectedRow;
                rellenarCasillas(clienteSeleccionado);
                tb_username_buscar.Text = clienteSeleccionado.Username;
                activarOpcionesEditar();
            }
        }

        private void activarOpcionesEditar()
        {
            tb_username_buscar.IsEnabled = false;
            bt_buscar_cliente_por_username.IsEnabled = false;
            bt_crear_editar_cliente.Content = "Editar";
        }

        private void desactivarOpcionesEditar()
        {
            tb_username_buscar.IsEnabled = true;
            tb_username_buscar.Text = "";
            bt_buscar_cliente_por_username.IsEnabled = true;
            bt_crear_editar_cliente.Content = "Crear";
            limpiarCasillas();
        }

        private void MenuItemEliminar_Click(object sender, RoutedEventArgs e)
        {
            var selectedRow = ClientesDataGrid.SelectedItem;

            if (selectedRow != null)
            {
                Cliente clienteSeleccionado = (Cliente)selectedRow;
                if (clienteSeleccionado.Id == Sesion.ClienteActual.Id)
                {
                    MessageBox.Show("No te puedes eliminar a ti mismo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBoxResult result = MessageBox.Show(string.Concat("¿Estás seguro de que deseas eliminar el cliente con username '", clienteSeleccionado.Username, "'? \n"), "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        connector.EliminarClientePorId(clienteSeleccionado.Id);
                        actualizarDataGrid();
                    }
                }
            }
        }

        private void bt_cancelar_operacion_Click(object sender, RoutedEventArgs e)
        {
            if (tb_apellidos.Text.Length > 0 || tb_nombre.Text.Length > 0 || tb_telefono.Text.Length > 0 || tb_ubicacion.Text.Length > 0 || tb_username.Text.Length > 0 || pb_password.Padding.Left > 0)
            {
                MessageBoxResult result = MessageBox.Show("¿Estás seguro de que deseas cancelar la operación?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    desactivarOpcionesEditar();
                }
            }

        }

        private void rellenarCasillas(Cliente cliente)
        {
            tb_username.Text = cliente.Username;
            pb_password.Password = cliente.Password;
            tb_nombre.Text = cliente.Nombre;
            tb_apellidos.Text = cliente.Apellidos;
            tb_telefono.Text = cliente.Telefono;
            tb_ubicacion.Text = cliente.Ubicacion;
            tb_id_cliente.Text = cliente.Id.ToString();
            for (int i = 0; i < cb_roles.Items.Count; i++)
            {
                if (cb_roles.Items[i].Equals(cliente.Rol))
                {
                    cb_roles.SelectedIndex = i;
                    break;
                }
            }
        }

        private void limpiarCasillas()
        {
            tb_username.Text = "";
            pb_password.Password = "";
            tb_nombre.Text = "";
            tb_apellidos.Text = "";
            tb_telefono.Text = "";
            tb_ubicacion.Text = "";
            tb_id_cliente.Text = "";
            cb_roles.SelectedIndex = -1;
        }

        private void actualizarDataGrid()
        {
            ClientesDataGrid.ItemsSource = connector.SeleccionarTodosLosClientes();
        }

        private void crear_editar_vehiculo(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                bt_crear_editar_cliente_Click(sender, e);
            }
        }

        private void buscar_cliente(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                bt_buscar_cliente_por_username_Click(sender, e);
            }
        }

        private void bt_buscar_cliente_por_username_Click(object sender, RoutedEventArgs e)
        {
            if (tb_username_buscar.Text.Length > 0)
            {
                Cliente cliente = connector.SeleccionarClientePorUsername(tb_username_buscar.Text);
                if (cliente != null)
                {
                    rellenarCasillas(cliente);
                    activarOpcionesEditar();
                }
                else
                {
                    MessageBox.Show("No se ha encontrado ningún cliente con el nombre de usuario '" + tb_username_buscar.Text + "'.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("No puede estar este campo vacío. Por favor, introduzca un nombre de usuairo.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void bt_crear_editar_cliente_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (bt_crear_editar_cliente.Content.Equals("Editar"))
                {
                    if (!Sesion.ClienteActual.Username.Equals(tb_username_buscar.Text))
                    {
                        Cliente clienteParaEditar = connector.SeleccionarClientePorUsername(tb_username_buscar.Text);
                        if (!tb_username.Text.Equals(clienteParaEditar.Username) || !pb_password.Password.Equals(clienteParaEditar.Password) || !tb_nombre.Text.Equals(clienteParaEditar.Nombre) || !tb_apellidos.Text.Equals(clienteParaEditar.Apellidos) || !tb_telefono.Text.Equals(clienteParaEditar.Telefono) || !tb_ubicacion.Text.Equals(clienteParaEditar.Ubicacion) || !cb_roles.Text.Equals(clienteParaEditar.Rol))
                        {
                            MessageBoxResult result = MessageBox.Show(string.Concat("¿Estás seguro de que deseas editar el cliente con nombre de usuario '", tb_username.Text, "'? \n"), "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
                            if (result == MessageBoxResult.Yes)
                            {
                                connector.EditarClientePorId(int.Parse(tb_id_cliente.Text), new Cliente(tb_username.Text, pb_password.Password, tb_nombre.Text, tb_apellidos.Text, tb_telefono.Text, tb_ubicacion.Text, cb_roles.Text));
                                desactivarOpcionesEditar();
                                actualizarDataGrid();
                                limpiarCasillas();
                            }
                        }
                        else
                        {
                            MessageBox.Show("No se ha aplicado ningún cambio al usuario " + clienteParaEditar.Username + ".", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("No te puedes editar a ti mismo desde esta pestaña. Ve a 'Configuración -> Mi perfil' para editar tu perfil.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    if (tb_username.Text.Length > 0 && pb_password.Password.Length > 0 && tb_nombre.Text.Length > 0 && tb_apellidos.Text.Length > 0 && tb_telefono.Text.Length > 0 && tb_ubicacion.Text.Length > 0 && cb_roles.SelectedIndex != -1)
                    {
                        connector.InsertarCliente(new Cliente(tb_username.Text, pb_password.Password, tb_nombre.Text, tb_apellidos.Text, tb_telefono.Text, tb_ubicacion.Text, cb_roles.Text));
                        actualizarDataGrid();
                        desactivarOpcionesEditar();
                    }
                    else
                    {
                        MessageBox.Show("Deben estar todos los campos rellenos.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            catch
            {
                MessageBox.Show("No se ha encontrado ningún cliente con el Id '" + tb_id_cliente.Text + "'.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void tb_username_LostFocus(object sender, RoutedEventArgs e)
        {
            bool usuarioEncontrado = connector.existeClientePorUsername(tb_username.Text);

            if (usuarioEncontrado)
            {
                if (bt_crear_editar_cliente.Content.Equals("Crear"))
                {
                    MessageBox.Show("Este nombre de usuairo ya está en uso. Escribe uno diferente.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                    tb_username.Text = "";
                    tb_username.Dispatcher.BeginInvoke(new Action(() => tb_username.Focus()));
                }
                else
                {
                    if (!tb_username.Text.Equals(tb_username_buscar.Text))
                    {
                        MessageBox.Show("Este nombre de usuairo ya está en uso. Escribe uno diferente.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                        tb_username.Text = tb_username_buscar.Text;
                        tb_username.Dispatcher.BeginInvoke(new Action(() => tb_username.Focus()));
                    }
                }
            }
        }

    }
}
