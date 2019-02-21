using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ExtranetAppsOmniSerca.ViewModels
{
    public class SessionData
    {
        public long UsuarioExtranetId { get; set; }
        public long UsuarioShamanId { get; set; }
        public string Identificacion { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }

        public SessionData(DataRow dr)
        {
            UsuarioExtranetId = Convert.ToInt64(dr["UsuarioExtranetId"]);
            UsuarioShamanId = Convert.ToInt64(dr["UsuarioShamanId"]);
            Identificacion = dr["Identificacion"].ToString();
            Nombre = dr["Nombre"].ToString();
            Email = dr["Email"].ToString();
        }
    }
}