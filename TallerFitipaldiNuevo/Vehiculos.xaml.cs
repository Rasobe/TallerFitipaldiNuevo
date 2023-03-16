using System.Windows;
using System.Windows.Input;
using TallerFitipaldiNuevo.Clases;

namespace TallerFitipaldiNuevo
{

    public partial class Vehiculos : Window
    {

        MySqlConnector connector;

        public Vehiculos()
        {
            connector = new MySqlConnector("localhost", "TallerFitipaldiV", "root", "root");
            InitializeComponent();
            actualizarDataGrid();
            tb_matricula_vehiculo.MaxLength = 10;

            if (!Sesion.ClienteActual.Rol.Equals("ADMIN"))
            {
                rect_clientes_id.Visibility = Visibility.Collapsed;
                lbl_busca_id_cliente.Visibility = Visibility.Collapsed;
                lbl_username.Visibility = Visibility.Collapsed;
                tb_buscar_username.Visibility = Visibility.Collapsed;
                linea_cliente_id.Visibility = Visibility.Collapsed;
                lbl_id_buscar.Visibility = Visibility.Collapsed;
                tb_id_cliente_buscado.Visibility = Visibility.Collapsed;
                lbl_id_cliente_vehiculo.Visibility = Visibility.Collapsed;
                tb_id_cliente_vehiculo.Visibility = Visibility.Collapsed;
                bt_buscar_id_usuario_por_username.Visibility = Visibility.Collapsed;
            }

        }

