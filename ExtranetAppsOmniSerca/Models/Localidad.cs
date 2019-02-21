using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text.RegularExpressions;

namespace ExtranetAppsOmniSerca.Models
{
    //public sealed class Localidad
    //{
    //    private static readonly Lazy<Localidad> lazy =
    //        new Lazy<Localidad>(() => new Localidad());

    //    public static Localidad Instance { get { return lazy.Value; } }

    //    private Localidad()
    //    {
    //    }
    //}


    public class Localidad
    {
        public string label { get; set; }
        //private string _label;

        //public string label
        //{
        //    get { return _label;  }
        //    set { _label = Regex.Replace(value, @" ?\(.*?\)", string.Empty); }
        //}

        public string value { get; set; }

        public string PartidoId { get; set; }

        public Localidad()
        {
            
        }
        public Localidad(DataRow dr, string descripcion = "Descripcion", string id = "ID", bool findInList = true)
        {
            if (dr.Table.Columns.Contains(descripcion))
                label = dr[descripcion].ToString();
            else if (dr.Table.Columns.Contains("Localidad"))
                label = dr["Localidad"].ToString();

            if (dr.Table.Columns.Contains(id))
                value = dr[id].ToString();
            else if (dr.Table.Columns.Contains("LocalidadId"))
                value = dr["LocalidadId"].ToString();
            else
                value = FindLocalidadByDesc(label).value;

            if (dr.Table.Columns.Contains("PartidoId"))
                PartidoId = dr["PartidoId"].ToString();
            else
                PartidoId = FindPartidoIdByLocalidadId(value);
        }



        private Localidad FindLocalidadByDesc(string label)
        {
            //Localidad localidad = GetListLocalidades().Where(loc => loc.label == label).FirstOrDefault();
            //return localidad ?? new Localidad { label = label, value = "0" };

            return new Localidad { label = label, value = "0" };
        }

        private string FindPartidoIdByLocalidadId(string value)
        {
            Localidad localidad = ListLocalidades.Where(loc => loc.value == value).FirstOrDefault();
            return localidad != null ? localidad.PartidoId : "0";
        }

        private static IList<Localidad> listLocalidades;

        public static IList<Localidad> ListLocalidades
        {
            get
            {
                if (listLocalidades == null)
                {
                    listLocalidades = GetListLocalidades();
                }
                return listLocalidades;
            }
            set { listLocalidades = value; }
        }

        private static IList<Localidad> GetListLocalidades()
        {
            List<Localidad> lstLocalidad = new List<Localidad>();

            //TODO: WSLocalidades
            //WSLocalidades.LocalidadesSoapClient wsClient = new WSLocalidades.LocalidadesSoapClient();
            //wsClient.Open();
            //DataSet ds = wsClient.GetLocalidadesSistema();
            //wsClient.Close();
            //foreach (DataRow item in ds.Tables[0].Rows)
            //    lstLocalidad.Add(new Localidad(item, "Descripcion", "ID", false));

            ////TODO: Comentar
            ////lstLocalidad.Add(new Localidad { label = "PILAR", value = "0" });
            ////lstLocalidad.Add(new Localidad { label = "José C Paz", value = "1" });
            ////lstLocalidad.Add(new Localidad { label = "Del Viso", value = "2" });
            ////lstLocalidad.Add(new Localidad { label = "San Miguel", value = "3" });
            return lstLocalidad;
        }
    }
}

