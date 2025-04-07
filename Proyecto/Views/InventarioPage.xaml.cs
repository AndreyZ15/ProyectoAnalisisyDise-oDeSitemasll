using Proyecto.Models;
using Proyecto.Services;
using System;
using System.Collections.ObjectModel;

namespace Proyecto.Views;

public partial class InventarioPage : ContentPage
{
    private readonly FireBaseService _firebaseService;
    public ObservableCollection<Producto> Productos { get; set; } = new ObservableCollection<Producto>();

    public InventarioPage()
    {
        InitializeComponent();
        _firebaseService = Application.Current.Handler.MauiContext.Services.GetService<FireBaseService>();

        ProductosCollectionView.ItemsSource = Productos;
        ProductosCollectionView.SelectionChanged += OnProductoSelected;

        // Cargar productos al iniciar
        CargarProductos();
    }

    private async void CargarProductos()
    {
        try
        {
            var productos = await _firebaseService.GetAllProductosAsync();
            Productos.Clear();
            foreach (var producto in productos)
            {
                Productos.Add(producto);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudieron cargar los productos: {ex.Message}", "OK");
        }
    }

    private void OnProductoSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Producto productoSeleccionado)
        {
            // Llenar los campos con la información del producto seleccionado
            IDProductoEntry.Text = productoSeleccionado.IDProducto;
            NombreEntry.Text = productoSeleccionado.Nombre;
            ColorEntry.Text = productoSeleccionado.Color;
            MarcaEntry.Text = productoSeleccionado.Marca;
            TallaEntry.Text = productoSeleccionado.Talla;
            DetalleEntry.Text = productoSeleccionado.Detalle;
            PrecioEntry.Text = productoSeleccionado.Precio.ToString();
        }
    }

    private async void OnAgregarProductoClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NombreEntry.Text) ||
            string.IsNullOrWhiteSpace(MarcaEntry.Text) ||
            string.IsNullOrWhiteSpace(ColorEntry.Text) ||
            string.IsNullOrWhiteSpace(TallaEntry.Text) ||
            string.IsNullOrWhiteSpace(DetalleEntry.Text) ||
            !decimal.TryParse(PrecioEntry.Text, out decimal precio))
        {
            await DisplayAlert("Error", "Complete todos los campos correctamente.", "OK");
            return;
        }

        try
        {
            var nuevoProducto = new Producto
            {
                IDProducto = string.IsNullOrEmpty(IDProductoEntry.Text) ?
                             Guid.NewGuid().ToString() : IDProductoEntry.Text,
                Nombre = NombreEntry.Text,
                Marca = MarcaEntry.Text,
                Color = ColorEntry.Text,
                Talla = TallaEntry.Text,
                Detalle = DetalleEntry.Text,
                Precio = precio
            };

            // Guardar en Firebase
            await _firebaseService.AddProductoAsync(nuevoProducto);

            // Actualizar la colección local
            Productos.Add(nuevoProducto);

            LimpiarCampos();
            await DisplayAlert("Éxito", "Producto agregado correctamente.", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudo guardar el producto: {ex.Message}", "OK");
        }
    }

    private async void OnEditarProductoClicked(object sender, EventArgs e)
    {
        if (ProductosCollectionView.SelectedItem is Producto productoSeleccionado)
        {
            if (string.IsNullOrWhiteSpace(NombreEntry.Text) ||
                string.IsNullOrWhiteSpace(MarcaEntry.Text) ||
                string.IsNullOrWhiteSpace(ColorEntry.Text) ||
                string.IsNullOrWhiteSpace(TallaEntry.Text) ||
                string.IsNullOrWhiteSpace(DetalleEntry.Text) ||
                !decimal.TryParse(PrecioEntry.Text, out decimal precio))
            {
                await DisplayAlert("Error", "Complete todos los campos correctamente.", "OK");
                return;
            }

            try
            {
                productoSeleccionado.Nombre = NombreEntry.Text;
                productoSeleccionado.Marca = MarcaEntry.Text;
                productoSeleccionado.Color = ColorEntry.Text;
                productoSeleccionado.Talla = TallaEntry.Text;
                productoSeleccionado.Detalle = DetalleEntry.Text;
                productoSeleccionado.Precio = precio;

                // Actualizar en Firebase
                await _firebaseService.UpdateProductoAsync(productoSeleccionado);

                // Actualizar la UI
                int index = Productos.IndexOf(productoSeleccionado);
                Productos[index] = productoSeleccionado;

                LimpiarCampos();
                await DisplayAlert("Éxito", "Producto actualizado correctamente.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudo actualizar el producto: {ex.Message}", "OK");
            }
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
            bool confirmar = await DisplayAlert("Confirmar",
                "¿Está seguro que desea eliminar este producto?", "Sí", "No");

            if (confirmar)
            {
                try
                {
                    // Eliminar de Firebase
                    await _firebaseService.DeleteProductoAsync(productoSeleccionado.IDProducto);

                    // Actualizar la colección local
                    Productos.Remove(productoSeleccionado);

                    LimpiarCampos();
                    await DisplayAlert("Éxito", "Producto eliminado.", "OK");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"No se pudo eliminar el producto: {ex.Message}", "OK");
                }
            }
        }
        else
        {
            await DisplayAlert("Error", "Seleccione un producto para eliminar.", "OK");
        }
    }

    private void OnActualizarProductosClicked(object sender, EventArgs e)
    {
        CargarProductos();
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
        ProductosCollectionView.SelectedItem = null;
    }
}