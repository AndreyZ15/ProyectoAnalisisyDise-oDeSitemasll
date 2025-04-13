using Proyecto.Models;
using Proyecto.Services;
using System;
using System.Collections.ObjectModel;

namespace Proyecto.Views;

public partial class ApartadosPage : ContentPage
{
    private readonly FireBaseService _firebaseService;
    public ObservableCollection<Apartado> Apartados { get; set; } = new ObservableCollection<Apartado>();
    public ObservableCollection<Cliente> Clientes { get; set; } = new ObservableCollection<Cliente>();
    public ApartadosPage()
    {
        InitializeComponent();
        _firebaseService = Application.Current.Handler.MauiContext.Services.GetService<FireBaseService>();
        ClientePicker.ItemsSource = Clientes;

        CargarClientes();
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
    private async void OnAgregarApartadoClicked(object sender, EventArgs e)
    {
        // Obtener el cliente seleccionado del Picker
        if (ClientePicker.SelectedItem is not Cliente clienteSeleccionado)
        {
            await DisplayAlert("Error", "Por favor, seleccione un cliente.", "OK");
            return;
        }

        // Validar los valores de abono y total
        bool esAbonoValido = decimal.TryParse(AbonoEntry.Text, out decimal abono);
        bool esTotalValido = decimal.TryParse(TotalEntry.Text, out decimal total);
        bool completado = CompletadoSwitch.IsToggled;

        // Validación de los datos ingresados
        if (!esAbonoValido || !esTotalValido)
        {
            await DisplayAlert("Error", "Por favor, complete todos los campos correctamente.", "OK");
            return;
        }

        try
        {
            // Validar que el abono no sea mayor que el total
            if (abono > total)
            {
                await DisplayAlert("Error", "El abono no puede ser mayor que el total.", "OK");
                return;
            }

            // Creación del Apartado
            Apartado nuevoApartado = new Apartado
            {
                IDCliente = clienteSeleccionado.IDCliente,
                Abono = abono,
                Total = total,
                Completado = completado,
                Fecha = DateTime.Now
            };

            // Guardar en Firebase
            int idApartado = await _firebaseService.AddApartadoAsync(nuevoApartado);

            // Actualizar la colección local
            nuevoApartado.IDApartado = idApartado;
            Apartados.Add(nuevoApartado);

            // Limpiar los campos después de agregar el apartado
            ClientePicker.SelectedItem = null;
            AbonoEntry.Text = string.Empty;
            TotalEntry.Text = string.Empty;
            CompletadoSwitch.IsToggled = false;

            await DisplayAlert("Éxito", "Apartado agregado correctamente", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudo guardar el apartado: {ex.Message}", "OK");
        }
    }

}