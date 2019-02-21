using ExtranetAppsOmniSerca.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ExtranetAppsOmniSerca.Models
{
    public class Proveedor
    {
        public long UsuarioId { get; set; }
        public string NombreProveedor { get; set; }

        public Proveedor(DataRow dr)
        {
            UsuarioId = Convert.ToInt64(dr["UsuarioId"]);
            NombreProveedor = dr["NombreProveedor"].ToString();
        }
    }
}