using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Globalization;

namespace ExtranetAppsOmniSerca.Models
{
    public class Conformidad
    {
        public DateTime FecIncidente { get; set; }
        public string NroIncidente { get; set; }
        public int ConformidadId { get; set; }
        public int flgConforme { get; set; }
        public int TerLiqMotivoReclamoId { get; set; }
        public decimal Importe { get; set; }
        public decimal virImpLiquidado { get; set; }
        public decimal virImpDiferencia { get; set; }
        public string Observaciones { get; set; }
        public string Respuesta { get; set; }
        public int Cerrado { get; set; }
        public int flgRespuesta { get; set; }

        public Conformidad(DataRow dr)
        {
            //FecIncidente = Convert.ToDateTime(dr["FecIncidente"].ToString());
            FecIncidente = DateTime.ParseExact(dr["FecIncidente"].ToString(), "yyyyMMdd", CultureInfo.InvariantCulture);
            NroIncidente = dr["NroIncidente"].ToString();
            ConformidadId = Convert.ToInt32(dr["ConformidadId"].ToString());
            flgConforme = Convert.ToInt32(dr["flgConforme"].ToString());
            TerLiqMotivoReclamoId = Convert.ToInt32(dr["TerLiqMotivoReclamoId"].ToString());
            Importe = Convert.ToDecimal(dr["Importe"].ToString());
            virImpLiquidado = Convert.ToDecimal(dr["virImpLiquidado"].ToString());
            virImpDiferencia = Convert.ToDecimal(dr["virImpDiferencia"].ToString());
            Observaciones = dr["Observaciones"].ToString();
            Respuesta = dr["Respuesta"].ToString();
            Cerrado = Convert.ToInt32(dr["Cerrado"].ToString());
            flgRespuesta = Convert.ToInt32(dr["flgRespuesta"].ToString());
        }

    }
}