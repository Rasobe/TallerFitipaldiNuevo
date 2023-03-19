

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using TallerFitipaldiNuevo.Clases;
using Color = System.Windows.Media.Color;
using MessageBox = System.Windows.MessageBox;

namespace TallerFitipaldiNuevo
{
    public partial class Reparaciones : Window
    {
        private MySqlConnector connector;

        private int vehiculoId;
        private string diaInicioReparacion;

        public Reparaciones()
        {
            InitializeComponent();
            connector = new MySqlConnector("localhost", "TallerFitipaldiV", "root", "root");
            
            inicioAplicacion();
            titulo_nombre_apellidos.Content = Sesion.ClienteActual.Nombre + " " + Sesion.ClienteActual.Apellidos;
            actualizarCbPiezas();
        }

        public void actualizarCbPiezas()
        {
            List<Pieza> piezas = connector.SeleccionarTodasLasPiezas();
            cb_piezas.Items.Clear();
            foreach (Pieza pieza in piezas)
            {
                cb_piezas.Items.Add(pieza.Nombre + " - " + pieza.Stock + " Ud");
            }
        }

        private void inicioAplicacion()
        {
            tb_vehiculo_seleccionado.Text = string.Empty;
            cb_piezas.SelectedIndex = -1;
            cb_piezas_elegidas.SelectedIndex = -1;
            tb_cantidad_pieza.Text = string.Empty;
            cb_piezas_elegidas.IsEnabled = false;
            cb_piezas_elegidas.Items.Clear();
            bt_anyadir_pieza.Content = "Añadir";
            cb_piezas.IsEnabled = true;
            tb_horas.Text = "0";
            tb_precio_hora.Text = "0";
            tb_precio_total_horas.Text = "0,0";
            tb_precio_sin_iva.Text = "0,0";
            tb_iva.Text = "21";
            tb_precio_total.Text = "0,0";
            dia_elegido_inicio.SelectedDate = DateTime.Now;
        }

        private void buscar_cliente_por_username(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                bt_buscar_cliente_por_username_Click(sender, e);
            }
        }

