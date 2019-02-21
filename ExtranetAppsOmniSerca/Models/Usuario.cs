using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ExtranetAppsOmniSerca.Models
{
	public class Usuario
	{
		public Int64 Id { get; set; }
		public string Nombre { get; set; }
        public int MailOpen { get; set; }
        public int AppOpen { get; set; }

        public Usuario()
		{

		}
		public Usuario(DataRow dr)
		{
			Id = dr.Table.Columns.Contains("ID") ? (long)dr["ID"] : (long)dr["UsuarioReceptorId"];
            Nombre = (string)dr["Nombre"];
            
            MailOpen = dr.Table.Columns.Contains("MailOpen") ? Convert.ToInt32(dr["MailOpen"].ToString()) : 0;
            AppOpen = dr.Table.Columns.Contains("AppOpen") ? Convert.ToInt32(dr["AppOpen"].ToString()) : 0;
        }
	}
}