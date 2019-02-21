using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExtranetAppsOmniSerca.Models
{
    public class Factura
    {
        public long ID { get; set; }
        public long CAE { get; set; }
        public string Fecha { get; set; }
        public string Periodo { get; set; }
        public decimal Monto { get; set; }
        public string Tipo { get; set; }
        public string Sucursal { get; set; }
        public string Numero { get; set; }
        public string NroOP { get; set; }
        public string ProveedorTangoId { get; set; }

        //Nombre de los archivos para asociar al email
        public string pdf { get; set; }
        public string cabecera { get; set; }
        public string detalle { get; set; }
        public string venta { get; set; }

    }
}