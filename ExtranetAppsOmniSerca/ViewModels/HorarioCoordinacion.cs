using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using m=ExtranetAppsOmniSerca.Models;

namespace ExtranetAppsOmniSerca.ViewModels
{
	public class HorarioCoordinacion
	{
		public string Legajo { get; set; }
		public string Medico { get; set; }
		public long ID1 { get; set; }
		public string Entrada1 { get; set; }
		public string Salida1 { get; set; }
		public string Movil1 { get; set; }
		public long ID2 { get; set; }
		public string Entrada2 { get; set; }
		public string Salida2 { get; set; }
		public string Movil2 { get; set; }
		public long ID3 { get; set; }
		public string Entrada3 { get; set; }
		public string Salida3 { get; set; }
		public string Movil3 { get; set; }
		public long ID4 { get; set; }
		public string Entrada4 { get; set; }
		public string Salida4 { get; set; }
		public string Movil4 { get; set; }
		public long ID5 { get; set; }
		public string Entrada5 { get; set; }
		public string Salida5 { get; set; }
		public string Movil5 { get; set; }
		public long ID6 { get; set; }
		public string Entrada6 { get; set; }
		public string Salida6 { get; set; }
		public string Movil6 { get; set; }
		public long ID7 { get; set; }
		public string Entrada7 { get; set; }
		public string Salida7 { get; set; }
		public string Movil7 { get; set; }

		public bool Libre(int posicion)
		{
			switch (posicion)
			{
				case 1:
					return ID1==0;
				case 2:
					return ID2 == 0;
				case 3:
					return ID3 == 0;
				case 4:
					return ID4 == 0;
				case 5:
					return ID5 == 0;
				case 6:
					return ID6 == 0;
				case 7:
					return ID7 == 0;
				default:
					return false;
			}
		}
		public void Set(int posicion, m.HorarioCoordinacion valor)
		{
			switch (posicion)
			{
				case 1:
					ID1 = valor.Id;
					Entrada1 = valor.HorEntrada;
					Salida1 = valor.HorSalida;
					Movil1 = valor.MovilId;
					break;
				case 2:
					ID2 = valor.Id;
					Entrada2 = valor.HorEntrada;
					Salida2 = valor.HorSalida;
					Movil2 = valor.MovilId;
					break;
				case 3:
					ID3 = valor.Id;
					Entrada3 = valor.HorEntrada;
					Salida3 = valor.HorSalida;
					Movil3 = valor.MovilId;
					break;
				case 4:
					ID4 = valor.Id;
					Entrada4 = valor.HorEntrada;
					Salida4 = valor.HorSalida;
					Movil4 = valor.MovilId;
					break;
				case 5:
					ID5 = valor.Id;
					Entrada5 = valor.HorEntrada;
					Salida5 = valor.HorSalida;
					Movil5 = valor.MovilId;
					break;
				case 6:
					ID6 = valor.Id;
					Entrada6 = valor.HorEntrada;
					Salida6 = valor.HorSalida;
					Movil6 = valor.MovilId;
					break;
				case 7:
					ID7 = valor.Id;
					Entrada7 = valor.HorEntrada;
					Salida7 = valor.HorSalida;
					Movil7 = valor.MovilId;
					break;
				default:
					break;
			}
		}
	}
}