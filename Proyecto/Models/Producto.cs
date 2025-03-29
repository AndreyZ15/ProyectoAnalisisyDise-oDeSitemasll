using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Models
{
    internal class Producto
    {


        public string IDProducto { get; set; }
        public string Nombre { get; set; }
        public string Color { get; set; }
        public string Marca { get; set; }
        public  string Talla  { get; set; }
        public string Detalle { get; set; }
        public decimal Precio { get; set; }




        public List<Exclusividad> Exclusividades { get; set; } = new List<Exclusividad>();
        public List<DetalleVenta> DetallesVentas { get; set; } = new List<DetalleVenta>();
        public List<DetalleApartado> DetallesApartados { get; set; } = new List<DetalleApartado>();




    }
}
