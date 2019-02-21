using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ExtranetAppsOmniSerca.Models
{
    public class FtrCliente
    {
        public int ID { get; set; }
        public string Codigo { get; set; }

        public void dataRowToFtrcliente(DataRow r)
        {
            ID = Convert.ToInt32(r["ID"]);
            Codigo = r["Codigo"].ToString();
        }
    }
}