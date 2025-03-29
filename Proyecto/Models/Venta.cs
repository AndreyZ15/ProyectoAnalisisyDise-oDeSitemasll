using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Models
{
    internal class Venta
    {

        public int IDVenta { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string IDCliente { get; set; } 
        public string MetodoPago { get; set; }
        public decimal Total { get; set; }


        public Cliente Cliente { get; set; }
        public List<DetalleVenta> Detalles { get; set; } = new List<DetalleVenta>();


    }
}
 