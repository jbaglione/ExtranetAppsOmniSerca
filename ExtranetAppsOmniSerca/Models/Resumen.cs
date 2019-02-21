using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExtranetAppsOmniSerca.Models
{
    public class Resumen
    {
        public List<ResumenItem> Productividad { get; set; }
        public List<ResumenItem> Factura { get; set; }
        public List<ResumenItem> Retenciones { get; set; }
        public List<ResumenItem> Descuentos { get; set; }
        public List<ResumenItem> Pagos { get; set; }

        public Resumen()
        {
            Productividad = new List<ResumenItem>();
            Factura = new List<ResumenItem>();
            Retenciones = new List<ResumenItem>();
            Descuentos = new List<ResumenItem>();
            Pagos = new List<ResumenItem>();
        }
    }
}