using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace appSCISA.Models
{
    public class Producto
    {
        public int ProductoID { get; set; }
        public string TipoProducto { get; set; }
        public string NombreProducto { get; set; }
        public string EstadoProducto { get; set; }
        public DateTime FechaIngreso { get; set; }
        public decimal ValorCalculado { get; set; }
        public TimeSpan TiempoVencimientoDevolucion { get; set; }
    }
}
