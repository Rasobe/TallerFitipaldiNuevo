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
    /// Lógica de interacción para VehiculoDeCliente.xaml
    /// </summary>
    public partial class VehiculoDeCliente : Window
    {

        MySqlConnector connector;

        public VehiculoDeCliente()
        {
            connector = new MySqlConnector("localhost", "TallerFitipaldiV", "root", "root"); ;
            InitializeComponent();
            VehiculosDataGrid.ItemsSource = connector.SeleccionarTodosLosVehiculosDelCliente(ClienteVer.clienteVerVehiculosReparaciones.Id);
        }
    }
}
