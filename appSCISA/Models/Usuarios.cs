using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace appSCISA.Models
{
    public class Usuarios
    {
        public int UserID { get; set; }
        public string Nombre { get; set; }
        public string CorreoElectronico { get; set; }
        public string Contraseña { get; set; }
        public string Rol { get; set; }

        public string ConfirmarClave { get; set; }
    }
}
