using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExtranetAppsOmniSerca.Models
{
    public class Guardia
    {
        public string ID { get; set; }
        public string Periodo { get; set; }
        public string DiaDeLaSemana { get; set; }
        public int Dia { get; set; }
        public string Tarifa { get; set; }
        public string HorarioEntrada { get; set; }
        public int MinutosLlegadaTarde { get; set; }
        public string HorarioSalida { get; set; }
        public int MinutosRetiroAnticipado { get; set; }
        public string Movil { get; set; }
        public string HorasTrabajadas { get; set; }
        public int Rojos { get; set; }
        public int Amarillos { get; set; }
        public int Verdes { get; set; }
        public int TrasladosProgramados { get; set; }
        public double ImpTotalHoras { get; set; }
        public double ImpEspecialidad { get; set; }
        public double ImpPrestacion { get; set; }
        public double ImpPrestacionExcedente { get; set; }
        public double ImpAnticipo { get; set; }
        public double ImpFinal { get; set; }
        public int Estado { get; set; }
        public string FecMovimiento { get; set; }

    }
}