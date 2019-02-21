using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExtranetAppsOmniSerca.Models
{
    public class GuardiaPotencialViewModel
    {
        public List<GuardiaPotencial> GuardiasPotenciales { get; set; }
        public List<Filtrable> Modalidades { get; set; }
        public List<Filtrable> Medicos { get; set; }
        public List<Filtrable> Dias { get; set; }
        public List<Filtrable> Zonas { get; set; }
    }
}