using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Proyecto.Models;
using Proyecto.Services;

namespace Proyecto.Views;

public partial class VentasPage : ContentPage, INotifyPropertyChanged
{
    private readonly FireBaseService _firebaseService;
    public ObservableCollection<Cliente> Clientes { get; set; } = new();
    public ObservableCollection<Venta> Ventas { get; set; } = new();

    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            if (_isBusy != value)
            {
                _isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }
    }

    public VentasPage()
    {
        InitializeComponent();
        _firebaseService = Application.Current.Handler?.MauiContext?.Services.GetService<FireBaseService>()
                           ?? throw new InvalidOperationException("FireBaseService not found in service provider.");

        // Establecer el contexto de datos
        this.BindingContext = this;

        ClientePicker.ItemsSource = Clientes;
        MetodoPagoPicker.ItemsSource = new List<string> { "Efectivo", "Tarjeta", "Transferencia" };
        VentasCollectionView.ItemsSource = Ventas;

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
            IsBusy = true;

            var ventas = await _firebaseService.GetAllVentasAsync();
            Ventas.Clear();

            foreach (var venta in ventas.OrderByDescending(v => v.Fecha))
            {
                // Verificar que el ID no sea cero o negativo
                if (venta.IDVenta <= 0)
                {
                    Console.WriteLine($"ADVERTENCIA: Venta con ID inválido: {venta.IDVenta}");
                    continue;
                }

                // Asignar cliente si es posible
                try
                {
                    if (!string.IsNullOrEmpty(venta.IDCliente))
                    {
                        venta.Cliente = await _firebaseService.GetClienteAsync(venta.IDCliente);
                    }
                    else
                    {
                        venta.Cliente = new Cliente { Nombre = "Cliente desconocido" };
                    }
                }
                catch (Exception clienteEx)
                {
                    Console.WriteLine($"Error obteniendo cliente: {clienteEx.Message}");
                    venta.Cliente = new Cliente { Nombre = "Cliente desconocido" };
                }

                // Agregar a la colección
                Ventas.Add(venta);

                // Verificar si el binding está funcionando
                Console.WriteLine($"Agregada venta a UI - ID: {venta.IDVenta}, Cliente: {venta.Cliente?.Nombre}");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudieron cargar las ventas: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void OnClientePickerSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ClientePicker.SelectedItem is Cliente clienteSeleccionado)
        {
            NombreClienteLabel.Text = $"Nombre: {clienteSeleccionado.Nombre} {clienteSeleccionado.Apellido}";
        }
        else
        {
            NombreClienteLabel.Text = "Nombre: ";
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
            IsBusy = true;

            var nuevaVenta = new Venta
            {
                IDCliente = clienteSeleccionado.IDCliente,
                MetodoPago = metodoPago,
                Total = total,
                Fecha = DateTime.Now
            };

            try
            {
                int idVenta = await _firebaseService.AddVentaAsync(nuevaVenta);
                nuevaVenta.IDVenta = idVenta;
                nuevaVenta.Cliente = clienteSeleccionado;

                // Insertar al principio de la colección
                Ventas.Insert(0, nuevaVenta);

                LimpiarCampos();
                await DisplayAlert("Éxito", "Venta agregada correctamente.", "OK");
            }
            catch (Exception fbEx)
            {
                Console.WriteLine($"Error al agregar venta: {fbEx.Message}");
                await DisplayAlert("Error", $"No se pudo agregar la venta. Por favor intente nuevamente. Detalles: {fbEx.Message}", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error inesperado: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void LimpiarCampos()
    {
        NombreClienteLabel.Text = "Nombre: ";
        ClientePicker.SelectedItem = null;
        MetodoPagoPicker.SelectedItem = null;
        TotalVentaEntry.Text = string.Empty;
        CompletadoSwitch.IsToggled = false;
    }
}