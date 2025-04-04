using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Models
{
    public class DetalleApartado
    {

        public int IDDetalleApartado { get; set; }  
        public int IDApartado { get; set; }   
        public string IDProducto { get; set; }
        public int  Cantidad { get; set; }   
        public decimal PrecioUnitario { get; set; } 
        public decimal SubTotal { get; set; }


        //Relaciones

        public Apartado Apartado { get; set; }  
        public Producto Producto { get; set; }






    }
}
