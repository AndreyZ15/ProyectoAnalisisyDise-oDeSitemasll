using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Models
{
    public class Exclusividad
    {
        public int IDExclusividad { get; set; }
        public string IDProducto { get; set; }
        public string LugarEvento { get; set; }
        public DateTime FechaEvento { get; set; }

        // Relaciones
        public Producto Producto { get; set; }



    }
}