        private void bt_buscar_vehiculo_matricula_Click(object sender, RoutedEventArgs e)
        {
            if (!(tb_matricula_buscar.Text.Length == 0))
            {
                Vehiculo vehiculo = connector.SeleccionarVehiculoPorMatricula(tb_matricula_buscar.Text);
                if (vehiculo != null)
                {
                    if (!Sesion.ClienteActual.Rol.Equals("ADMIN"))
                    {
                        if (vehiculo.ClienteId.Equals(Sesion.ClienteActual.Id))
                        {
                            rellenarCasillas(vehiculo);
                            tb_matricula_buscar.Text = "";
                            activarOpcionesEdicion();
                        }
                        else
                        {
                            MessageBox.Show("No puedes buscar vehículos que no sean de tu propiedad.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            tb_matricula_buscar.Text = "";
                        }
                    }
                    else
                    {
                        rellenarCasillas(vehiculo);
                        tb_matricula_buscar.Text = "";
                        activarOpcionesEdicion();
                    }
                }
                else
                {
                    MessageBox.Show("No se ha encontrado ningún vehículo con la matrícula '" + tb_matricula_buscar.Text + "'.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            else
            {
                MessageBox.Show("Debe estar el campo relleno para iniciar la búsqueda.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void presionar_enter(object sender, KeyEventArgs e)
        {

        }

        private void MenuItemEditar_Click(object sender, RoutedEventArgs e)
        {
            var selectedRow = VehiculosDataGrid.SelectedItem;

            if (selectedRow != null)
            {
                Vehiculo vehiculoSeleccionado = (Vehiculo)selectedRow;
                rellenarCasillas(vehiculoSeleccionado);
                tb_matricula_buscar.Text = vehiculoSeleccionado.Matricula;
                activarOpcionesEdicion();
            }
        }

        private void bt_crear_editar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (bt_crear_editar_vehiculo.Content.Equals("Editar"))
                {
                    MessageBoxResult result = MessageBox.Show(string.Concat("¿Estás seguro de que deseas editar el vehículo con matrícula '", tb_matricula_vehiculo.Text, "'? \n"), "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        connector.EditarVehiculoPorMatricula(tb_matricula_buscar.Text, new Vehiculo(tb_matricula_vehiculo.Text, tb_modelo_vehiculo.Text, tb_marca_vehiculo.Text, int.Parse(tb_id_cliente_vehiculo.Text)));
                        desactivarOpcionesEdicion();
                        actualizarDataGrid();
                        limpiarCasillas();
                    }
                }
                else
                {
                    Vehiculo vehiculo;
                    if (tb_id_cliente_vehiculo.Text.Length == 0)
                    {
                        vehiculo = new Vehiculo(tb_matricula_vehiculo.Text, tb_modelo_vehiculo.Text, tb_marca_vehiculo.Text, Sesion.ClienteActual.Id);
                    }
                    else
                    {
                        vehiculo = new Vehiculo(tb_matricula_vehiculo.Text, tb_modelo_vehiculo.Text, tb_marca_vehiculo.Text, int.Parse(tb_id_cliente_vehiculo.Text));
                    }
                    connector.InsertarVehiculo(vehiculo);
                    actualizarDataGrid();
                    limpiarCasillas();
                }
            }
            catch
            {
                MessageBox.Show("No se ha encontrado ningún cliente con el Id '" + tb_id_cliente_vehiculo.Text + "'.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MenuItemEliminar_Click(object sender, RoutedEventArgs e)
        {
            var selectedRow = VehiculosDataGrid.SelectedItem;

            if (selectedRow != null)
            {
                Vehiculo vehiculoSeleccionado = (Vehiculo)selectedRow;
                MessageBoxResult result = MessageBox.Show(string.Concat("¿Estás seguro de que deseas eliminar el vehículo con matrícula '", vehiculoSeleccionado.Matricula, "'? \n"), "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    connector.EliminarVehiculoPorMatricula(vehiculoSeleccionado.Matricula);
                    actualizarDataGrid();
                }
            }
        }

        private void bt_cancelar_operacion_Click(object sender, RoutedEventArgs e)
        {
            if (tb_id_cliente_vehiculo.Text.Length > 0 || tb_marca_vehiculo.Text.Length > 0 || tb_modelo_vehiculo.Text.Length > 0 || tb_matricula_vehiculo.Text.Length > 0)
            {
                MessageBoxResult result = MessageBox.Show("¿Estás seguro de que deseas cancelar la operación?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    limpiarCasillas();
                    desactivarOpcionesEdicion();
                }
            }

        }

        private void rellenarCasillas(Vehiculo vehiculo)
        {
            tb_id_cliente_vehiculo.Text = vehiculo.ClienteId.ToString();
            tb_marca_vehiculo.Text = vehiculo.Marca;
            tb_matricula_vehiculo.Text = vehiculo.Matricula;
            tb_modelo_vehiculo.Text = vehiculo.Modelo;
        }

        private void limpiarCasillas()
        {
            tb_id_cliente_vehiculo.Text = "";
            tb_marca_vehiculo.Text = "";
            tb_matricula_vehiculo.Text = "";
            tb_modelo_vehiculo.Text = "";
        }

        private void actualizarDataGrid()
        {
            if (Sesion.ClienteActual.Rol.Equals("ADMIN"))
            {
                VehiculosDataGrid.ItemsSource = connector.SeleccionarTodosLosVehiculos();
            }
            else
            {
                VehiculosDataGrid.ItemsSource = connector.SeleccionarTodosLosVehiculosDelCliente(Sesion.ClienteActual.Id);
            }

        }

        private void activarOpcionesEdicion()
        {
            bt_crear_editar_vehiculo.Content = "Editar";
            tb_matricula_buscar.IsEnabled = false;
            bt_buscar_vehiculo_por_matricula.IsEnabled = false;
        }

        private void desactivarOpcionesEdicion()
        {
            tb_matricula_buscar.Text = "";
            bt_crear_editar_vehiculo.Content = "Crear";
            tb_matricula_buscar.IsEnabled = true;
            bt_buscar_vehiculo_por_matricula.IsEnabled = true;
        }

        private void bt_buscar_id_por_username_Click(object sender, RoutedEventArgs e)
        {
            if (tb_buscar_username.Text.Length > 0)
            {
                Cliente cliente = connector.SeleccionarClientePorUsername(tb_buscar_username.Text);
                if (cliente != null)
                {
                    tb_id_cliente_buscado.Text = cliente.Id.ToString();
                }
                else
                {
                    MessageBox.Show("No se ha encontrado ningún cliente con el nombre de usuario '" + tb_buscar_username.Text + "'.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("No puede estar este campo vacío. Por favor, introduzca un nombre de usuairo.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

    }
}
