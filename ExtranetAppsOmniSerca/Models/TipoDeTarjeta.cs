using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace ExtranetAppsOmniSerca.Models
{
    public class TipoDeTarjeta
    {
        public string ID { get; set; }
        public string Descripcion { get; set; }
        public string MascaraIngreso { get; set; }
        public TipoDeTarjeta()
        {
        }
        public TipoDeTarjeta(DataRow dr)
        {
            ID = dr["ID"].ToString();
            Descripcion = dr["Descripcion"].ToString();
            MascaraIngreso = dr["mascaraingreso"].ToString().Replace(" ","-");
        }

    }
}