using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Models
{
    public class Encargo
    {
        public int IDEncargo { get; set; }
        public string IDProducto { get; set; }
        public string NombreProducto { get; set; }
        public string MarcaProducto { get; set; }
        public string ColorProducto { get; set; }
        public string TallaProdcuto { get; set; }
        public string DetalleProducto { get; set; }
        public string NombreCliente { get; set; } // cuando se selecciona el cliente que se llenen los demas campos, (apellido y telefono)
        public string ApellidoCliente { get; set; }
        public string TelefonoCliente { get; set; }

        // Relaciones
        public Producto Producto { get; set; }


    }
}
