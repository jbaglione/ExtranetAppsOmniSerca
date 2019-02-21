using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace ExtranetAppsOmniSerca.Models
{
    public class IncidenteDetalle
    {
        public DateTime FecIncidente { get; set; }
        public string NroIncidente { get; set; }
        public string Paciente { get; set; }
        public string Sexo { get; set; }
        public string Edad { get; set; }
        public string Origen { get; set; }
        public string Destino { get; set; }
        public string Anexo1 { get; set; }
        public string Anexo2 { get; set; }
        public string Concepto { get; set; }
        public string Motivo { get; set; }
        public decimal Importe { get; set; }
        public decimal CoPago { get; set; }
        public DateTime HorDespacho { get; set; }
        public DateTime HorLlegada { get; set; }
        public List<IncidenteCalculo> IncidenteCalculoList { get; set; }

        public IncidenteDetalle(DataRow dr, DataTable dtCalculo = null)
        {
            FecIncidente = DateTime.ParseExact(dr["FecIncidente"].ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture); //Convert.ToInt64(dr["FecIncidente"].ToString());
            NroIncidente = dr["NroIncidente"].ToString();
            Paciente = dr["Paciente"].ToString();
            Sexo =dr["Sexo"].ToString();
            Edad = dr["Edad"].ToString();
            Origen = dr["Origen"].ToString();
            Destino = dr["Destino"].ToString();
            Anexo1 = dr["Anexo1"].ToString();
            Anexo2 = dr["Anexo2"].ToString();
            Concepto = dr["Concepto"].ToString();
            Motivo = dr["Motivo"].ToString();
            Importe = Convert.ToDecimal(dr["Importe"].ToString());
            CoPago = Convert.ToDecimal(dr["CoPago"].ToString());
            HorDespacho = Convert.ToDateTime(dr["HorDespacho"].ToString());
            HorLlegada = Convert.ToDateTime(dr["HorLlegada"].ToString());
            IncidenteCalculoList = new List<IncidenteCalculo>();
            if (dtCalculo != null)
                foreach (DataRow drCalculo in dtCalculo.Rows)
                    IncidenteCalculoList.Add(new IncidenteCalculo(drCalculo));
            //Limpiar campo Edad
            int iReturn;
            if (!string.IsNullOrEmpty(Edad) && !int.TryParse(Edad, out iReturn))
            {
                Edad = Edad.ToUpper();
                int i = -1;
                do
                {
                    i++;
                    if (!Char.IsNumber(Edad[i]))
                    {
                        if (Edad.Substring(i, 1) == "A")
                            Edad = Edad.Substring(0, i);
                        else
                            Edad = Edad.Substring(0, i+1);
                    }
                } while (i < Edad.Length - 1);
            }

        }
    }
}