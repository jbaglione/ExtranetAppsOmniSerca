using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExtranetAppsOmniSerca.Models
{
    public class EstadoReclamoGuardia
    {
        public int Cerrado { get; set; }
        public int Conforme { get; set; }
        public string MotivoId { get; set; }
        public string Entrada { get; set; }
        public string Salida { get; set; }
        public string Reclamo { get; set; }
        public string Respuesta { get; set; }
        public int Estado { get; set; }
        public string GuardiaID { get; set; }
    }
}