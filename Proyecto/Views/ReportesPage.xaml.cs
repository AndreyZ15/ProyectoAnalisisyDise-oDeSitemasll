using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Proyecto.Models;
using Proyecto.Services;

namespace Proyecto.Views;

public partial class ReportesPage : ContentPage
{
    private readonly FireBaseService _firebaseService;

    public ReportesPage()
    {
        InitializeComponent();
        _firebaseService = Application.Current.Handler.MauiContext.Services.GetService<FireBaseService>();
    }

    private async void OnGenerarReporteClicked(object sender, EventArgs e)
    {
        if (ReportesPicker.SelectedItem == null)
        {
            await DisplayAlert("Error", "Seleccione un tipo de reporte", "OK");
            return;
        }

        string tipoReporte = ReportesPicker.SelectedItem.ToString();

        try
        {
            List<string> reporteDatos = await ObtenerDatosReporteAsync(tipoReporte);
            ReportesCollectionView.ItemsSource = reporteDatos;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudo generar el reporte: {ex.Message}", "OK");
        }
    }

    private async Task<List<string>> ObtenerDatosReporteAsync(string tipoReporte)
    {
        List<string> datos = new List<string>();

        switch (tipoReporte)
        {
            case "Ventas realizadas":
                datos = await GenerarReporteVentasAsync();
                break;

            case "Apartados pendientes":
                datos = await GenerarReporteApartadosAsync();
                break;

            case "Inventario disponible":
                datos = await GenerarReporteInventarioAsync();
                break;

            case "Clientes destacados":
                datos = await GenerarReporteClientesDestacadosAsync();
                break;

            default:
                datos.Add("No hay datos disponibles.");
                break;
        }

        return datos;
    }

    private async Task<List<string>> GenerarReporteVentasAsync()
    {
        List<string> datos = new List<string>();

        try
        {
            var ventas = await _firebaseService.GetAllVentasAsync();

            if (ventas.Count == 0)
            {
                datos.Add("No hay ventas registradas.");
                return datos;
            }

            // Encabezado del reporte
            datos.Add($"REPORTE DE VENTAS - {DateTime.Now.ToString("dd/MM/yyyy")}");
            datos.Add("-------------------------------------------");

            decimal totalGeneral = 0;

            foreach (var venta in ventas.OrderByDescending(v => v.Fecha))
            {
                // Obtener cliente
                var cliente = await _firebaseService.GetClienteAsync(venta.IDCliente);
                string nombreCliente = cliente != null
                    ? cliente.NombreCompleto()
                    : "Cliente desconocido";

                // Agregar datos de la venta
                datos.Add($"Venta #{venta.IDVenta} - Fecha: {venta.Fecha.ToString("dd/MM/yyyy HH:mm")}");
                datos.Add($"Cliente: {nombreCliente}");
                datos.Add($"Método de pago: {venta.MetodoPago}");
                datos.Add($"Total: ₡{venta.Total:N2}");
                datos.Add("-------------------------------------------");

                totalGeneral += venta.Total;
            }

            // Resumen
            datos.Add($"Total de ventas: {ventas.Count}");
            datos.Add($"Monto total: ₡{totalGeneral:N2}");
        }
        catch (Exception ex)
        {
            datos.Add($"Error al generar reporte: {ex.Message}");
        }

        return datos;
    }

    private async Task<List<string>> GenerarReporteApartadosAsync()
    {
        List<string> datos = new List<string>();

        try
        {
            var apartados = await _firebaseService.GetAllApartadosAsync();
            var apartadosPendientes = apartados.Where(a => !a.Completado).ToList();

            if (apartadosPendientes.Count == 0)
            {
                datos.Add("No hay apartados pendientes.");
                return datos;
            }

            // Encabezado del reporte
            datos.Add($"REPORTE DE APARTADOS PENDIENTES - {DateTime.Now.ToString("dd/MM/yyyy")}");
            datos.Add("-------------------------------------------");

            decimal totalPendiente = 0;

            foreach (var apartado in apartadosPendientes.OrderBy(a => a.Fecha))
            {
                // Obtener cliente
                var cliente = await _firebaseService.GetClienteAsync(apartado.IDCliente);
                string nombreCliente = cliente != null
                    ? cliente.NombreCompleto()
                    : "Cliente desconocido";

                // Calcular saldo pendiente
                decimal saldoPendiente = apartado.Total - apartado.Abono;

                // Agregar datos del apartado
                datos.Add($"Apartado #{apartado.IDApartado} - Fecha: {apartado.Fecha.ToString("dd/MM/yyyy")}");
                datos.Add($"Cliente: {nombreCliente}");
                datos.Add($"Abono: ₡{apartado.Abono:N2} de ₡{apartado.Total:N2}");
                datos.Add($"Saldo pendiente: ₡{saldoPendiente:N2}");
                datos.Add("-------------------------------------------");

                totalPendiente += saldoPendiente;
            }

            // Resumen
            datos.Add($"Total de apartados pendientes: {apartadosPendientes.Count}");
            datos.Add($"Monto total pendiente: ₡{totalPendiente:N2}");
        }
        catch (Exception ex)
        {
            datos.Add($"Error al generar reporte: {ex.Message}");
        }

        return datos;
    }

