using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using ExtranetAppsOmniSerca.Helpers;

namespace ExtranetAppsOmniSerca.Models
{
	public class ComprobanteServicio
	{
		public long ID { get; set; }
		public string NroIncidente{ get; set; }
		public long Fecha { get; set; }
		public string FormatedFecha
		{
			get
			{
				return Fecha.ToString().AnsiToFormatedDate();
			}
		}
		public string ConceptoId { get; set; }
		public string NroInterno{ get; set; }
		public string Iva{ get; set; }
		public string Arba{ get; set; }
		public string Agip{ get; set; }
		public string NroAfiliado{ get; set; }
		public string Paciente { get; set; }
		public string FormatedPaciente
		{
			get
			{
				int largo = Paciente.Length;
				if(largo>3)
				if (Paciente.IndexOf(',', 3) > 0)
				{
					largo = Paciente.IndexOf(',', 2);
				}
				else if (Paciente.IndexOf(' ', 3) > 0)
				{
					largo = Paciente.IndexOf(' ', 3);
				}
				return Paciente.Substring(0, largo);

			}
		}
		public string Desde{ get; set; }
		public string Hasta{ get; set; }
		public long Kmt{ get; set; }
		public string TpoEspera{ get; set; }
		public decimal ImporteBase{ get; set; }
		public decimal Recargos{ get; set; }
		public decimal Importe{ get; set; }
		public string OrdenesFiles { get; set; }

		public ComprobanteServicio()
		{

		}
		public ComprobanteServicio(DataRow dr)
		{
			ID = Convert.ToInt64(dr["ID"]);
			Fecha = Convert.ToInt64(dr["FecIncidente"]);
			NroIncidente = dr.GetNulleableDBString("NroIncidente");
			ConceptoId = dr.GetNulleableDBString("ConceptoId");
			NroInterno = dr.GetNulleableDBString("NroInterno");
			Iva = dr.GetNulleableDBString("Iva");
			Arba = dr.GetNulleableDBString("Arba");
			Agip = dr.GetNulleableDBString("Agip");
			NroAfiliado = dr.GetNulleableDBString("NroAfiliado");
			Paciente = dr.GetNulleableDBString("Paciente");
			Desde = dr.GetNulleableDBString("Desde");
			Hasta = dr.GetNulleableDBString("Hasta");
			Kmt = Convert.ToInt64(dr["Kmt"]);
			TpoEspera = dr.GetNulleableDBString("TpoEspera");
			ImporteBase = (decimal)dr["ImporteBase"];
			Recargos = (decimal)dr["Recargos"];
			Importe = (decimal)dr["Importe"];
			OrdenesFiles = dr.GetNulleableDBString("OrdenesFiles");

		}
	}
}