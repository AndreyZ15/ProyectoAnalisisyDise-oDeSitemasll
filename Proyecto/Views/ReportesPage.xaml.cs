using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;

namespace Proyecto.Views;

    public partial class ReportesPage : ContentPage
    {
        public ReportesPage()
        {
            InitializeComponent();
        }

        private void OnGenerarReporteClicked(object sender, EventArgs e)
        {
            if (ReportesPicker.SelectedItem == null)
            {
                DisplayAlert("Error", "Seleccione un tipo de reporte", "OK");
                return;
            }

            string tipoReporte = ReportesPicker.SelectedItem.ToString();
            List<string> reporteDatos = ObtenerDatosReporte(tipoReporte);

            ReportesCollectionView.ItemsSource = reporteDatos;
        }

    //Hacer los reportes y se llama en esta funcion
    private List<string> ObtenerDatosReporte(string tipoReporte)
        {
            List<string> datos = new List<string>();

            switch (tipoReporte)
            {
                case "Ventas realizadas":
                    datos.Add("Venta 1 - Cliente: Juan - ₡50,000");
                    datos.Add("Venta 2 - Cliente: María - ₡30,000");
                    break;

                case "Apartados pendientes":
                    datos.Add("Apartado 1 - Cliente: Pedro - ₡10,000 de ₡40,000");
                    datos.Add("Apartado 2 - Cliente: Ana - ₡5,000 de ₡25,000");
                    break;

                case "Inventario disponible":
                    datos.Add("Vestido rojo - Talla M - ₡15,000");
                    datos.Add("Vestido azul - Talla L - ₡18,000");
                    break;

                case "Clientes destacados":
                    datos.Add("Juan Pérez - 5 compras");
                    datos.Add("María Gómez - 3 compras");
                    break;

                default:
                    datos.Add("No hay datos disponibles.");
                    break;
            }

            return datos;
        }
    }

