using Proyecto.Models;
//using Proyecto.ViewModels;
using System;

namespace Proyecto.Views;


    public partial class ClientesPage : ContentPage
    {
        public ClientesPage()
        {
            InitializeComponent();
        }

        private async void OnAgregarClienteClicked(object sender, EventArgs e)
        {
            string nombre = NombreClienteEntry.Text;
            string apellido = ApellidoClienteEntry.Text;
            string correo = CorreoClienteEntry.Text;
            string telefono = TelefonoClienteEntry.Text;

            // Validaci�n
            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(apellido) || string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(telefono))
            {
                await DisplayAlert("Error", "Todos los campos son obligatorios", "OK");
                return;
            }

            // Crear el objeto Cliente
            Cliente nuevoCliente = new Cliente
            {
                Nombre = nombre,
                Apellido = apellido,
                Correo = correo,
                Telefono = telefono,
                IDCliente = Guid.NewGuid().ToString()  // Puedes usar un ID �nico para cada cliente
            };

            // Aqu� puedes agregar la l�gica para guardar este cliente en la base de datos (por ejemplo, Firebase)
            // Por ejemplo, llamar a un m�todo del ViewModel o un servicio de Firebase

            // Limpiar los campos despu�s de agregar el cliente
            NombreClienteEntry.Text = string.Empty;
            ApellidoClienteEntry.Text = string.Empty;
            CorreoClienteEntry.Text = string.Empty;
            TelefonoClienteEntry.Text = string.Empty;

            await DisplayAlert("�xito", "Cliente agregado correctamente", "OK");
        }
    }

