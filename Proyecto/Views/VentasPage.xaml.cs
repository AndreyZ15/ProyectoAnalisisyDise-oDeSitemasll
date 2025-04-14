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
        _firebaseService = Application.Current.Handler?.MauiContext?.Services.GetService<FireBaseService>()
                           ?? throw new InvalidOperationException("FireBaseService not found in service provider.");

        ClientePicker.ItemsSource = Clientes;
        MetodoPagoPicker.ItemsSource = new List<string> { "Efectivo", "Tarjeta", "Transferencia" };

        CargarClientes();
        CargarVentas();
    }

    private void OnClientePickerSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ClientePicker.SelectedItem is Cliente clienteSeleccionado)
        {
            // Actualizar las etiquetas con los datos del cliente seleccionado
            NombreClienteLabel.Text = $"Nombre: {clienteSeleccionado.Nombre} {clienteSeleccionado.Apellido}";


        }
        else
        {
            // Limpiar las etiquetas si no hay cliente seleccionado
            NombreClienteLabel.Text = "Nombre: ";
            
        }
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
                if (venta != null)
                {
                    Ventas.Add(venta);
                }
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

        if (!decimal.TryParse(TotalVentaEntry.Text, out decimal total) || total <= 0)
        {
            await DisplayAlert("Error", "Ingrese un total válido.", "OK");
            return;
        }

        try
        {
            var nuevaVenta = new Venta
            {
                IDCliente = clienteSeleccionado.IDCliente,
                MetodoPago = metodoPago,
                Total = total,
                Cliente = clienteSeleccionado,
                Fecha = DateTime.Now
            };

            int idVenta = await _firebaseService.AddVentaAsync(nuevaVenta);
            nuevaVenta.IDVenta = idVenta;
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
        NombreClienteLabel.Text = "Nombre: ";
        ClientePicker.SelectedItem = null;
        MetodoPagoPicker.SelectedItem = null;
        TotalVentaEntry.Text = string.Empty;
    }
}
