using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using ExtranetAppsOmniSerca.Helpers;

namespace ExtranetAppsOmniSerca.Models
{
    public class SolicitudDigitalizada
    {
        public SolicitudDigitalizadaBasico SolicitudDigitalizadaBasico { get; set; }

        public Cliente EntidadCliente { get; set; }

        public ClienteContacto ClienteContacto { get; set; }

        public AreaProtegida AreaProtegida { get; set; }

        public DatosAdministrativos DatosAdministrativos { get; set; }

        public List<Producto> Productos { get; set; }

        public static List<string> GetAllPrefix()
        {
            List<string> allPrefix = new List<string>();
            //allPrefix.Add(Prefix);
            allPrefix.Add(SolicitudDigitalizadaBasico.Prefix);
            allPrefix.Add(Cliente.Prefix);
            allPrefix.Add("cc_");
            allPrefix.Add(AreaProtegida.Prefix);
            allPrefix.AddRange(DatosAdministrativos.GetAllPrefix());
            return allPrefix;
            //return new List<string> { "sdb_", "ap_", "da_", "tc_", "dc_", "cc_" };
        }
    }

    public class SolicitudDigitalizadaBasico
    {
        public static string Prefix = "sdb_";

        //Necesario para la conversion Json.
        public SolicitudDigitalizadaBasico()
        {
        }

        public SolicitudDigitalizadaBasico(DataRow dr)
        {
            int fecIngreso = Convert.ToInt32(dr["FechaIngreso"]);

            if (fecIngreso < 20000000)
                fecIngreso = 20000101;
            FechaIngreso = Convert.ToDateTime(fecIngreso.ToString().AnsiToFormatedDate());

            NroArea = dr["NroArea"].ToString();
            NroSolicitud = dr["NroSolicitud"].ToString() == "" ? 0 : Convert.ToInt64(dr["NroSolicitud"]);
            Vendedor = dr["Vendedor"].ToString();
            VendedorId = dr["VendedorId"].ToString() == "" ? 0 : Convert.ToInt64(dr["VendedorId"]);
        }

        public DateTime FechaIngreso { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public long NroSolicitud { get; set; }
        public string NroArea { get; set; }
        public string Vendedor { get; set; }
        public long VendedorId { get; set; }
        
    }
}