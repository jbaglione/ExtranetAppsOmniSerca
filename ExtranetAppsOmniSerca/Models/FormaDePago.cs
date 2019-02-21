using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace ExtranetAppsOmniSerca.Models
{
    public class FormaDePago
    {
        public int ID { get; set; }
        public string Descripcion { get; set; }
        public FormaDePago()
        {
        }
        public FormaDePago(DataRow dr, string descripcion = "Descripcion", string id = "ID")
        {
            if (dr.Table.Columns.Contains(descripcion))
                Descripcion = dr[descripcion].ToString();

            if (dr.Table.Columns.Contains(id))
                ID = dr[id].ToString() == "" ? 0 : Convert.ToInt32(dr[id]);
            else if (dr.Table.Columns.Contains("FormaPagoId"))
                ID = dr["FormaPagoID"].ToString() == "" ? 1 : Convert.ToInt32(dr["FormaPagoID"]);//1 es efectivo.
        }
    }
}