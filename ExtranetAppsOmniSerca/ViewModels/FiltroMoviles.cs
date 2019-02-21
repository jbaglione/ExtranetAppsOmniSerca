using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExtranetAppsOmniSerca.ViewModels
{
    public class FiltroMoviles
    {
        public string ID { get; set; }
        public string Descripcion { get; set; }
        public FiltroMoviles()
        {
        }
        public FiltroMoviles(string ID, string Descripcion)
        {
            this.ID = ID;
            this.Descripcion = Descripcion;
        }
    }
}