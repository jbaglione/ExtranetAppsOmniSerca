using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExtranetAppsOmniSerca.ViewModels
{
    public class Horario
    {
		public int DiaNumero { get; set; }
		public string DiaDeLaSemana { get; set; }
        public string Entrada1 { get; set; }
        public string Salida1 { get; set; }
        public string Movil1 { get; set; }
        public string Entrada2 { get; set; }
        public string Salida2 { get; set; }
		public string Movil2 { get; set; }
		public Byte Disponibilidad { get; set; }
    }
}