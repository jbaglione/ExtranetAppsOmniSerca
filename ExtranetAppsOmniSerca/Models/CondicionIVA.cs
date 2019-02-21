using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace ExtranetAppsOmniSerca.Models
{
    public class CondicionIVA
    {
        public string ID { get; set; }
        public string Descripcion { get; set; }
        public CondicionIVA()
        {
        }

        public CondicionIVA(DataRow dr, string id = "ID", string descripcion = "Descripcion")
        {
            ID = dr[id].ToString();
            Descripcion = dr[descripcion].ToString();
        }
        //public void construsctorFake(DataRow dr)
        //{
        //    this.ID = dr["ID"].ToString();
        //    this.Descripcion = dr["Descripcion"].ToString();
        //}
    }
}