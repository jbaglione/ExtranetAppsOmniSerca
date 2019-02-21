using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExtranetAppsOmniSerca.Models
{
    public class SetMensajeResult
    {
        public int UsuarioId { get; set; }
        public string Nombre { get; set; }
        public int Resultado { get; set; }
        public string DescripcionError { get; set; }
        public int NotificacionUsuarioId { get; set; }
    }
}