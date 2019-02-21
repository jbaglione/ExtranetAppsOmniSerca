using Newtonsoft.Json;
using NLog;
using ExtranetAppsOmniSerca.Helpers;
using ExtranetAppsOmniSerca.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Protocols;
using System.Text;

namespace ExtranetAppsOmniSerca.Controllers
{
    public class DireccionFormateada
    {
        public bool Valor { get; set; }

        private string calle;

        public string Calle
        {
            get
            {
                return calle;
            }
            set
            {
                calle = value.ToUpper();
            }
        }
        public string Altura { get; set; }
        public int LocalidadId { get; set; }
        public string LocalidadCodigo { get; set; }
        public string PartidoId { get; set; }
        public string LocalidadDesc { get; set; }
        public string CodPostal { get; set; }
        public string Longitud { get; set; }
        public string Provincia { get; set; }
        public string Latitud { get; set; }
        public bool GrabarCodigoPostal { get; set; }

        public List<string> Provincias
        {
            get
            {
                return new List<string>{
                                        //"Capital Federal"
                                        //,
                                        "Buenos Aires"
                                        , "Catamarca"
                                        , "Chaco"
                                        , "Chubut"
                                        , "Córdoba"
                                        , "Corrientes"
                                        , "Entre Ríos"
                                        , "Formosa"
                                        , "Jujuy"
                                        , "La Pampa"
                                        , "La Rioja"
                                        , "Mendoza"
                                        , "Misiones"
                                        , "Neuquén"
                                        , "Río Negro"
                                        , "Salta"
                                        , "San Juan"
                                        , "San Luis"
                                        , "Santa Cruz"
                                        , "Santa Fe"
                                        , "Santiago Del Estero"
                                        , "Tierra Del Fuego"
                                        , "Tucumán"
                                        };
            }
        }
    }
    public class MapaController : Controller
    {
        //TODO: WSLocalidades
        //public static List<Localidad> lstLocalidad = new List<Localidad>();

        //public MapaController()
        //{
        //    WSLocalidades.LocalidadesSoapClient wsClient = new WSLocalidades.LocalidadesSoapClient();
        //    wsClient.Open();
        //    DataSet ds = wsClient.GetLocalidadesSistema();
        //    wsClient.Close();
        //    foreach (DataRow item in ds.Tables[0].Rows)
        //        lstLocalidad.Add(new Localidad(item));
        //}

        //public JsonResult GetIDByDescripcionProvincia(string descripcion, string provincia)
        //{
        //    //List<Localidad> lstLocalidad = new List<Localidad>();
        //    WSLocalidades.LocalidadesSoapClient wsClient = new WSLocalidades.LocalidadesSoapClient();
        //    wsClient.Open();
        //    DataSet ds = wsClient.GetIDByDescripcionProvincia(descripcion, provincia);
        //    wsClient.Close();
        //    //foreach (DataRow item in ds.Tables[0].Rows)
        //    //    lstLocalidad.Add(new Localidad(item, "Descripcion", "LocalidadId"));

        //    return Json(Convert.ToInt32(ds.Tables[0].Rows[0]["LocalidadId"]), JsonRequestBehavior.AllowGet);
        //}

        ////public JsonResult GetIDByCodigoPostal(string codigoPostal)
        ////{
        ////    return Json(GetIDByCodigoPostalCustom(codigoPostal), JsonRequestBehavior.AllowGet);
        ////}

        //public int GetIDByCodigoPostalCustom(string codigoPostal, WSLocalidades.LocalidadesSoapClient wsClient = null, bool buscarSinLetras = false)
        //{
        //    bool closeClient = false;

        //    if (wsClient == null)
        //    {
        //        wsClient = new WSLocalidades.LocalidadesSoapClient();
        //        wsClient.Open();
        //        closeClient = true;
        //    }

        //    DataSet ds = wsClient.GetIDByCodigoPostal(codigoPostal);

        //    int localidadId = Convert.ToInt32(ds.Tables[0].Rows[0]["LocalidadId"]);
        //    if (localidadId == 0 && buscarSinLetras)
        //    {
        //        string codigoPostalNumber = Regex.Match(codigoPostal, @"\d+").Value;
        //        ds = wsClient.GetIDByCodigoPostal(codigoPostalNumber);
        //        localidadId = Convert.ToInt32(ds.Tables[0].Rows[0]["LocalidadId"]);
        //    }
        //    if (closeClient)
        //        wsClient.Close();

        //    return localidadId;
        //}

        //public JsonResult GetLocalidadById(string localidadId)
        //{
        //    return Json(lstLocalidad.Where(loc => loc.value == localidadId).FirstOrDefault(), JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult FormatearFecha(string direction)
        //{
        //    DireccionFormateada df = new DireccionFormateada();
        //    if (!string.IsNullOrEmpty(direction))
        //    {

        //        string[] arrayDireccion = direction.Split('$');
        //        string[] domicilio = arrayDireccion[2].Split(',');
        //        WSLocalidades.LocalidadesSoapClient conLocalidad = new WSLocalidades.LocalidadesSoapClient();
        //        conLocalidad.Open();

        //        DataSet ds;
        //        string calleAltura = "";
        //        string codPostalLocalidad = "";
        //        string pais;

        //        //int indicePais = 0;
        //        //for (int i = 0; i < domicilio.Length; i++)
        //        //{
        //        //    if (domicilio[i].Trim() == "Argentina")
        //        //        indicePais = i;
        //        //}

