using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ExtranetAppsOmniSerca.Models
{
	public class GrupoUsuario
	{
		public Int64 Id { get; set; }
		public string Descripcion { get; set; }
		public GrupoUsuario(DataRow dr)
		{
			Id = (Int64)dr["ID"];
			Descripcion = (string)dr["Descripcion"];
		}
        public GrupoUsuario( Int64 Id, string Descripcion)
        {
            this.Id = Id;
            this.Descripcion = Descripcion;
        }
	}
}