using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ExtranetAppsOmniSerca.Models
{
    public class EmpleadoLiquidacionDetalle
    {
        public EmpleadoLiquidacionDetalle(DataRow dr)
        {
            //TODO: Rename Camelcase in WS and Class
            ID = dr["ID"].ToString();
            MovilPacto = dr["MovilPacto"].ToString();
            TipoNovedad = dr["TipoNovedad"].ToString();
            MovilReal = dr["MovilReal"].ToString();
            pacFecHorEntrada = dr["pacFecHorEntrada"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["pacFecHorEntrada"]);
            pacFecHorSalida = dr["pacFecHorSalida"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["pacFecHorSalida"]);
            relFecHorEntrada = dr["relFecHorEntrada"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["relFecHorEntrada"]);
            relFecHorSalida = dr["relFecHorSalida"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["relFecHorSalida"]);
            MinTrabajado = Convert.ToInt64(dr["MinTrabajado"]);
            MinNocturno = Convert.ToInt64(dr["MinNocturno"]);
            MinDiurno = Convert.ToInt64(dr["MinDiurno"]);
            MinFinSemana = Convert.ToInt64(dr["MinFinSemana"]);
            MinLLegadaTarde = Convert.ToInt64(dr["MinLLegadaTarde"]);
            MinRetAnticipado = Convert.ToInt64(dr["MinRetAnticipado"]);
            MinIncumplimiento = Convert.ToInt64(dr["MinIncumplimiento"]);
            flgAusenciaSinPremio = Convert.ToInt64(dr["flgAusenciaSinPremio"]);
            cntServicios = Convert.ToInt64(dr["cntServicios"]);
            Status = Convert.ToInt64(dr["Status"]);
        }
        public string ID { get; set; }
        public DateTime? pacFecHorEntrada { get; set; }
        public DateTime? pacFecHorSalida { get; set; }
        public string MovilPacto { get; set; }
        public DateTime? relFecHorEntrada { get; set; }
        public DateTime? relFecHorSalida { get; set; }
        public string MovilReal { get; set; }
        public long MinTrabajado { get; set; }
        public long MinNocturno { get; set; }
        public long MinDiurno { get; set; }
        public long MinFinSemana { get; set; }
        public long MinIncumplimiento { get; set; }
        public long MinLLegadaTarde { get; set; }
        public long MinRetAnticipado { get; set; }
        public long flgAusenciaSinPremio { get; set; }
        public long cntServicios { get; set; }
        public string TipoNovedad { get; set; }
        public long Status { get; set; }
        public string pacFecEntrada
        {
            get
            {
                CultureInfo ci = new CultureInfo("Es-Es");
                Console.WriteLine(ci.DateTimeFormat.GetDayName(DateTime.Now.DayOfWeek));

                DateTime pacFec = pacFecHorEntrada.HasValue ? Convert.ToDateTime(pacFecHorEntrada.Value) : Convert.ToDateTime(relFecHorEntrada);

                string diaNombre = ci.DateTimeFormat.GetDayName(pacFec.DayOfWeek);
                string diaNumero = pacFec.Day.ToString();
                return string.Format("{0} - {1} ", diaNombre.Substring(0, 3), diaNumero);
            }
        }

        public string HoraTrabajado
        { get { return FormatearMinutos(MinTrabajado); } }

        public string HoraNocturno
        { get { return FormatearMinutos(MinNocturno); } }

        public string HoraDiurno
        { get { return FormatearMinutos(MinDiurno); } }

        public string HoraFinSemana
        { get { return FormatearMinutos(MinFinSemana); } }

        public string HoraIncumplimiento
        { get { return FormatearMinutos(MinIncumplimiento); } }

        private string FormatearMinutos(long minutos)
        {
            return minutos == 0 ? "" :
                string.Format((minutos / 60).ToString("00")) + ":" +
                string.Format((minutos % 60).ToString("00"));
        }
    }
}