        //        switch (domicilio.Length)
        //        {
        //            case 5:
        //                calleAltura = domicilio[1].Trim();
        //                codPostalLocalidad = domicilio[0].Trim() + " " + domicilio[2].Trim();
        //                df.Provincia = domicilio[3].Trim();
        //                pais = domicilio[4].Trim();
        //                break;
        //            case 4:
        //                calleAltura = domicilio[0].Trim();
        //                codPostalLocalidad = domicilio[1].Trim();
        //                df.Provincia = domicilio[2].Trim();
        //                pais = domicilio[3].Trim();
        //                break;
        //            case 3:
        //                codPostalLocalidad = domicilio[0].Trim();
        //                df.Provincia = domicilio[1].Trim();
        //                pais = domicilio[2].Trim();

        //                calleAltura = domicilio[0];
        //                codPostalLocalidad = domicilio[1].Trim();
        //                pais = domicilio[2].Trim();

        //                break;
        //            case 2:
        //                df.Provincia = domicilio[0].Trim();
        //                pais = domicilio[1].Trim();
        //                break;
        //        }

        //        df.Provincia = df.Provincias.Contains(df.Provincia) ? df.Provincia : "Buenos Aires";
        //        //if ((domicilio.Length == 4))
        //        //{
        //        //    calleAltura = domicilio[0];
        //        //    codPostalLocalidad = domicilio[1];
        //        //    provincia = domicilio[2];
        //        //    pais = domicilio[3];
        //        //}
        //        //else
        //        //{
        //        //    calleAltura = domicilio[0];
        //        //    codPostalLocalidad = domicilio[1];
        //        //    provincia = "";
        //        //    pais = domicilio[2];
        //        //}

        //        if (codPostalLocalidad.Trim().Split(' ').Length > 1)
        //        {
        //            if (IsCodigoPostal(codPostalLocalidad.Trim().Split(' ')[0]))
        //            {
        //                df.CodPostal = codPostalLocalidad.Trim().Split(' ')[0];
        //                df.LocalidadId = GetIDByCodigoPostalCustom(df.CodPostal, conLocalidad, df.Provincia == "Buenos Aires");

        //                if (df.LocalidadId == 0)
        //                {
        //                    df.GrabarCodigoPostal = true;
        //                    ds = conLocalidad.GetIDByDescripcionProvincia(codPostalLocalidad.Replace(df.CodPostal, "").Trim(), df.Provincia);
        //                    df.LocalidadId = Convert.ToInt32(ds.Tables[0].Rows[0]["LocalidadId"]);
        //                }
        //            }
        //            else
        //            {
        //                ds = conLocalidad.GetIDByDescripcionProvincia(codPostalLocalidad.Trim(), df.Provincia);
        //                df.LocalidadId = Convert.ToInt32(ds.Tables[0].Rows[0]["LocalidadId"]);
        //                df.CodPostal = string.Empty;
        //            }
        //        }
        //        else
        //        {
        //            ds = conLocalidad.GetIDByDescripcionProvincia(codPostalLocalidad.Trim().Split(' ')[0], df.Provincia);
        //            df.LocalidadId = Convert.ToInt32(ds.Tables[0].Rows[0]["LocalidadId"]);
        //            df.CodPostal = string.Empty;
        //        }

        //        if (df.LocalidadId != 0)
        //        {
        //            var localidad = lstLocalidad.Where(loc => loc.value == df.LocalidadId.ToString()).FirstOrDefault();
        //            var lstLoc = lstLocalidad.Where(loc => loc.label.Contains("TUCU"));
        //            df.LocalidadDesc = localidad.label.ToUpper();
        //            df.LocalidadCodigo = Regex.Match(localidad.label, @"\(([^)]*)\)").Groups[1].Value;
        //            df.PartidoId = localidad.PartidoId;
        //        }

        //        if (!string.IsNullOrEmpty(calleAltura))
        //        {
        //            string[] calleAlt = calleAltura.Split(' ');
        //            string altura = calleAlt[(calleAlt.Count() - 1)];
        //            if (IsNumeric(altura))
        //            {
        //                df.Calle = calleAltura.Replace(altura, "");
        //                df.Altura = altura;
        //            }
        //            else
        //            {
        //                //df.Calle = calleAltura.Replace(altura, "");
        //                df.Calle = domicilio[0].ToUpper();
        //            }
        //        }


        //        df.Longitud = arrayDireccion[1];
        //        df.Latitud = arrayDireccion[0];
        //    }

        //    return Json(df, JsonRequestBehavior.AllowGet);
        //}

        //private bool IsNumeric(string str)
        //{
        //    float output;
        //    return float.TryParse(str, out output);
        //}
        //private bool IsCodigoPostal(string codigo)
        //{
        //    string codPost = codigo;// Regex.Match(codigo, @"\d+").Value;
        //    int i;
        //    int letra;
        //    if (codPost == "")
        //        return false;

        //    for (i = 1; (i <= codPost.Length); i++)
        //    {
        //        letra = (int)Convert.ToChar(codPost.Substring((i - 1), 1).ToUpper());
        //        if (i == 1 || i > 5)
        //        {
        //            if (letra < 65 || letra > 90)
        //                return false;
        //        }
        //        else if (i >= 2 && i <= 5)
        //        {
        //            if (("0123456789".IndexOf(codPost.Substring((i - 1), 1)) + 1) == 0)
        //                return false;
        //        }
        //    }

        //    return true;
        //}
    }
}