using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using Proyecto.Models;
using System.Collections.ObjectModel;

namespace Proyecto.Services
{
    public class FireBaseService
    {
        private readonly FirebaseClient _firebaseClient;

        public FireBaseService(string apiKey)
        {
            // Reemplaza esta URL con la URL de tu proyecto Firebase
            _firebaseClient = new FirebaseClient(apiKey);
        }

        #region Clientes

        public async Task<List<Cliente>> GetAllClientesAsync()
        {
            var clientes = await _firebaseClient
                .Child("clientes")
                .OnceAsync<Cliente>();

            return clientes.Select(item => new Cliente
            {
                IDCliente = item.Key,
                Nombre = item.Object.Nombre,
                Apellido = item.Object.Apellido,
                Telefono = item.Object.Telefono,
                Correo = item.Object.Correo
            }).ToList();
        }

        public async Task<Cliente> GetClienteAsync(string idCliente)
        {
            var cliente = await _firebaseClient
                .Child("clientes")
                .Child(idCliente)
                .OnceSingleAsync<Cliente>();

            return cliente;
        }

        public async Task<string> AddClienteAsync(Cliente cliente)
        {
            if (string.IsNullOrEmpty(cliente.IDCliente))
            {
                cliente.IDCliente = Guid.NewGuid().ToString();
            }

            await _firebaseClient
                .Child("clientes")
                .Child(cliente.IDCliente)
                .PutAsync(cliente);

            return cliente.IDCliente;
        }

        public async Task UpdateClienteAsync(Cliente cliente)
        {
            await _firebaseClient
                .Child("clientes")
                .Child(cliente.IDCliente)
                .PutAsync(cliente);
        }

        public async Task DeleteClienteAsync(string idCliente)
        {
            await _firebaseClient
                .Child("clientes")
                .Child(idCliente)
                .DeleteAsync();
        }

        #endregion

        #region Productos

        public async Task<List<Producto>> GetAllProductosAsync()
        {
            var productos = await _firebaseClient
                .Child("productos")
                .OnceAsync<Producto>();

            return productos.Select(item => new Producto
            {
                IDProducto = item.Key,
                Nombre = item.Object.Nombre,
                Color = item.Object.Color,
                Marca = item.Object.Marca,
                Talla = item.Object.Talla,
                Detalle = item.Object.Detalle,
                Precio = item.Object.Precio
            }).ToList();
        }

        public async Task<Producto> GetProductoAsync(string idProducto)
        {
            var producto = await _firebaseClient
                .Child("productos")
                .Child(idProducto)
                .OnceSingleAsync<Producto>();

            return producto;
        }

        public async Task<string> AddProductoAsync(Producto producto)
        {
            if (string.IsNullOrEmpty(producto.IDProducto))
            {
                producto.IDProducto = Guid.NewGuid().ToString();
            }

            await _firebaseClient
                .Child("productos")
                .Child(producto.IDProducto)
                .PutAsync(producto);

            return producto.IDProducto;
        }

        public async Task UpdateProductoAsync(Producto producto)
        {
            await _firebaseClient
                .Child("productos")
                .Child(producto.IDProducto)
                .PutAsync(producto);
        }

        public async Task DeleteProductoAsync(string idProducto)
        {
            await _firebaseClient
                .Child("productos")
                .Child(idProducto)
                .DeleteAsync();
        }

        #endregion

        #region Apartados

        public async Task<List<Apartado>> GetAllApartadosAsync()
        {
            var apartados = await _firebaseClient
                .Child("apartados")
                .OnceAsync<Apartado>();

            return apartados.Select(item =>
            {
                var apartado = item.Object;
                apartado.IDApartado = int.Parse(item.Key);
                return apartado;
            }).ToList();
        }

        public async Task<Apartado> GetApartadoAsync(int idApartado)
        {
            var apartado = await _firebaseClient
                .Child("apartados")
                .Child(idApartado.ToString())
                .OnceSingleAsync<Apartado>();

            apartado.IDApartado = idApartado;
            return apartado;
        }

        public async Task<int> AddApartadoAsync(Apartado apartado)
        {
            // Obtener el siguiente ID para el apartado
            var apartados = await GetAllApartadosAsync();
            int nextId = apartados.Count > 0 ? apartados.Max(a => a.IDApartado) + 1 : 1;

            apartado.IDApartado = nextId;
            apartado.Fecha = DateTime.Now;

            await _firebaseClient
                .Child("apartados")
                .Child(nextId.ToString())
                .PutAsync(apartado);

            return nextId;
        }

        public async Task UpdateApartadoAsync(Apartado apartado)
        {
            await _firebaseClient
                .Child("apartados")
                .Child(apartado.IDApartado.ToString())
                .PutAsync(apartado);
        }

        public async Task DeleteApartadoAsync(int idApartado)
        {
            await _firebaseClient
                .Child("apartados")
                .Child(idApartado.ToString())
                .DeleteAsync();
        }

        #endregion

        #region Ventas

