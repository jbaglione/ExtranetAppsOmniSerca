using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace ExtranetAppsOmniSerca.Models
{
    public class Asistencia
    {
        public DateTime FecMovimiento { get; set; }
        public string DiaSemana { get; set; }
        public DateTime pacFecHorInicio { get; set; }
        public DateTime pacFecHorFinal { get; set; }
        public int HorasPactadas { get; set; }
        public DateTime? relFecHorInicio { get; set; }
        public int minTarde { get; set; }
        public DateTime Tarde { get; set; }
        public DateTime? relFecHorFinal { get; set; }
        public int minAnticipado { get; set; }
        public DateTime Anticipado { get; set; }
        public string MotivoDescuento { get; set; }
        public int virEvlDescontable { get; set; }
        public DateTime virTpoDescontable { get; set; }
        public int HorasTrabajadas { get; set; }

        public Asistencia(DataRow dr)
        {
            FecMovimiento = Convert.ToDateTime(dr["FecMovimiento"].ToString());
            DiaSemana = dr["DiaSemana"].ToString();
            if (dr["pacFecHorInicio"] != DBNull.Value)
                pacFecHorInicio = Convert.ToDateTime(dr["pacFecHorInicio"].ToString());
            if (dr["pacFecHorFinal"] != DBNull.Value)
                pacFecHorFinal = Convert.ToDateTime(dr["pacFecHorFinal"].ToString());
            HorasPactadas = Convert.ToInt32(dr["HorasPactadas"].ToString());
            if (dr["relFecHorInicio"].ToString() != "")
                relFecHorInicio = Convert.ToDateTime(dr["relFecHorInicio"].ToString());
            minTarde = Convert.ToInt32(dr["minTarde"].ToString());
            Tarde = Convert.ToDateTime(dr["Tarde"].ToString());//-> mintarde>15 -> rojo
            if (dr["relFecHorFinal"].ToString() != "")
                relFecHorFinal = Convert.ToDateTime(dr["relFecHorFinal"].ToString());
            minAnticipado = Convert.ToInt32(dr["minAnticipado"].ToString());
            Anticipado = Convert.ToDateTime(dr["Anticipado"].ToString());//-> minAnticipado>15 -> rojo
            MotivoDescuento = dr["MotivoDescuento"].ToString();
            virEvlDescontable = Convert.ToInt32(dr["virEvlDescontable"].ToString());
            virTpoDescontable = Convert.ToDateTime(dr["virTpoDescontable"].ToString());//virEvlDescontable>15-->rojo
            HorasTrabajadas = Convert.ToInt32(dr["HorasTrabajadas"].ToString());
        }
    }
}