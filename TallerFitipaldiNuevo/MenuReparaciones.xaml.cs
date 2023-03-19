using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TallerFitipaldiNuevo.Clases;

namespace TallerFitipaldiNuevo
{
    /// <summary>
    /// Lógica de interacción para MenuReparaciones.xaml
    /// </summary>
    public partial class MenuReparaciones : Window
    {
        private MySqlConnector connector;

        public MenuReparaciones()
        {
            InitializeComponent();
            connector = new MySqlConnector("localhost", "TallerFitipaldiV", "root", "root");
            ReparacionDataGrid.ItemsSource = connector.SeleccionarReparacionesPorMecanicoId(Sesion.ClienteActual.Id);
        }

        private void bt_crear_reparacion_Click(object sender, RoutedEventArgs e)
        {
            Reparaciones reparaciones = new Reparaciones();
            reparaciones.Show();
            this.Close();
        }

        private void ReparacionDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Reparacion reparacion = ReparacionDataGrid.SelectedItem as Reparacion;
            Console.WriteLine(reparacion.ToString());
        }
        private void cb_piezas_no_cambiar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            comboBox.SelectedIndex = -1;
        }

        private void MenuItemImprimirFactura_Click(object sender, RoutedEventArgs e)
        {
            Factura factura = new Factura(ReparacionDataGrid.SelectedItem as Reparacion);
            factura.Imprimir();
        }

        private void MenuItemFinalizar_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
