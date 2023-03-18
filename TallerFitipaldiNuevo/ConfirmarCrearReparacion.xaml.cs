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

namespace TallerFitipaldiNuevo
{
    /// <summary>
    /// Lógica de interacción para ConfirmarCrearReparacion.xaml
    /// </summary>
    public partial class ConfirmarCrearReparacion : Window
    {
        public bool Confirmed { get; private set; }

        public ConfirmarCrearReparacion(string cliente, string vehiculo, string piezas,
                              string horas, string diaInicio, string precioTotal)
        {
            InitializeComponent();
            ClienteText.Text = cliente;
            VehiculoText.Text = vehiculo;
            PiezasText.Text = piezas;
            HorasText.Text = horas;
            DiaInicioText.Text = diaInicio;
            PrecioTotalText.Text = precioTotal;

            Confirmed = false;
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            Confirmed = true;
            Close();
        }

    }
}
