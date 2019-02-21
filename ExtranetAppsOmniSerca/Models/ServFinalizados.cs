using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using ExtranetAppsOmniSerca.Helpers;

namespace ExtranetAppsOmniSerca.Models
{
    public class ServFinalizados
    {
        public long ID { get; set; }
        public string FecIncidente { get; set; }
        public string NroIncidente { get; set; }
        public string ClienteID { get; set; }
        public string Llamada { get; set; }
        public string IntegranteID { get; set; }
        public string Nombre { get; set; }
        public string Domicilio { get; set; }
        public string Localidad { get; set; }
        public string Sintoma { get; set; }
        public string Grado { get; set; }
        public string Arribo { get; set; }
        public string Diagnostico { get; set; }
        public string Deriva { get; set; }
        public string Final { get; set; }
        public string NroInterno { get; set; }

        public void dataRowToServFinalizados(DataRow r)
        {
            Helpers.Helpers hlp = new Helpers.Helpers();
            
            ID = Convert.ToInt32(r["ID"]);
            FecIncidente = hlp.formatDate(r["FecIncidente"].ToString());
            NroIncidente = r["NroIncidente"].ToString();
            Llamada = r["Llamada"].ToString();
            IntegranteID = r["IntegranteId"].ToString();
            Nombre = r["Nombre"].ToString();
            Domicilio = r["dm_virDomicilio"].ToString();
            Localidad = r["Localidad"].ToString();
            Sintoma = r["Sintoma"].ToString();
            Grado = r["virFACConceptoId"].ToString();
            Arribo = r["Arribo"].ToString();
            Diagnostico = r["Diagnostico"].ToString();
            Deriva = r["Deriva"].ToString();
            Final = r["Final"].ToString();
            NroInterno = r["NroInterno"].ToString();
   
        }


    }



    
}