using Microsoft.Win32;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;

namespace TallerFitipaldiNuevo.Clases
{

    public class Factura
    {

        private MySqlConnector connector;

        public int Numero { get; set; }
        public DateTime FechaEmision { get; set; }
        public Vehiculo Vehiculo { get; set; }
        public Cliente Cliente { get; set; }
        public Cliente Mecanico { get; set; }
        public List<PiezaViewReparacion> listaPiezas { get; set; }
        public decimal PrecioTotalPiezas { get; set; }
        public decimal PrecioTotalHoras { get; set; }
        public decimal PrecioConIva { get; set; }
        public decimal Iva { get; set; }

        public Factura(Reparacion reparacion)
        {
            if (reparacion != null)
            {
                connector = new MySqlConnector("localhost", "TallerFitipaldiV", "root", "root");
                PrecioConIva = reparacion.PrecioTotal;
                PrecioTotalPiezas = reparacion.PrecioSinIva;
                PrecioTotalHoras = reparacion.PrecioTotalHoras;
                Iva = reparacion.Iva;
                Vehiculo = connector.SeleccionarVehiculoPorId(reparacion.VehiculoId);
                Mecanico = connector.SeleccionarClientePorId(reparacion.MecanicoId);
                Cliente = connector.SeleccionarClientePorId(Vehiculo.ClienteId);
                listaPiezas = connector.SeleccionarPiezasPorIdReparacion(reparacion.Id);
                FechaEmision = DateTime.Now;
            }
        }

        public void Imprimir()
        {

            using (var document = new PdfDocument())
            {
                var page = document.AddPage();

                var gfx = XGraphics.FromPdfPage(page);

                // Create a font for drawing text
                var fontTitle = new XFont("Arial", 30);
                var font = new XFont("Arial", 12);

                var x = 50;
                var y = 70;

                gfx.DrawString("Taller Fitipaldi", fontTitle, XBrushes.Black, x, y);
                y += 40;

                gfx.DrawString("Reparación", font, XBrushes.Black, x, y);
                y += 20;

                gfx.DrawString($"Mecánico/a a cargo: {Mecanico.Nombre} {Mecanico.Apellidos}", font, XBrushes.Black, x, y);
                y += 20;

                gfx.DrawString($"Vehículo: {Vehiculo.Marca} {Vehiculo.Modelo}", font, XBrushes.Black, x, y);
                y += 20;

                gfx.DrawString($"Cliente: {Cliente.Nombre} {Cliente.Apellidos}", font, XBrushes.Black, x, y);
                y += 40;

                gfx.DrawString("Piezas a cambiar:", new XFont("Arial", 15), XBrushes.Black, x, y);
                y += 20;

                Pieza pieza;
                foreach (var parte in listaPiezas)
                {
                    pieza = connector.SeleccionarPiezaPorNombre(parte.Nombre);

                    // Draw the part name
                    gfx.DrawString(parte.Nombre + " (" + parte.Cantidad + ")", font, XBrushes.Black, x, y);

                    // Draw the price aligned to the right
                    string priceString = $"{pieza.Precio:C2}";
                    double priceWidth = gfx.MeasureString(priceString, font).Width;
                    double rightMargin = page.Width - x - priceWidth;
                    gfx.DrawString(priceString, font, XBrushes.Black, rightMargin, y);

                    // Draw dots between part name and price
                    string dotsString = "..........................................";
                    double dotsWidth = gfx.MeasureString(dotsString, font).Width;
                    double dotsX = x + gfx.MeasureString(parte.Nombre + " (" + parte.Cantidad + ")", font).Width;

                    while (dotsX + dotsWidth < rightMargin)
                    {
                        gfx.DrawString(dotsString, font, XBrushes.Black, dotsX, y);
                        dotsX += dotsWidth;
                    }
                    if (dotsX < rightMargin)
                    {
                        int remainingDots = (int)((rightMargin - dotsX) / gfx.MeasureString(".", font).Width);
                        string remainingDotsStr = new String('.', remainingDots);
                        gfx.DrawString(remainingDotsStr, font, XBrushes.Black, dotsX, y);
                    }

                    y += 20;
                }

                string text = $"Precio sin IVA (Piezas): {PrecioTotalPiezas:C2}";

                double textWidth = gfx.MeasureString(text, font).Width;
                double x2 = page.Width - textWidth - 50;

                // Draw the right-aligned text
                gfx.DrawString(text, font, XBrushes.Black, x2, y);
                gfx.DrawString($"IVA: {Iva}", font, XBrushes.Black, x2, y);
                gfx.DrawString($"Precio con IVA (Piezas): {PrecioConIva}", font, XBrushes.Black, x2, y);

                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.FileName = $"Factura_{Numero}.pdf";
                y += 20;

                saveFileDialog1.Filter = "PDF File|*.pdf";
                y += 20;

                saveFileDialog1.Title = "Guardar archivo PDF";
                y += 20;

                if (saveFileDialog1.ShowDialog() == true)
                {
                    try
                    {
                        document.Save(saveFileDialog1.FileName);
                        Process.Start(saveFileDialog1.FileName);
                    }
                    catch
                    {
                        MessageBox.Show("No se pudo guardar o reemplazar el archivo. Si está abierto otro archivo con el mismo nombre, por favor ciérrelo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

    }
}
