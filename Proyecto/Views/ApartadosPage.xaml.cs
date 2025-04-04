using Proyecto.Models;
using System;

namespace Proyecto.Views;


    public partial class ApartadosPage : ContentPage
    {
        public ApartadosPage()
        {
            InitializeComponent();
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

            // Creación del Apartado
            Apartado nuevoApartado = new Apartado
            {
                IDCliente = idCliente,
                Abono = abono,
                Total = total,
                Completado = completado
            };

            // Aquí puedes agregar la lógica para guardar el apartado en la base de datos (Firebase o SQL)
            // Llamar a un servicio o ViewModel para manejar la base de datos

            // Limpiar los campos después de agregar el apartado
            IDClienteEntry.Text = string.Empty;
            AbonoEntry.Text = string.Empty;
            TotalEntry.Text = string.Empty;
            CompletadoSwitch.IsToggled = false;

            await DisplayAlert("Éxito", "Apartado agregado correctamente", "OK");
        }
    }
