using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace ExtranetAppsOmniSerca.Models
{
    public class IncidenteCalculo
    {
        public string SubConcepto { get; set; }
        public int Cantidad { get; set; }
        public decimal Importe { get; set; }

        public IncidenteCalculo(DataRow dr)
        {
            SubConcepto = dr["SubConcepto"].ToString();
            Cantidad = Convert.ToInt32(dr["Cantidad"].ToString());
            Importe = Convert.ToDecimal(dr["Importe"].ToString());
        }
    }
}