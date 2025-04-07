using Proyecto.Models;
using Proyecto.Services;
using System;
using System.Collections.ObjectModel;

namespace Proyecto.Views;

public partial class ApartadosPage : ContentPage
{
    private readonly FireBaseService _firebaseService;
    public ObservableCollection<Apartado> Apartados { get; set; } = new ObservableCollection<Apartado>();

    public ApartadosPage()
    {
        InitializeComponent();
        _firebaseService = Application.Current.Handler.MauiContext.Services.GetService<FireBaseService>();
    }

    private async void OnAgregarApartadoClicked(object sender, EventArgs e)
    {
        string idCliente = IDClienteEntry.Text;
        bool esAbonoValido = decimal.TryParse(AbonoEntry.Text, out decimal abono);
        bool esTotalValido = decimal.TryParse(TotalEntry.Text, out decimal total);
        bool completado = CompletadoSwitch.IsToggled;

        // Validación de los datos ingresados
        if (string.IsNullOrEmpty(idCliente) || !esAbonoValido || !esTotalValido)
        {
            await DisplayAlert("Error", "Por favor, complete todos los campos correctamente.", "OK");
            return;
        }

        try
        {
            // Validar que el cliente exista
            var cliente = await _firebaseService.GetClienteAsync(idCliente);
            if (cliente == null)
            {
                await DisplayAlert("Error", "El cliente especificado no existe.", "OK");
                return;
            }

            // Validar que el abono no sea mayor que el total
            if (abono > total)
            {
                await DisplayAlert("Error", "El abono no puede ser mayor que el total.", "OK");
                return;
            }

            // Creación del Apartado
            Apartado nuevoApartado = new Apartado
            {
                IDCliente = idCliente,
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
            IDClienteEntry.Text = string.Empty;
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