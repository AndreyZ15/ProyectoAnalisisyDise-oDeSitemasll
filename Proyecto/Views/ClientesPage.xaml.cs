using Proyecto.Models;
using Proyecto.Services;
using System;
using System.Collections.ObjectModel;

namespace Proyecto.Views;

public partial class ClientesPage : ContentPage
{
    private readonly FireBaseService _firebaseService;
    public ObservableCollection<Cliente> Clientes { get; set; } = new ObservableCollection<Cliente>();

    public ClientesPage()
    {
        InitializeComponent();
        _firebaseService = Application.Current.Handler.MauiContext.Services.GetService<FireBaseService>();

        // Cargar los clientes al iniciar la p�gina
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

    private async void OnAgregarClienteClicked(object sender, EventArgs e)
    {
        string nombre = NombreClienteEntry.Text;
        string apellido = ApellidoClienteEntry.Text;
        string correo = CorreoClienteEntry.Text;
        string telefono = TelefonoClienteEntry.Text;

        // Validaci�n
        if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(apellido) ||
            string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(telefono))
        {
            await DisplayAlert("Error", "Todos los campos son obligatorios", "OK");
            return;
        }

        if (!nombre.All(char.IsLetter) || !apellido.All(char.IsLetter))
        {
            await DisplayAlert("Error", "El nombre y el apellido deben contener solo letras", "OK");
            return;
        }

        if (!correo.Contains("@") || !correo.Contains("."))
        {
            await DisplayAlert("Error", "El correo electr�nico no es v�lido", "OK");
            return;
        }

        if (!telefono.All(char.IsDigit))
        {
            await DisplayAlert("Error", "El tel�fono debe contener solo n�meros", "OK");
            return;
        }

        try
        {
            // Crear el objeto Cliente
            Cliente nuevoCliente = new Cliente
            {
                Nombre = nombre,
                Apellido = apellido,
                Correo = correo,
                Telefono = telefono,
                IDCliente = Guid.NewGuid().ToString()
            };

            // Guardar en Firebase
            await _firebaseService.AddClienteAsync(nuevoCliente);

            // Actualizar la colecci�n local
            Clientes.Add(nuevoCliente);

            // Limpiar los campos despu�s de agregar el cliente
            LimpiarCampos();

            await DisplayAlert("�xito", "Cliente agregado correctamente", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudo guardar el cliente: {ex.Message}", "OK");
        }
    }

    private void LimpiarCampos()
    {
        NombreClienteEntry.Text = string.Empty;
        ApellidoClienteEntry.Text = string.Empty;
        CorreoClienteEntry.Text = string.Empty;
        TelefonoClienteEntry.Text = string.Empty;
    }
}
