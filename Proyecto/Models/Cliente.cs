using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Models
{
    internal class Cliente
    {

        public string  IDCliente { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }

        //Relaciones 
        public List<Venta> Ventas { get; set; } = new List<Venta>();
        public List<Apartado> Apartados { get; set; } 

        public string NombreCompleto()
        {

            return $" {Nombre} {Apellido} ";

        }
    }
}
