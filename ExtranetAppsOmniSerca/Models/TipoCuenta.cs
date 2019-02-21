using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace ExtranetAppsOmniSerca.Models
{
    public class TipoCuenta
    {
        public long ID { get; set; }
        public string AbreviaturaId { get; set; }
        public string Descripcion { get; set; }
        public TipoCuenta()
        {
        }
        public TipoCuenta(DataRow dr)
        {
            ID = Convert.ToInt32(dr["ID"]);
            AbreviaturaId = dr["AbreviaturaId"].ToString();
            Descripcion = dr["Descripcion"].ToString();
        }
    }
}