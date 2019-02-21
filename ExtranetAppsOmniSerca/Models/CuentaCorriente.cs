using ExtranetAppsOmniSerca.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ExtranetAppsOmniSerca.Models
{
    public class CuentaCorriente
    {
        public long ID { get; set; }
        /*
        public long Periodo { get; set; }
        public string FormatedPeriodo
        {
            get
            {
                if (Periodo == 0)
                    return "";
                return $"{Periodo.ToString().Substring(4)}/{Periodo.ToString().Substring(0, 4)}";
            }
        }
        */
        public long FecPago { get; set; }
        public string FormatedFecPago
        {
            get
            {
                if (FecPago == 0)
                    return "";
                return FecPago.ToString().AnsiToFormatedDate();
            }
        }
        public string NroOrdenPago { get; set; }
        public string TipoComprobante { get; set; }
        public string NroComprobante { get; set; }
        public string Referencias { get; set; }
        public decimal ImporteOP { get; set; }
        public bool Arba { get; set; }
        public bool Agip { get; set; }
        public bool Ganancias { get; set; }
        public bool Iva { get; set; }
        public bool CajaPrevisional { get; set; }
        public List<CuentaCorriente> CuentaCorrientes { get; set; }
        public bool TieneHijos { get; set; }
        public bool Novedad { get; set; }
        public string ProveedorTangoId { get; set; }

        public CuentaCorriente()
        {

        }

        public CuentaCorriente(long id, DataRow dr, bool tieneHijos)
        {
            ID = id;
            //ID = Convert.ToInt64(dr["ID"]);
            //Periodo = Convert.ToInt64(dr["Periodo"]);
            FecPago = Convert.ToInt64(dr["FecPago"].ToString().Replace("-", "").Substring(0, 8));
            NroOrdenPago = dr.GetNulleableDBString("NroOrdenPago");
            TipoComprobante = dr.GetNulleableDBString("TipoComprobante");
            NroComprobante = dr.GetNulleableDBString("NroComprobante");
            Referencias = dr.GetNulleableDBString("Referencias");
            ImporteOP = (decimal)dr["Importe"];
            TieneHijos = tieneHijos;
            if (!tieneHijos)
            {
                Novedad = (((DateTime.Now - DateTime.ParseExact(FormatedFecPago, "dd/MM/yyyy", CultureInfo.InvariantCulture)).Days <= 7) && NroComprobante == "");
                Arba = dr.GetNulleableDBString("Arba") == "1";
                Agip = dr.GetNulleableDBString("Agip") == "1";
                Ganancias = dr.GetNulleableDBString("Ganancias") == "1";
                Iva = dr.GetNulleableDBString("Iva") == "1";
                CajaPrevisional = dr.GetNulleableDBString("CajaPrevisional") == "1";
            }
            ProveedorTangoId = dr.GetNulleableDBString("ProveedorTangoId");
            CuentaCorrientes = new List<CuentaCorriente>();
        }

    }
}