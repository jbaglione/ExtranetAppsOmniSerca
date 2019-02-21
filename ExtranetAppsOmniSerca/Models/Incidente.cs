using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace ExtranetAppsOmniSerca.Models
{
    public class Incidente
    {
        public long IncidenteId { get; set; }
        public string ID { get; set; }
        public string FecIncidente { get; set; }
        public string NroIncidente { get; set; }
        public string Iva { get; set; }
        public string IIBB { get; set; }
        public string Nombre { get; set; }
        public string LocalidadDesde { get; set; }
        public string LocalidadHasta { get; set; }
        public string Kilometros { get; set; }
        public string Retorno { get; set; }
        public string Turno { get; set; }
        public string TpoEspera { get; set; }
        public string ConceptoFacturacionId { get; set; }
        public decimal CoPago { get; set; }
        public string Deriva { get; set; }
        public decimal Importe { get; set; }
        public string Rem { get; set; }
        public long Conf { get; set; }
        public string Rev { get; set; }
        public string ArchivoOrden { get; set; }

        public Incidente(DataRow dr)
        {
            IncidenteId = Convert.ToInt64(dr["IncidenteId"]);
            ID = dr["ID"].ToString();
            FecIncidente = dr["FecIncidente"].ToString();
            NroIncidente = dr["NroIncidente"].ToString();
            Iva = dr["Iva"].ToString();

            IIBB = dr["IIBB"].ToString();
            Nombre = dr["Nombre"].ToString();
            LocalidadDesde = dr["LocalidadDesde"].ToString();
            LocalidadHasta = dr["LocalidadHasta"].ToString();
            Kilometros = dr["Kilometros"].ToString();

            Retorno = dr["Retorno"].ToString();
            Turno = dr["Turno"].ToString();
            TpoEspera = dr["TpoEspera"].ToString();
            ConceptoFacturacionId = dr["ConceptoFacturacionId"].ToString();
            CoPago = (decimal)dr["CoPago"];

            Deriva = dr["Deriva"].ToString();
            Importe = (decimal)dr["Importe"];
            Rem = dr["Rem"].ToString();
            Conf = Convert.ToInt64(dr["Conf"]);
            Rev = dr["Rev"].ToString();
            ArchivoOrden = dr["ArchivoOrden"].ToString();
        }
    }
}