using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExtranetAppsOmniSerca.ViewModels
{
    public class FiltroCoordinaciones
    {
        public int ID { get; set; }
        public string Descripcion { get; set; }

        public FiltroCoordinaciones(int id, string descripcion)
        {
            this.ID = id;
            this.Descripcion = descripcion;
        }
    }
}