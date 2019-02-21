using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ExtranetAppsOmniSerca.Models
{
    public class Denuncia
    {
        public long NroDenuncia { get; set; }
        public string FecHorDenuncia { get; set; }
        public string Receptor { get; set; }
        public string Empresa { get; set; }
        public string CUIT { get; set; }
        public string Empleado { get; set; }
        public string CUIL { get; set; }
        public string TipoAccidente { get; set; }
        public string Denunciante { get; set; }
        public string Telefono { get; set; }
        public string Domicilio { get; set; }
        public string FecHorAccidente { get; set; }
        public string Hecho { get; set; }
        public string Observaciones { get; set; }
        public string Super { get; set; }

        public void dataRowToDenuncia(DataRow r)
        {
            FecHorDenuncia = r["FecHorDenuncia"].ToString();
            Receptor = r["Receptor"].ToString();
            Empresa = r["Empresa"].ToString();
            CUIT = r["CUIT"].ToString();
            Super = r["Super"].ToString();
            Empleado = r["Empleado"].ToString();
            CUIL = r["CUIL"].ToString();
            TipoAccidente = r["TipoAccidente"].ToString();
            Denunciante = r["Denunciante"].ToString();
            Telefono = r["Telefono"].ToString();
            Domicilio = r["Domicilio"].ToString();
            FecHorAccidente = r["FecHorAccidente"].ToString();
            Hecho = r["Hecho"].ToString();
            Observaciones = r["Observaciones"].ToString();
            NroDenuncia = Convert.ToInt64(r["NroDenuncia"]);

        }
    }


}