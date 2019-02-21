using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExtranetAppsOmniSerca.ViewModels
{
    public class FiltroPeriodos
    {
        public string Periodo { get; set; }
        public string Descripcion { get; set; }

        public FiltroPeriodos(string periodo, string descripcion)
        {
            this.Periodo = periodo;
            this.Descripcion = descripcion;
        }
    }
}