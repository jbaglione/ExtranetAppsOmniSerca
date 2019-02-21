using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace ExtranetAppsOmniSerca.Models
{
    public class Rubro
    {
        public int ID { get; set; }
        public string Descripcion { get; set; }
        public Rubro()
        {
        }
        public Rubro(DataRow dr, string id = "ID", string descripcion = "Descripcion")
        {
            ID = Convert.ToInt32(dr[id]);
            Descripcion = dr[descripcion].ToString();
        }

        //public void dataRowToObject(DataRow dr)
        //{
        //    this.ID = Convert.ToInt32(dr["ID"]);
        //    this.Descripcion = dr["Descripcion"].ToString();
        //}
    }
}