using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace ExtranetAppsOmniSerca.Models
{
    public class MotivoReclamo
    {
        public string ID { get; set; }
        public string Descripcion { get; set; }
        public int flgCoPago { get; set; }

        public MotivoReclamo()
        { }

        public MotivoReclamo(DataRow dr)
        {
            this.ID = dr["ID"].ToString();
            this.Descripcion = dr["Descripcion"].ToString();
            this.flgCoPago = dr.Table.Columns.Contains("flgCoPago")? Convert.ToInt32(dr["flgCoPago"].ToString()) : 0;
        }
    }
}