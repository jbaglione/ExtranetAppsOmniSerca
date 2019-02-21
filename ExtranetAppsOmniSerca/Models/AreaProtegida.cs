using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace ExtranetAppsOmniSerca.Models
{
    public class AreaProtegida
    {
        internal static readonly string Prefix = "ap_";
        //Necesario para la conversion Json.
        public AreaProtegida()
        {
        }

        public AreaProtegida(DataRow dr)
        {
            FlgDomicilioLegalCobertura = Convert.ToInt32(dr["FlgDomicilioLegalCobertura"]);
            Altura = Convert.ToInt32(dr["Altura"]);
            Depto = dr["Depto"].ToString();
            Calle = dr["Calle"].ToString();
            Observaciones = dr["Observaciones"].ToString();
            Piso = Convert.ToInt32(dr["Piso"]);
            Referencia = dr["Referencia"].ToString();
            EntreCalle1 = dr["EntreCalle1"].ToString();
            EntreCalle2 = dr["EntreCalle2"].ToString();
            CP = dr["CodigoPostal"].ToString();
            Localidad = new Localidad(dr);
            Latitud = dr.Table.Columns.Contains("Latitud") ? dr["Latitud"].ToString() : "";
            Longitud = dr.Table.Columns.Contains("Longitud") ? dr["Longitud"].ToString() : "";
            Telefono = dr.Table.Columns.Contains("Telefono") ? dr["Telefono"].ToString() : "";

        }
        public int FlgDomicilioLegalCobertura { get; set; }
        public string Calle { get; set; }
        public int Altura { get; set; }
        public int Piso { get; set; }
        public string Depto { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public string Referencia { get; set; }
        public string EntreCalle1 { get; set; }
        public string EntreCalle2 { get; set; }
        public string CP { get; set; }
        public Localidad Localidad { get; set; }
        public string Telefono { get; set; }
        public string Observaciones { get; set; }
        
        //TODO: Cambiar por Productos
    }
}