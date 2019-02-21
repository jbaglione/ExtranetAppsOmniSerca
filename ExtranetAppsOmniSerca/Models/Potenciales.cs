using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ExtranetAppsOmniSerca.Models
{
    public class GuardiaPotencial
    {
        public int PotencialID { get; set; }
        public Filtrable Modalidad { get; set; }
        public string ModalidadDescripcion { get { return Modalidad.ToString(); } }
        public Filtrable Medico { get; set; }
        public string MedicoDescripcion { get { return Medico.ToString(); } }
        //public string Dia
        //{
        //    get {
        //            int indiceSemanal = DiaSemana.Id == 7 ? 0 : DiaSemana.Id;
        //            return DateTimeFormatInfo.CurrentInfo.DayNames[indiceSemanal].ToUpper();
        //        }
        //}
        public Filtrable Dia { get; set; }
        public string DiaDescripcion { get { return Dia.ToString(); } }

        public Filtrable Zona { get; set; }
        public string ZonaDescripcion { get { return Zona.ToString(); } }
        public string Localidad { get; set; }
        public string HoraEntrada { get; set; }
        public string HoraSalida { get; set; }
        public string FechaCarga { get; set; }
    }

    public class Filtrable
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }

        public override string ToString()
        {
            return Descripcion;
        }
    }
}