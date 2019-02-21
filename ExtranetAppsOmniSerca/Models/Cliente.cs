using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ExtranetAppsOmniSerca.Models
{
    //public class Estados
    //{
    //    public int ID { get; set; }
    //    public string Descripcion { get; set; }
    //}

    public class Cliente
    {
        internal static readonly string Prefix = "cli_";
        public enum EstadosEnum
        {
            Todos = 0, potencial = 1, preparado = 2, activo = 3, inactivo = 4
        }
        //public static List<Estados> EstadosLista
        //{
        //    get
        //    {
        //        return new List<Estados> {
        //            new Estados {ID=0, Descripcion = "Todos" },
        //            new Estados {ID=1, Descripcion = "Potenciales" },
        //            new Estados {ID=2, Descripcion = "Preparados" },
        //            new Estados {ID=3, Descripcion = "Activos" },
        //            new Estados {ID=4, Descripcion = "Inactivos" },
        //        };
        //    }
        //}

        public Cliente()
        { }
        public Cliente(DataRow dr)
        {
            ClienteId = Convert.ToInt64(dr["ClienteId"]);
            NombreComercial = dr["NombreComercial"].ToString();
            Rubro = new Rubro(dr, "RubroId", "Rubro");
            RazonSocial = dr["RazonSocial"].ToString();
            Cuit = dr["Cuit"].ToString();
            CondicionIVA = new CondicionIVA(dr, "CondicionIVAId", "CondicionIVA");
            Latitud = dr.Table.Columns.Contains("Latitud") ? dr["Latitud"].ToString() : "";
            Longitud = dr.Table.Columns.Contains("Longitud") ? dr["Longitud"].ToString() : "";
            Domicilio = dr.Table.Columns.Contains("Domicilio") ? dr["Domicilio"].ToString() : "";
            Calle = dr["Calle"].ToString();
            if (dr["Altura"] != DBNull.Value)
                Altura = Convert.ToInt32(dr["Altura"]);
            if (dr["Piso"] != DBNull.Value)
                Piso = Convert.ToInt32(dr["Piso"]);
            Depto = dr["Depto"].ToString();
            Referencia = dr["Referencia"].ToString();
            EntreCalle1 = dr["EntreCalle1"].ToString();
            EntreCalle2 = dr["EntreCalle2"].ToString();
            CP = dr["CodigoPostal"].ToString();
            Localidad = new Localidad(dr, "Localidad");


            Estado = Convert.ToInt32(dr["Estado"]);
            CredencialID = dr["CredencialID"].ToString();//nro de cliente
        }

        public long ClienteId { get; set; }
        public string NombreComercial { get; set; }
        public Rubro Rubro { get; set; }
        //public string RubroId { get; set; }
        //public string Rubro { get; set; }
        public string RazonSocial { get; set; }
        public string Cuit { get; set; }
        public long CuitNumber
        {
            get
            {
                string cuitSinGuion = Cuit.Replace("-", "");
                string cuitNumber = Regex.Match(cuitSinGuion, @"\d+").Value;
                return string.IsNullOrEmpty(cuitNumber) ? 0 : Convert.ToInt64(cuitNumber);
            }
        }
        public CondicionIVA CondicionIVA { get; set; }
        //public string CondicionIVAId { get; set; }
        //public string CondicionIVA { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public string Domicilio { get; set; }
        public string Calle { get; set; }
        public int Altura { get; set; }
        public int Piso { get; set; }
        public string Depto { get; set; }
        public string Referencia { get; set; }
        public string EntreCalle1 { get; set; }
        public string EntreCalle2 { get; set; }
        public string CP { get; set; }
        public Localidad Localidad { get; set; }
        public int Estado { get; set; } //TODO: potencial = 1, activo = 2, inactivo = 3
        public string CredencialID { get; set; }//TODO: potencial no tienen codigocliente

        public int EsPreparado
        {
            get { return (EstadosEnum)Estado == EstadosEnum.preparado ? 1 : 0; }
        }
        public int EsPotencial
        {
            get { return (EstadosEnum)Estado == EstadosEnum.potencial ? 1 : 0; }
        }
        public int EsActivo
        {
            get { return (EstadosEnum)Estado == EstadosEnum.activo ? 1 : 0; }
        }
        public int EsInactivo
        {
            get { return (EstadosEnum)Estado == EstadosEnum.inactivo ? 1 : 0; }
        }
    }
}
