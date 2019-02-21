using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ExtranetAppsOmniSerca.Models
{
    public enum TipoDeCuenta
    {
        CC,
        CA
    }
    public class DebitoEnCuenta
    {
        internal static readonly string Prefix = "dc_";
        public DebitoEnCuenta()
        { }

        public DebitoEnCuenta(DataRow dr)
        {
            Banco = new Banco(dr, "BancoID", "Banco");

            CBU = dr["CBU"].ToString();
            NombreTitular = dr["NombreTitular"].ToString();
            NroDNI = dr["NroDNI"].ToString();
            NroCuenta = dr["NroCuenta"].ToString();
            TipoCuenta = new TipoCuenta
            {
                ID = Convert.ToInt32(dr["TipoCuentaID"])
            };

        }

        public Banco Banco { get; set; }
        public TipoCuenta TipoCuenta { get; set; }
        public string NroCuenta { get; set; }
        public string NombreTitular { get; set; }
        public string NroDNI { get; set; }
        public string CBU { get; set; }
    }
}