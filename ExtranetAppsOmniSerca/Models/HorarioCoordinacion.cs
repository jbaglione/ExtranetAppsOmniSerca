using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExtranetAppsOmniSerca.Models
{
	public class HorarioCoordinacion
	{
		public long Id { get; set; }
		public string PersonalId { get; set; }
		public string Apellido { get; set; }
		public string FecEntrada { get; set; }
		public string HorEntrada { get; set; }
		public string FecSalida { get; set; }
		public string HorSalida { get; set; }
		public string MovilId { get; set; }
		public long SituacionId { get; set; }
	}
}