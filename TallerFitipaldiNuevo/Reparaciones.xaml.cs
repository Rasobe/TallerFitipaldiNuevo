

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using TallerFitipaldiNuevo.Clases;
using MessageBox = System.Windows.MessageBox;

namespace TallerFitipaldiNuevo
{
    /// <summary>
    /// Lógica de interacción para Reparaciones.xaml
    /// </summary>
    public partial class Reparaciones : Window
    {
        MySqlConnector connector;
        public Reparaciones()
        {
            InitializeComponent();
            connector = new MySqlConnector("localhost", "TallerFitipaldiV", "root", "root");
            List<Pieza> piezas = connector.SeleccionarTodasLasPiezas();
            Console.WriteLine(piezas.Count);
            foreach (Pieza pieza in piezas)
            {
                cb_piezas.Items.Add(pieza.Nombre + " - " + pieza.Stock + " Ud");
            }
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
            tb_vehiculo_seleccionado.Text = string.Empty;
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
            if (vehiculoSeleccionado != null )
            {
                tb_vehiculo_seleccionado.Text = vehiculoSeleccionado.ToString();
            }
        }

        private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (tb_vehiculo_seleccionado.Text.Length == 0)
            {
                cb_piezas.SelectedIndex = -1;
                MessageBox.Show("Debe haber seleccionado un vehículo posteriormente.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }               
        }

        private void solo_numeros(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {

            }
        }

        private void bt_anyadir_pieza_Click(object sender, RoutedEventArgs e)
        {
            if (tb_cantidad_pieza.Text.All(char.IsDigit))
            {
                String nombrePieza = cb_piezas.Text.Substring(0, cb_piezas.Text.IndexOf(" "));
                Console.WriteLine(nombrePieza);
            }
            else
            {
                MessageBox.Show("Solo se adminten números como cantidad.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
