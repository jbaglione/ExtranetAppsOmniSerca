using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ExtranetAppsOmniSerca.Models
{
    public class OperativaEnCurso
    {
        public int ID { get; set; }
        public string NroInc { get; set; }
        public string ViajeID { get; set; }
        public string ClienteID { get; set; }
        public string NroInterno { get; set; }
        public string Llamada { get; set; }
        public string Nombre { get; set; }
        public string Domicilio { get; set; }
        public string Localidad { get; set; }
        public string Grado { get; set; }
        public string WebMovil { get; set; }
        public string Arribo { get; set; }
        public string WebEstado { get; set; }
        public string Respuesta { get; set; }
        public string Reclamo { get; set; }

        public void dataRowToOperativaEnCurso(DataRow r)
        {
            Arribo = r["Arribo"].ToString();
            ClienteID = r["ClienteID"].ToString();
            Domicilio = r["dm_virDomicilio"].ToString();
            Grado = r["virFACConceptoId"].ToString();
            ID = Convert.ToInt32(r["ID"]);
            Llamada = r["Llamada"].ToString();
            Localidad = r["Loc"].ToString();
            Nombre = r["Nombre"].ToString();
            NroInc = r["NroIncidente"].ToString();
            NroInterno = r["NroInterno"].ToString();
            Reclamo = r["Rcl"].ToString();
            Respuesta = r["Rta"].ToString();
            ViajeID = r["ViajeId"].ToString();
            WebEstado = r["virWebEstado"].ToString();
            WebMovil = r["virWebMovil"].ToString();

        }
    }


}