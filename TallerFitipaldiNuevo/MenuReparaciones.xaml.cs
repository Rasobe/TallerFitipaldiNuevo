using System.Windows;
using System.Windows.Controls;
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

            if (Sesion.ClienteActual.Rol.Equals("MECANICO"))
            {
                ReparacionDataGrid.ItemsSource = connector.SeleccionarReparacionesPorMecanicoId(Sesion.ClienteActual.Id);
                bt_crear_reparacion.Visibility = Visibility.Visible;
            }
            else if (Sesion.ClienteActual.Rol.Equals("ADMIN"))
            {
                bt_crear_reparacion.Visibility = Visibility.Collapsed;
                ReparacionDataGrid.ItemsSource = connector.SeleccionarTodasReparaciones();
            }

        }

        private void bt_crear_reparacion_Click(object sender, RoutedEventArgs e)
        {
            Reparaciones reparaciones = new Reparaciones();
            reparaciones.Show();
            this.Close();
        }

        private void ReparacionDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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
            if (Sesion.ClienteActual.Rol.Equals("MECANICO"))
            {
                Reparacion r = (Reparacion)ReparacionDataGrid.SelectedItem;
                MessageBoxResult result = MessageBox.Show(string.Concat("¿Estás seguro de que deseas finalizar la reparación con Id '", r.Id, "'? \n"), "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    connector.FinalizarReparacionPorIdReparacion(r.Id);
                    ReparacionDataGrid.ItemsSource = connector.SeleccionarReparacionesPorMecanicoId(Sesion.ClienteActual.Id);
                }
            } 
            else
            {
                MessageBox.Show("No tienes permiso para hacer esta acción.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MenuItemEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (Sesion.ClienteActual.Rol.Equals("MECANICO"))
            {
                Reparacion r = (Reparacion)ReparacionDataGrid.SelectedItem;
                MessageBoxResult result = MessageBox.Show(string.Concat("¿Estás seguro de que deseas eliminar la reparación con Id '", r.Id, "'? \n"), "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    connector.EliminarReparacionPorIdReparacion(r.Id);
                    ReparacionDataGrid.ItemsSource = connector.SeleccionarReparacionesPorMecanicoId(Sesion.ClienteActual.Id);
                }
            } 
            else
            {
                MessageBox.Show("No tienes permiso para hacer esta acción.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
