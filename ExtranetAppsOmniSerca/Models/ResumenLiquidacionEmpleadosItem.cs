using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ExtranetAppsOmniSerca.Models
{
    public class ResumenLiquidacionEmpleadosItem
    {
        public int GrupoId { get; set; }
        public string Descripcion { get; set; }
        public string Valor { get; set; }

        public ResumenLiquidacionEmpleadosItem(DataRow dr)
        {
            this.GrupoId = Convert.ToInt32(dr["Grupo"].ToString());
            this.Descripcion = dr["Descripcion"].ToString();
            this.Valor = dr["Valor"].ToString();
        }
    }
}