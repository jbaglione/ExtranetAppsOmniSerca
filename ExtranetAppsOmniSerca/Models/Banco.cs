using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace ExtranetAppsOmniSerca.Models
{
    public class Banco
    {
        public Banco()
        {
        }
        public Banco(DataRow dr, string id = "ID", string descripcion = "Descripcion")
        {
            ID = Convert.ToInt32(dr[id]);
            if (dr.Table.Columns.Contains(descripcion))
                Descripcion = dr[descripcion].ToString();
        }

        public int ID { get; set; }
        public string Descripcion { get; set; }
        
    }
}