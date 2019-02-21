using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ExtranetAppsOmniSerca.Models
{
    public class ClienteContacto
    {
        internal static readonly string Prefix = "cc_";

        public ClienteContacto()
        { }
        public ClienteContacto(DataRow data)
        {
            //No esta en DB
            //ClienteID = Convert.ToInt32(data["ClienteID"]);
            NombreCompleto = data["NombreCompleto"].ToString();
            Cargo = data["Cargo"].ToString();
            Telefono = data["Telefono"].ToString();
            Email = data["Email"].ToString();
            TelefonoAlternativo = data["TelefonoAlternativo"].ToString();
            EmailAlternativo = data["EmailAlternativo"].ToString();
        }
        //public int ClienteID { get; set; }
        public string NombreCompleto { get; set; }
        public string Cargo { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string TelefonoAlternativo { get; set; }
        public string EmailAlternativo { get; set; }
    }
}