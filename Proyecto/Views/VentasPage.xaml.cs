using Proyecto.Models;
using Proyecto.Services;
using System;
using System.Collections.ObjectModel;

namespace Proyecto.Views;

public partial class VentasPage : ContentPage
{
    private readonly FireBaseService _firebaseService;
    public ObservableCollection<Cliente> Clientes { get; set; } = new ObservableCollection<Cliente>();
    public ObservableCollection<Venta> Ventas { get; set; } = new ObservableCollection<Venta>();

    public VentasPage()
    {
        InitializeComponent();
        _firebaseService = Application.Current.Handler.MauiContext.Services.GetService<FireBaseService>();

        ClientePicker.ItemsSource = Clientes;
        MetodoPagoPicker.ItemsSource = new List<string> { "Efectivo", "Tarjeta", "Transferencia" };

        CargarClientes();
        CargarVentas();
    }

    private async void CargarClientes()
    {
        try
        {
            var clientes = await _firebaseService.GetAllClientesAsync();
            Clientes.Clear();
            foreach (var cliente in clientes)
            {
                Clientes.Add(cliente);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudieron cargar los clientes: {ex.Message}", "OK");
        }
    }

    private async void CargarVentas()
    {
        try
        {
            var ventas = await _firebaseService.GetAllVentasAsync();
            Ventas.Clear();
            foreach (var venta in ventas)
            {
                Ventas.Add(venta);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudieron cargar las ventas: {ex.Message}", "OK");
        }
    }

    private async void OnAgregarVentaClicked(object sender, EventArgs e)
    {
        if (ClientePicker.SelectedItem is not Cliente clienteSeleccionado)
        {
            await DisplayAlert("Error", "Seleccione un cliente.", "OK");
            return;
        }

        if (MetodoPagoPicker.SelectedItem is not string metodoPago)
        {
            await DisplayAlert("Error", "Seleccione un método de pago.", "OK");
            return;
        }

        if (!decimal.TryParse(TotalEntry.Text, out decimal total) || total <= 0)
        {
            await DisplayAlert("Error", "Ingrese un total válido.", "OK");
            return;
        }

        try
        {
            var nuevaVenta = new Venta
            {
                IDVenta = new Random().Next(100000, 999999), // Simula un ID único
                IDCliente = clienteSeleccionado.IDCliente,
                MetodoPago = metodoPago,
                Total = total,
                Cliente = clienteSeleccionado,
                Fecha = DateTime.Now
            };

            await _firebaseService.AddVentaAsync(nuevaVenta);
            Ventas.Add(nuevaVenta);

            LimpiarCampos();
            await DisplayAlert("Éxito", "Venta agregada correctamente.", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudo agregar la venta: {ex.Message}", "OK");
        }
    }

    private void LimpiarCampos()
    {
        ClientePicker.SelectedItem = null;
        MetodoPagoPicker.SelectedItem = null;
        TotalEntry.Text = string.Empty;
    }
}
