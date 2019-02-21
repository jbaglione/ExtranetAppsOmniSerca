using ExtranetAppsOmniSerca.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ExtranetAppsOmniSerca.Models
{
    public class TarjetaDeCredito
    {
        internal static readonly string Prefix = "tc_";
        public TarjetaDeCredito()
        { }
        public TarjetaDeCredito(DataRow dr)
        {
            NombreTitular = dr["NombreTitular"].ToString();
            NroDNI = string.IsNullOrEmpty(dr["NroDNI"].ToString()) ? null : dr["NroDNI"].ToString();
            
            Numero = string.IsNullOrEmpty(dr["Numero"].ToString())?null: dr["Numero"].ToString();

            TipoDeTarjeta = new TipoDeTarjeta
            {
                ID = dr["TarjetaCreditoId"].ToString()                
            };
            ////Falta TipoDeTarjeta en DB
            //TipoDeTarjeta = new TipoDeTarjeta
            //{
            //    ID = "AMER",
            //    Descripcion = "AMERICAN EXPRESS"
            //};
            //Tipo = (TipoDeTarjeta)Enum.Parse(typeof(TipoDeTarjeta), ds.Tables[4].Rows[0]["Tipo"].ToString());
            string vto = dr["Vencimiento"].ToString();
            Vencimiento = vto.Length == 6? vto.Substring(4,2) + "/" + vto.Substring(2, 2): "";
        }

        public TipoDeTarjeta TipoDeTarjeta { get; set; }
        public string Numero { get; set; }
        public string Vencimiento { get; set; }
        public string NombreTitular { get; set; }
        public string NroDNI { get; set; }
    }
}