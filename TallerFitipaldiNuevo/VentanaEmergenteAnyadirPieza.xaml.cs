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
    /// Lógica de interacción para VentanaEmergenteAnyadirPieza.xaml
    /// </summary>
    public partial class VentanaEmergenteAnyadirPieza : Window
    {

        private MySqlConnector connector;

        public VentanaEmergenteAnyadirPieza()
        {
            InitializeComponent();
            connector = new MySqlConnector("localhost", "TallerFitipaldiV", "root", "root");
            tit_nombre_pieza.Content = AnyadirPiezaVer.anyadirPiezaVer.Nombre;
        }

        private void bt_anyadir_pieza_Click(object sender, RoutedEventArgs e)
        {
            if (tb_cantidad.Text.All(c => char.IsDigit(c)) && tb_cantidad.Text.Length > 0)
            {
                MessageBoxResult result = MessageBox.Show(string.Concat("¿Estás seguro de que deseas añadir la cantidad de ", tb_cantidad.Text, "Ud a la pieza '", AnyadirPiezaVer.anyadirPiezaVer.Nombre, "'? \n"), "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    connector.IncrementarStockPieza(AnyadirPiezaVer.anyadirPiezaVer.Nombre, int.Parse(tb_cantidad.Text));
                    this.Close();
                    AnyadirPiezaVer.reparaciones.actualizarCbPiezas();
                }
            }
            else
            {
                MessageBox.Show("Debes rellenar ambos campos con carácteres numéricos o no puedes poner una cantidad inferior a 0.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                tb_cantidad.Text = "0";
                tb_cantidad.Focus();
            }
        }
    }
}
