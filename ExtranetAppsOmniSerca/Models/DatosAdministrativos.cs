using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using ExtranetAppsOmniSerca.Helpers;

namespace ExtranetAppsOmniSerca.Models
{
    //public enum FormasDePago
    //{
    //    TarjetaDeCrédito,
    //    Pagomiscuentas,
    //    DebitoEnCuenta
    //}

    public class DatosAdministrativos
    {
        internal static readonly string Prefix = "da_";

        public static List<string> GetAllPrefix()
        {
            List<string> allPrefix = new List<string>();
            allPrefix.Add(Prefix);
            allPrefix.Add(TarjetaDeCredito.Prefix);
            allPrefix.Add(DebitoEnCuenta.Prefix);
            return allPrefix;
        }

        public DatosAdministrativos()
        { }

        public DatosAdministrativos(DataRow dr)
        {
            Adelanto = Convert.ToDecimal(dr["Adelanto"]);
            ImporteMensual = Convert.ToDecimal(dr["ImporteMensual"]);

            FormaDePago = new FormaDePago(dr);

            int proximoPago = Convert.ToInt32(dr["ProximoPago"]);
            if (proximoPago < 200000)
                proximoPago = 200001;
            ProximoPago = proximoPago.ToString().AnsiToFormatedDate();

            Firmado = dr["Firmado"].ToString() == "" || dr["Firmado"].ToString() == "0" ? false : Convert.ToBoolean(Convert.ToInt16(dr["Firmado"]));
        }

        public decimal ImporteMensual { get; set; }
        public decimal Adelanto { get; set; }
        public string ProximoPago { get; set; }
        public FormaDePago FormaDePago { get; set; }
        public TarjetaDeCredito TarjetaDeCredito { get; set; }
        public bool Pagomiscuentas { get; set; }
        public DebitoEnCuenta DebitoEnCuenta { get; set; }
        public bool Firmado { get; set; }

        public string FirmaBase64 { get; set; }
    }
}