    private async Task<List<string>> GenerarReporteInventarioAsync()
    {
        List<string> datos = new List<string>();

        try
        {
            var productos = await _firebaseService.GetAllProductosAsync();

            if (productos.Count == 0)
            {
                datos.Add("No hay productos en el inventario.");
                return datos;
            }

            // Encabezado del reporte
            datos.Add($"REPORTE DE INVENTARIO - {DateTime.Now.ToString("dd/MM/yyyy")}");
            datos.Add("-------------------------------------------");

            // Agrupar por marca
            var productosPorMarca = productos.GroupBy(p => p.Marca).OrderBy(g => g.Key);

            foreach (var grupo in productosPorMarca)
            {
                datos.Add($"MARCA: {grupo.Key}");
                datos.Add("-------------------------------------------");

                foreach (var producto in grupo.OrderBy(p => p.Nombre))
                {
                    datos.Add($"{producto.Nombre} - {producto.Color} - Talla {producto.Talla}");
                    datos.Add($"Detalle: {producto.Detalle}");
                    datos.Add($"Precio: ₡{producto.Precio:N2}");
                    datos.Add("-------------------------------------------");
                }
            }

            // Resumen
            datos.Add($"Total de productos: {productos.Count}");
            datos.Add($"Valor total del inventario: ₡{productos.Sum(p => p.Precio):N2}");
        }
        catch (Exception ex)
        {
            datos.Add($"Error al generar reporte: {ex.Message}");
        }

        return datos;
    }

    private async Task<List<string>> GenerarReporteClientesDestacadosAsync()
    {
        List<string> datos = new List<string>();

        try
        {
            var ventas = await _firebaseService.GetAllVentasAsync();

            if (ventas.Count == 0)
            {
                datos.Add("No hay datos de ventas para generar este reporte.");
                return datos;
            }

            // Encabezado del reporte
            datos.Add($"REPORTE DE CLIENTES DESTACADOS - {DateTime.Now.ToString("dd/MM/yyyy")}");
            datos.Add("-------------------------------------------");

            // Agrupar ventas por cliente
            var ventasPorCliente = ventas.GroupBy(v => v.IDCliente)
                                         .Select(g => new
                                         {
                                             IDCliente = g.Key,
                                             CantidadCompras = g.Count(),
                                             TotalGastado = g.Sum(v => v.Total)
                                         })
                                         .OrderByDescending(x => x.CantidadCompras)
                                         .ThenByDescending(x => x.TotalGastado)
                                         .Take(10) // Top 10 clientes
                                         .ToList();

            foreach (var clienteStats in ventasPorCliente)
            {
                // Obtener información del cliente
                var cliente = await _firebaseService.GetClienteAsync(clienteStats.IDCliente);

                if (cliente != null)
                {
                    datos.Add($"{cliente.NombreCompleto()}");
                    datos.Add($"Compras realizadas: {clienteStats.CantidadCompras}");
                    datos.Add($"Total gastado: ₡{clienteStats.TotalGastado:N2}");
                    datos.Add("-------------------------------------------");
                }
            }

            if (ventasPorCliente.Count == 0)
            {
                datos.Add("No hay clientes con compras registradas.");
            }
        }
        catch (Exception ex)
        {
            datos.Add($"Error al generar reporte: {ex.Message}");
        }

        return datos;
    }
}