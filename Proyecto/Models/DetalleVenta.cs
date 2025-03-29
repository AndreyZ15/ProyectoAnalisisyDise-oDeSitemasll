using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Models
{
    internal class DetalleVenta
    {


        public int IDDetalleVenta { get; set; } 
        public int IDVenta { get; set; }
        public string IDProducto   { get; set; }
        public int Cantidad { get; set; }  
        public decimal PrecioUnitario { get; set; } 
        public decimal SubTotal  { get; set; }


        //Relaciones
        public Venta Venta { get; set; }

        public Producto Producto { get; set; }




        

    }
}
