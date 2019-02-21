using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ExtranetAppsOmniSerca.Models
{
    public class ServErroneos
    {
        public long ID { get; set; }
        public string FecIncidente { get; set; }
        public string NroIncidente { get; set; }
        public string ClienteID { get; set; }
        public string NroInterno { get; set; }
        public string NroAfiliado { get; set; }
        public string Nombre { get; set; }
        public string Domicilio { get; set; }
        public string Grado { get; set; }
        public string DescError { get; set; }
        public string FecHorProcesamiento { get; set; }


        public void dataRowToServErroneos(DataRow r)
        {

            Helpers.Helpers hlp = new Helpers.Helpers();

            ID = Convert.ToInt32(r["ID"]);
            FecIncidente = hlp.formatDate(r["FecIncidente"].ToString());
            NroIncidente = r["NroIncidente"].ToString();
            ClienteID = r["ClienteId"].ToString();
            NroInterno = r["NroInterno"].ToString();
            NroAfiliado = r["NroAfiliado"].ToString();
            Nombre = r["Nombre"].ToString();
            Domicilio = r["dm_virDomicilio"].ToString();
            Grado = r["virFACConceptoId"].ToString();
            DescError = r["DescripcionError"].ToString();
            FecHorProcesamiento = hlp.formatDate(r["FecHorProcesamiento"].ToString());

        }

    }
}