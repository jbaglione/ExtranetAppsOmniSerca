using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExtranetAppsOmniSerca.ViewModels
{
    public class FiltroReclamo
    {
        public string ID { get; set; }
        public string Descripcion { get; set; }

        public FiltroReclamo(string id, string descripcion)
        {
            this.ID = id;
            this.Descripcion = descripcion;
        }
    }
}