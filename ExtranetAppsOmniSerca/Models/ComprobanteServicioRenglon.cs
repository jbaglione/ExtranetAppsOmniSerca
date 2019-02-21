using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using ExtranetAppsOmniSerca.Helpers;

namespace ExtranetAppsOmniSerca.Models
{
	public class ComprobanteServicioRenglon
	{
		public string Codigo { get; set; }
		public string Concepto { get; set; }
		public long Cantidad { get; set; }
		public decimal Unitario { get; set; }
		public decimal Importe { get; set; }

		public ComprobanteServicioRenglon( DataRow row)
		{
			Codigo = row.GetNulleableDBString("Codigo");
			Concepto = row.GetNulleableDBString("Concepto");
			Cantidad = Convert.ToInt64(row["Cantidad"]);
			Unitario = (decimal)row["Unitario"];
			Importe = (decimal)row["Importe"];
		}
		public ComprobanteServicioRenglon()
		{

		}
	}
}