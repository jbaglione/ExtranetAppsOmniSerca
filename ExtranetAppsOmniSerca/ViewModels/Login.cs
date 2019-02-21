using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExtranetAppsOmniSerca.ViewModels
{
    public class LoginUsuario
    {
        public string Jerarquia { get; set; }
        public string Titulo { get; set; }
        public string URL { get; set; }
        public string UsuarioNombre { get; set; }
    }

    public class LoginSession
    {
        public List<LoginUsuario> LoginUsuarios { get; set; }
        public SessionData MySessionData { get; set; }
    }
}