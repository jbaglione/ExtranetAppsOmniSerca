using System;
using System.Data;
using System.Globalization;

namespace ExtranetAppsOmniSerca.Models
{
    public class ResumenItem
    {
        public int GrupoId { get; set; }
        public string Descripcion { get; set; }
        public decimal Importe { get; set; }
        public int Link { get; set; }
        public long LiquidacionMovilId { get; set; }
        public int flgStatus { get; set; }

        public ResumenItem(DataRow dr)
        {
            this.GrupoId = Convert.ToInt32(dr["GrupoId"].ToString());
            this.Descripcion = dr["Descripcion"].ToString();
            this.Importe = Convert.ToDecimal(dr["Importe"].ToString(), new CultureInfo("en-US"));
            this.Link = Convert.ToInt32(dr["Link"].ToString());
            this.LiquidacionMovilId = Convert.ToInt64(dr["LiquidacionMovilId"].ToString());
            this.flgStatus = Convert.ToInt32(dr["flgStatus"].ToString());
        }

    }
}