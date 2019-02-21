using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ExtranetAppsOmniSerca.Models
{
	public class Notificacion
	{
		public enum Estados
		{
			Pendiente=0,
			Leido=1
		}
		public int Id { get; set; }
		public DateTime Fecha { get; set; }
		public string FechaConFormato { get { return Fecha.ToString("dd/MM/yyyy"); } }
		public string Mensaje { get; set; }
		public int EstadoId { get; set; }
        public bool Mail { get; set; }
        public string MailText { get; set; }
        public List<int> Usuarios { get; set; }
		public List<Usuario> UsuarioList { get; set; }
		public List<GrupoUsuario> Grupos { get; set; }
		public Notificacion()
		{
			Grupos = new List<GrupoUsuario>();
		}
		public Notificacion(DataRow dr, bool gestion)
		{
			Id = Convert.ToInt32(dr["ID"]);
			if (!(dr["FecHorNotificacion"] is DBNull))
				Fecha = DateTime.Parse((string)dr["FecHorNotificacion"]);
			Mensaje = (string)dr["Mensaje"];
            if (gestion)
            {
                MailText = (string)dr["NotificacionEmail"];
                Mail = MailText =="SI";
            }
            try
			{
				EstadoId = Convert.ToInt32(dr["EstadoId"]);
			}
			catch (Exception)
			{
			}
		}
	}
}