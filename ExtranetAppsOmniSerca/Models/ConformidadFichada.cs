using System;
using System.Data;
using System.Globalization;

namespace ExtranetAppsOmniSerca.Models
{
    public class ConformidadFichada
    {
        public int Cerrado { get; set; }
        public int ConformidadId { get; set; }
        public int flgConforme { get; set; }
        public int TerLiqMotivoReclamoId { get; set; }
        public string Entrada { get; set; }
        public string Salida { get; set; }
        public string EntradaReclamo { get; set; }
        public string SalidaReclamo { get; set; }
        public string Observaciones { get; set; }
        public string Respuesta { get; set; }
        public int flgRespuesta { get; set; }
        public ConformidadFichada()
        { }

        public ConformidadFichada(DataRow dr)
        {
            ConformidadId = Convert.ToInt32(dr["ConformidadId"].ToString());
            flgConforme = Convert.ToInt32(dr["flgConforme"].ToString());
            TerLiqMotivoReclamoId = Convert.ToInt32(dr["TerLiqMotivoReclamoId"].ToString());

            Entrada = dr["relFecHorEntrada"].ToString();
            EntradaReclamo = dr["rclFecHorEntrada"].ToString();
            Salida = dr["relFecHorSalida"].ToString();
            SalidaReclamo = dr["rclFecHorSalida"].ToString();

            Observaciones = dr["Observaciones"].ToString();
            Respuesta = dr["Respuesta"].ToString();
            Cerrado = Convert.ToInt32(dr["Cerrado"].ToString());
            flgRespuesta = Convert.ToInt32(dr["flgRespuesta"].ToString());
        }

        public void SetFakeConformidadFichada()
        {
            //FecIncidente = Convert.ToDateTime(dr["FecIncidente"].ToString());
            //FecIncidente = DateTime.ParseExact("20180106", "yyyyMMdd", CultureInfo.InvariantCulture);
            //NroIncidente = "1A";
            ConformidadId = 1;
            flgConforme = 0;
            TerLiqMotivoReclamoId = 2;

            //Entrada = string.Format("{0:N0}:{1:N0}", (400 / 60), (400 % 60));
            EntradaReclamo = string.Format("{0:N0}:{1:N0}", (350 / 60), (350 % 60));
            //Salida = string.Format("{0:N0}:{1:N0}", (850 / 60), (850 % 60));
            SalidaReclamo = string.Format("{0:N0}:{1:N0}", (950 / 60), (950 % 60));

            Observaciones = "Observaciones Observaciones";
            Respuesta = "Respuesta Respuesta";
            Cerrado = 0;
            flgRespuesta = 0;
        }

    }
}