using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Models
{
    internal class Apartado
    {

        public int  IDApartado { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string IDCliente { get; set; }
        public decimal Abono    { get; set; }
        public decimal Total { get; set; }
        public bool Completado  { get; set; } = false;


        //Relaciones

        public Cliente Cliente { get; set; }
        public  List<DetalleApartado> Detalles  { get; set; }  = new List<DetalleApartado>();

    }
}
