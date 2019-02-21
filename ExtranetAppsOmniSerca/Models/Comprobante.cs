using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using ExtranetAppsOmniSerca.Helpers;

namespace ExtranetAppsOmniSerca.Models
{
	public class Comprobante
	{
		public long ID { get; set; }
		public long Fecha { get; set; }
		public string FormatedFecha
		{
			get
			{
				return Fecha.ToString().AnsiToFormatedDate();
			}
		}
		public string TipoComprobante{ get; set; }
		public string NroComprobante{ get; set; }
		public decimal ImporteExento{ get; set; }
		public decimal ImporteGravado{ get; set; }
		public decimal PorcentajeIva{ get; set; }
		public decimal ImporteIva{ get; set; }
		public decimal PorcentajeARBA{ get; set; }
		public decimal ImporteARBA{ get; set; }
		public decimal PorcentajeAGIP{ get; set; }
		public decimal ImporteAGIP{ get; set; }
		public decimal Importe{ get; set; }
		public Comprobante()
		{

		}
		public Comprobante(DataRow dr)
		{
			ID = Convert.ToInt64(dr["ID"]);
			Fecha = Convert.ToInt64(dr["FecDocumento"]);
			TipoComprobante = (string)dr["TipoComprobante"];
			NroComprobante = (string)dr["NroComprobante"];
			ImporteExento = (decimal)dr["ImporteExento"];
			ImporteGravado = (decimal)dr["ImporteGravado"];
			PorcentajeIva = (decimal)dr["PorcentajeIva"];
			ImporteIva = (decimal)dr["ImporteIva"];
			PorcentajeARBA = (decimal)dr["PorcentajeARBA"];
			ImporteARBA = (decimal)dr["ImporteARBA"];
			PorcentajeAGIP = (decimal)dr["PorcentajeAGIP"];
			ImporteAGIP = (decimal)dr["ImporteAGIP"];
			Importe = (decimal)dr["Importe"];

		}
	}
}