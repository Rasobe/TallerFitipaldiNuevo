using System;
using System.Collections.Generic;
using System.Linq;
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
    public partial class MenuReparacionesCliente : Window
    {
        private MySqlConnector connector;

        public MenuReparacionesCliente()
        {
            InitializeComponent();
            connector = new MySqlConnector("localhost", "TallerFitipaldiV", "root", "root");
            ReparacionDataGrid.ItemsSource = connector.SeleccionarReparacionesPorMecanicoId(Sesion.ClienteActual.Id);
        }

        private void bt_crear_reparacion_Click(object sender, RoutedEventArgs e)
        {
            Reparaciones reparaciones = new Reparaciones();
            reparaciones.Show();
        }

        private void ReparacionDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