        public async Task<List<Venta>> GetAllVentasAsync()
        {
            var ventas = await _firebaseClient
                .Child("ventas")
                .OnceAsync<Venta>();

            return ventas.Select(item =>
            {
                var venta = item.Object;
                venta.IDVenta = int.Parse(item.Key);
                return venta;
            }).ToList();
        }

        public async Task<Venta> GetVentaAsync(int idVenta)
        {
            var venta = await _firebaseClient
                .Child("ventas")
                .Child(idVenta.ToString())
                .OnceSingleAsync<Venta>();

            venta.IDVenta = idVenta;
            return venta;
        }

        public async Task<int> AddVentaAsync(Venta venta)
        {
            // Obtener el siguiente ID para la venta
            var ventas = await GetAllVentasAsync();
            int nextId = ventas.Count > 0 ? ventas.Max(v => v.IDVenta) + 1 : 1;

            venta.IDVenta = nextId;
            venta.Fecha = DateTime.Now;

            await _firebaseClient
                .Child("ventas")
                .Child(nextId.ToString())
                .PutAsync(venta);

            return nextId;
        }

        public async Task UpdateVentaAsync(Venta venta)
        {
            await _firebaseClient
                .Child("ventas")
                .Child(venta.IDVenta.ToString())
                .PutAsync(venta);
        }

        public async Task DeleteVentaAsync(int idVenta)
        {
            await _firebaseClient
                .Child("ventas")
                .Child(idVenta.ToString())
                .DeleteAsync();
        }

        #endregion

        #region DetalleApartados

        public async Task<List<DetalleApartado>> GetDetallesApartadoAsync(int idApartado)
        {
            var detalles = await _firebaseClient
                .Child("detalleApartados")
                .OrderBy("IDApartado")
                .EqualTo(idApartado)
                .OnceAsync<DetalleApartado>();

            return detalles.Select(item =>
            {
                var detalle = item.Object;
                detalle.IDDetalleApartado = int.Parse(item.Key);
                return detalle;
            }).ToList();
        }

        public async Task<int> AddDetalleApartadoAsync(DetalleApartado detalle)
        {
            // Obtener todos los detalles para conseguir el siguiente ID
            var todosDetalles = await _firebaseClient
                .Child("detalleApartados")
                .OnceAsync<DetalleApartado>();

            int nextId = todosDetalles.Count > 0 ?
                         todosDetalles.Max(d => int.Parse(d.Key)) + 1 : 1;

            detalle.IDDetalleApartado = nextId;

            await _firebaseClient
                .Child("detalleApartados")
                .Child(nextId.ToString())
                .PutAsync(detalle);

            return nextId;
        }

        #endregion

        #region DetalleVentas

        public async Task<List<DetalleVenta>> GetDetallesVentaAsync(int idVenta)
        {
            var detalles = await _firebaseClient
                .Child("detalleVentas")
                .OrderBy("IDVenta")
                .EqualTo(idVenta)
                .OnceAsync<DetalleVenta>();

            return detalles.Select(item =>
            {
                var detalle = item.Object;
                detalle.IDDetalleVenta = int.Parse(item.Key);
                return detalle;
            }).ToList();
        }

        public async Task<int> AddDetalleVentaAsync(DetalleVenta detalle)
        {
            // Obtener todos los detalles para conseguir el siguiente ID
            var todosDetalles = await _firebaseClient
                .Child("detalleVentas")
                .OnceAsync<DetalleVenta>();

            int nextId = todosDetalles.Count > 0 ?
                         todosDetalles.Max(d => int.Parse(d.Key)) + 1 : 1;

            detalle.IDDetalleVenta = nextId;

            await _firebaseClient
                .Child("detalleVentas")
                .Child(nextId.ToString())
                .PutAsync(detalle);

            return nextId;
        }

        #endregion

        #region Exclusividades

        public async Task<List<Exclusividad>> GetExclusividadesProductoAsync(string idProducto)
        {
            var exclusividades = await _firebaseClient
                .Child("exclusividades")
                .OrderBy("IDProducto")
                .EqualTo(idProducto)
                .OnceAsync<Exclusividad>();

            return exclusividades.Select(item =>
            {
                var exclusividad = item.Object;
                exclusividad.IDExclusividad = int.Parse(item.Key);
                return exclusividad;
            }).ToList();
        }

        public async Task<int> AddExclusividadAsync(Exclusividad exclusividad)
        {
            // Obtener todas las exclusividades para conseguir el siguiente ID
            var todasExclusividades = await _firebaseClient
                .Child("exclusividades")
                .OnceAsync<Exclusividad>();

            int nextId = todasExclusividades.Count > 0 ?
                         todasExclusividades.Max(e => int.Parse(e.Key)) + 1 : 1;

            exclusividad.IDExclusividad = nextId;

            await _firebaseClient
                .Child("exclusividades")
                .Child(nextId.ToString())
                .PutAsync(exclusividad);

            return nextId;
        }

        #endregion
    }
}