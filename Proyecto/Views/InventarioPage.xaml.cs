using Proyecto.Models;
using System;
using System.Collections.ObjectModel;

namespace Proyecto.Views;

    public partial class InventarioPage : ContentPage
    {
        public ObservableCollection<Producto> Productos { get; set; } = new ObservableCollection<Producto>();

        public InventarioPage()
        {
            InitializeComponent();
            ProductosCollectionView.ItemsSource = Productos;
        }

        private async void OnAgregarProductoClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(IDProductoEntry.Text) ||
                string.IsNullOrWhiteSpace(NombreEntry.Text) ||
                string.IsNullOrWhiteSpace(MarcaEntry.Text) ||
                string.IsNullOrWhiteSpace(ColorEntry.Text) ||
                string.IsNullOrWhiteSpace(TallaEntry.Text) ||
                string.IsNullOrWhiteSpace(DetalleEntry.Text) ||
                !decimal.TryParse(PrecioEntry.Text, out decimal precio))
            {
                await DisplayAlert("Error", "Complete todos los campos correctamente.", "OK");
                return;
            }

            var nuevoProducto = new Producto
            {
                IDProducto = IDProductoEntry.Text,
                Nombre = NombreEntry.Text,
                Marca = MarcaEntry.Text,
                Color = ColorEntry.Text,
                Talla = TallaEntry.Text,
                Detalle = DetalleEntry.Text,
                Precio = precio
            };

            Productos.Add(nuevoProducto);
            LimpiarCampos();
        }

        private async void OnEditarProductoClicked(object sender, EventArgs e)
        {
            if (ProductosCollectionView.SelectedItem is Producto productoSeleccionado)
            {
                productoSeleccionado.Nombre = NombreEntry.Text;
                productoSeleccionado.Marca = MarcaEntry.Text;
                productoSeleccionado.Color = ColorEntry.Text;
                productoSeleccionado.Talla = TallaEntry.Text;
                productoSeleccionado.Detalle = DetalleEntry.Text;

                if (decimal.TryParse(PrecioEntry.Text, out decimal precio))
                {
                    productoSeleccionado.Precio = precio;
                }

                ProductosCollectionView.ItemsSource = null;
                ProductosCollectionView.ItemsSource = Productos;

                LimpiarCampos();
                await DisplayAlert("Éxito", "Producto actualizado correctamente.", "OK");
            }
            else
            {
                await DisplayAlert("Error", "Seleccione un producto para editar.", "OK");
            }
        }

        private async void OnEliminarProductoClicked(object sender, EventArgs e)
        {
            if (ProductosCollectionView.SelectedItem is Producto productoSeleccionado)
            {
                Productos.Remove(productoSeleccionado);
                LimpiarCampos();
                await DisplayAlert("Éxito", "Producto eliminado.", "OK");
            }
            else
            {
                await DisplayAlert("Error", "Seleccione un producto para eliminar.", "OK");
            }
        }

        private void OnActualizarProductosClicked(object sender, EventArgs e)
        {
            ProductosCollectionView.ItemsSource = null;
            ProductosCollectionView.ItemsSource = Productos;
        }

        private void LimpiarCampos()
        {
            IDProductoEntry.Text = string.Empty;
            NombreEntry.Text = string.Empty;
            ColorEntry.Text = string.Empty;
            MarcaEntry.Text = string.Empty;
            TallaEntry.Text = string.Empty;
            DetalleEntry.Text = string.Empty;
            PrecioEntry.Text = string.Empty;
        }
    }