        private void bt_buscar_cliente_por_username_Click(object sender, RoutedEventArgs e)
        {
            inicioAplicacion();

            if (tb_buscar_cliente_username.Text.Length > 0)
            {
                Cliente cliente = connector.SeleccionarClientePorUsername(tb_buscar_cliente_username.Text);
                if (cliente != null)
                {
                    VehiculosDataGrid.ItemsSource = connector.SeleccionarTodosLosVehiculosDelCliente(cliente.Id);
                }
                else
                {
                    MessageBox.Show("No se ha encontrado ningún cliente con el username '" + tb_buscar_cliente_username.Text + "'.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Debe estar el campo relleno para iniciar la búsqueda.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void VehiculosDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Vehiculo vehiculoSeleccionado = (Vehiculo)VehiculosDataGrid.SelectedItem;
            if (vehiculoSeleccionado != null)
            {
                tb_vehiculo_seleccionado.Text = vehiculoSeleccionado.ToString();

                Match match = Regex.Match(vehiculoSeleccionado.ToString(), @"Matricula\s*=\s*(\w+)");
                if (match.Success)
                {
                    vehiculoId = connector.SeleccionarVehiculoPorMatricula(match.Groups[1].Value).Id;
                }
            }
        }

        private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (tb_vehiculo_seleccionado.Text.Length == 0 && cb_piezas.SelectedIndex != -1)
            {
                cb_piezas.SelectedIndex = -1;
                MessageBox.Show("Debe haber seleccionado un vehículo posteriormente.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                tb_buscar_cliente_username.Focus();
            }
        }

        private void bt_anyadir_pieza_Click(object sender, RoutedEventArgs e)
        {
            if (tb_cantidad_pieza.Text.All(char.IsDigit) && tb_cantidad_pieza.Text.Length > 0)
            {
                String nombrePieza = "";
                int stockPieza = 0;
                int cantidadPedida = 0;

                if (bt_anyadir_pieza.Content.Equals("Editar"))
                {
                    bool cambioExitoso = false;
                    if (tb_cantidad_pieza.Text == "0")
                    {
                        cb_piezas_elegidas.Items.Remove(cb_piezas_elegidas.SelectedItem);
                        cambioExitoso = true;
                        if (cb_piezas_elegidas.Items.Count == 0)
                        {
                            cb_piezas_elegidas.SelectedIndex = -1;
                            cb_piezas_elegidas.IsEnabled = false;
                        }
                        else
                        {
                            cb_piezas_elegidas.IsEnabled = true;
                        }
                    }
                    else
                    {
                        nombrePieza = cb_piezas_elegidas.Text.Split('-')[0].Trim();
                        stockPieza = connector.SeleccionarStockDePiezaPorNombre(nombrePieza);

                        cantidadPedida = int.Parse(tb_cantidad_pieza.Text);

                        if (cantidadPedida < 0 || cantidadPedida > stockPieza)
                        {
                            MessageBox.Show("No puedes poner una cantidad mayor a la que hay de stock o inferior a 0. Si desea tener más piezas, vaya al menú de piezas y encargue piezas.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        else
                        {
                            cb_piezas_elegidas.IsEnabled = true;
                            cambioExitoso = true;
                        }
                    }

                    if (cambioExitoso)
                    {
                        bt_anyadir_pieza.Content = "Añadir";
                        cb_piezas_elegidas.Items.Remove(cb_piezas_elegidas.SelectedItem);
                        atributosActualizadosAlEditarAnyadirPieza(nombrePieza, cantidadPedida);
                    }

                }
                else
                {
                    nombrePieza = cb_piezas.Text.Split('-')[0].Trim();
                    stockPieza = connector.SeleccionarStockDePiezaPorNombre(nombrePieza);
                    int stockPiezaRequerido = int.Parse(tb_cantidad_pieza.Text);

                    if (stockPieza < stockPiezaRequerido && stockPieza > 0)
                    {
                        MessageBox.Show("No puedes poner una cantidad mayor a la que hay de stock o inferior a 0. Si desea tener más piezas, vaya al menú de piezas y encargue piezas.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        bool piezaEncontrada = false;
                        foreach (string piezaYaElegida in cb_piezas_elegidas.Items)
                        {
                            if (nombrePieza.Equals(piezaYaElegida.Split('-')[0].Trim()))
                            {
                                piezaEncontrada = true;
                                break;
                            }
                        }

                        if (piezaEncontrada)
                        {
                            MessageBox.Show("No puedes añadir una pieza que has añadido anteriormente.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                            cb_piezas.SelectedIndex = -1;
                            tb_cantidad_pieza.Text = string.Empty;
                        }
                        else
                        {
                            cb_piezas_elegidas.IsEnabled = true;
                            atributosActualizadosAlEditarAnyadirPieza(nombrePieza, stockPiezaRequerido);
                        }
                    }
                }
                actualizarPrecios();
            }
            else
            {
                MessageBox.Show("Solo se adminten números como cantidad.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                tb_cantidad_pieza.Focus();
            }

        }

        public void atributosActualizadosAlEditarAnyadirPieza(string nombrePieza, int stockPiezaRequerido)
        {
            if (stockPiezaRequerido > 0)
            {
                cb_piezas_elegidas.Items.Add(nombrePieza + " - " + stockPiezaRequerido + " Ud ");
            }
            cb_piezas.SelectedIndex = -1;
            tb_cantidad_pieza.Text = String.Empty;
            cb_piezas_elegidas.SelectedIndex = -1;
            cb_piezas.IsEnabled = true;
        }

        private void tb_cantidad_pieza_GotFocus(object sender, RoutedEventArgs e)
        {
            if (cb_piezas.Text == string.Empty)
            {
                MessageBox.Show("Debes elegir primero una pieza para poder añadir una cantidad.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                cb_piezas.Focus();
            }
        }

        private void editarPieza_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

            if (cb_piezas_elegidas.SelectedIndex != -1)
            {
                string piezaElegida = cb_piezas_elegidas.SelectedItem.ToString();
                string nombrePiezaAModificar = cb_piezas_elegidas.SelectedItem.ToString().Split('-')[0].Trim();
                foreach (String pieza in cb_piezas.Items)
                {
                    string nombrePieza = pieza.Split('-')[0].Trim();

                    if (nombrePiezaAModificar.Equals(nombrePieza, StringComparison.OrdinalIgnoreCase))
                    {
                        Match match = Regex.Match(piezaElegida, @"\d+");
                        if (match.Success)
                        {
                            cb_piezas.SelectedItem = pieza;
                            tb_cantidad_pieza.Text = match.Value;
                            bt_anyadir_pieza.Content = "Editar";
                            cb_piezas_elegidas.IsEnabled = false;
                            cb_piezas.IsEnabled = false;
                        }
                    }
                }
            }
        }

        private void tb_vehiculo_seleccionado_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            cb_piezas.IsEnabled = true;
            cb_piezas.SelectedIndex = -1;
            tb_cantidad_pieza.Text = string.Empty;
            bt_anyadir_pieza.Content = "Añadir";
            cb_piezas_elegidas.Items.Clear();
            cb_piezas_elegidas.IsEnabled = false;
        }

        private void bt_ocultar_mostrar_Click(object sender, RoutedEventArgs e)
        {
            if (bt_ocultar_mostrar.Content.Equals("Mostrar"))
            {
                bt_ocultar_mostrar.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xA0, 0x00, 0x00));
                bt_ocultar_mostrar.Content = "Ocultar";
                tb_instrucciones.Visibility = Visibility.Visible;
            }
            else
            {
                bt_ocultar_mostrar.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0xA0, 0x0A));
                bt_ocultar_mostrar.Content = "Mostrar";
                tb_instrucciones.Visibility = Visibility.Collapsed;
            }
        }

        private void actualizarPrecios()
        {
            decimal precioTotalSinIva = 0;
            foreach (string pieza in cb_piezas_elegidas.Items)
            {
                string nombrePieza = pieza.Split('-')[0].Trim();
                Match match = Regex.Match(pieza, @"\d+");
                if (match.Success)
                {
                    precioTotalSinIva += int.Parse(match.Value) * connector.SeleccionarPiezaPorNombre(nombrePieza).Precio;
                }
            }

            tb_precio_sin_iva.Text = formatearNumero(precioTotalSinIva);

            decimal iva = decimal.Parse("1," + tb_iva.Text);

            tb_precio_total.Text = formatearNumero(precioTotalSinIva * iva);

        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            // Generar el contenido de la factura aquí
            e.Graphics.DrawString("Factura", new Font("Arial", 20), System.Drawing.Brushes.Black, 20, 20);
        }

        private void anyadir_horas_precio(object sender, RoutedEventArgs e)
        {
            if (tb_horas.Text.All(c => char.IsDigit(c)) && tb_horas.Text.Length > 0)
            {
                tb_precio_total_horas.Text = formatearNumero(decimal.Parse(tb_horas.Text) * decimal.Parse(tb_precio_hora.Text));
            }
            else
            {
                MessageBox.Show("Debes rellenar ambos campos con carácteres numéricos o no puedes poner una cantidad inferior a 0.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                tb_horas.Text = "0";
                tb_horas.Focus();
            }
        }

        private void anyadir_horas_precio2(object sender, RoutedEventArgs e)
        {
            if (tb_precio_hora.Text.All(c => char.IsDigit(c)) && tb_precio_hora.Text.Length > 0)
            {

                tb_precio_total_horas.Text = formatearNumero(decimal.Parse(tb_horas.Text) * decimal.Parse(tb_precio_hora.Text));
            }
            else
            {
                MessageBox.Show("Debes rellenar ambos campos con carácteres numéricos o no puedes poner una cantidad inferior a 0.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                tb_precio_hora.Text = "0";
                tb_precio_hora.Focus();
            }
        }

        private void dia_elegido_inicio_SelectedDatesChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            DateTime? selectedDate = dia_elegido_inicio.SelectedDate;
            if (selectedDate.HasValue)
            {
                diaInicioReparacion = selectedDate.Value.ToShortDateString();
            }
        }

        private void bt_crear_reparacion_Click(object sender, RoutedEventArgs e)
        {

            // Comprobaciones previas a la creación de la reparación

            if (tb_buscar_cliente_username.Text.Length > 0 && tb_vehiculo_seleccionado.Text.Length > 0 && tb_horas.Text.Length > 0 && tb_precio_total_horas.Text.Length > 0 && diaInicioReparacion != null && cb_piezas_elegidas.Items.Count > 0)
            {
                Vehiculo vehiculo = connector.SeleccionarVehiculoPorId(vehiculoId);
                Cliente clienteDelVehiculo = connector.SeleccionarClientePorId(vehiculo.ClienteId);
                string todasLasPiezas = "";

                foreach (string pieza in cb_piezas_elegidas.Items)
                {
                    todasLasPiezas += pieza + ", ";
                }

                var dialog = new ConfirmarCrearReparacion(
                        clienteDelVehiculo.Nombre + " " + clienteDelVehiculo.Apellidos,
                        vehiculo.Tipo + ": " + vehiculo.Marca + " " + vehiculo.Modelo,
                        todasLasPiezas,
                        tb_horas.Text,
                        diaInicioReparacion,
                        Math.Round((float.Parse(tb_precio_total_horas.Text) + float.Parse(tb_precio_total.Text)), 2).ToString() + " €"
                    );

                dialog.ShowDialog();

                if (dialog.Confirmed)
                {
                    // Insertamos la reparación
                    Reparacion reparacion = new Reparacion(vehiculoId, decimal.Parse(tb_horas.Text), decimal.Parse(tb_precio_hora.Text), decimal.Parse(tb_precio_sin_iva.Text), decimal.Parse(tb_iva.Text), DateTime.Parse(diaInicioReparacion), Sesion.ClienteActual.Id);
                    connector.InsertarReparacion(reparacion);

                    // Antes de introducir lo del paso siguiente, tenemos que pasar el nombre de la pieza a su respectivo id y separarla de su cantidad pedida.
                    List<string> listaPiezas = cb_piezas_elegidas.Items.Cast<string>().ToList();
                    List<(int, int)> listaPiezasBD = new List<(int, int)>();
                    string nombrePieza = "";
                    int idPieza = 0;
                    int cantidadPieza = 0;
                    for (int i = 0; i < listaPiezas.Count; i++)
                    {
                        nombrePieza = listaPiezas[i].Split('-')[0].Trim();
                        idPieza = connector.SeleccionarPiezaPorNombre(nombrePieza).Id;

                        Match match = Regex.Match(listaPiezas[i], @"\d+");
                        if (match.Success)
                        {
                            cantidadPieza = int.Parse(match.Value);
                        }

                        listaPiezasBD.Add((idPieza, cantidadPieza));

                    }

                    // Recogemos el ID de la reparación que hemos creado anteriormente
                    int idReparacion = connector.SeleccionarIdReparacionPorReparacion(reparacion);
                    Console.WriteLine("Id reparacion: " + idReparacion.ToString());

                    // Introducimos las piezas con su reparación correspondiente y actualizamos el Stock de la tabla de Piezas.
                    Console.WriteLine(idReparacion);
                    connector.InsertarPiezasReparacionPorIdReparacion(idReparacion, listaPiezasBD);

                    // Actualizamos el Stock de las piezas
                    connector.ActualizarStockPieza(listaPiezasBD);

                    // Iniciamos el menú reparaciones
                    MenuReparaciones menuReparaciones = new MenuReparaciones();
                    menuReparaciones.Show();
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Debes rellenar todos los campos necesarios para hacer una reparación.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void bt_limpiar_todo_Click(object sender, RoutedEventArgs e)
        {
            inicioAplicacion();
        }

        private string formatearNumero(decimal numero)
        {
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = ",";
            nfi.NumberGroupSeparator = ".";
            return numero.ToString("N2", nfi);
        }

        private void bt_anyadir_piezas_Click(object sender, RoutedEventArgs e)
        {
            AnyadirPiezaVer.reparaciones = this;
            AnyadirPiezas anyadirPiezas = new AnyadirPiezas();
            anyadirPiezas.Show();
        }
    }
}
