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
    /// Lógica de interacción para AnyadirPiezas.xaml
    /// </summary>
    public partial class AnyadirPiezas : Window
    {

        MySqlConnector connector;

        public AnyadirPiezas()
        {
            InitializeComponent();
            connector = new MySqlConnector("localhost", "TallerFitipaldiV", "root", "root");
            PiezasDataGrid.ItemsSource = connector.SeleccionarTodasLasPiezas();
        }

        private void PiezaDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Pieza pieza = PiezasDataGrid.SelectedItem as Pieza;

            if (pieza != null)
            {
                AnyadirPiezaVer.anyadirPiezaVer = pieza;
                VentanaEmergenteAnyadirPieza ventanaEmergenteAnyadirPieza = new VentanaEmergenteAnyadirPieza();
                ventanaEmergenteAnyadirPieza.Show();
                this.Close();
            }

        }
    }
}
