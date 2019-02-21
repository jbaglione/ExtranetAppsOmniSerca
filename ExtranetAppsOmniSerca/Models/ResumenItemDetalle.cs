using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Globalization;

namespace ExtranetAppsOmniSerca.Models
{
    public class ResumenItemDetalle
    {
        public string Descripcion { get; set; }
        public decimal Importe { get; set; }
        public string Referencia { get; set; }
        public long? Cantidad { get; set; }
        public ResumenItemDetalle(DataRow dr)
        {
            Descripcion = dr["Descripcion"].ToString();
            Importe = Convert.ToDecimal(dr["Importe"].ToString());
            if (dr.Table.Columns.Contains("Referencia"))
                Referencia = dr["Referencia"].ToString();
            if (dr.Table.Columns.Contains("Cantidad"))
                Cantidad = Convert.ToInt64(dr["Cantidad"].ToString());
        }
    }
}