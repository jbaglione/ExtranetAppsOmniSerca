using NLog;
using ExtranetAppsOmniSerca.Helpers;
using ExtranetAppsOmniSerca.Models;
using System;
using System.Data;
using System.IO;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Protocols;
using System.Collections.Generic;
using ExtranetAppsOmniSerca.ViewModels;
using System.Text;
using Ionic.Zip;

namespace ExtranetAppsOmniSerca.Controllers
{
    public class EmpleadosLiquidacionesController : Controller
    {
        private Logger logger = LogManager.GetCurrentClassLogger();
        //
        // GET: /Liquidacion/

        public ActionResult Index()
        {
            ViewBag.Title = "Gestión de Empleados";

            int usr = Convert.ToInt32(Session["usr_id"]);
            if (usr != 0 && validarUsuario(usr))
                return View();
            else
            {
                //Session.RemoveAll();
                return RedirectToAction("Index", "Error");
            }
        }



        public bool validarUsuario(long usr)
        {
            try
            {
                WSEmpleadosLiquidaciones.EmpleadosLiquidacionesSoapClient wsClient = new WSEmpleadosLiquidaciones.EmpleadosLiquidacionesSoapClient();
                wsClient.Open();
                DataTable dtUsuario = wsClient.GetUsuarioValidacion(usr).Tables[0];
                wsClient.Abort();
                
               if (Convert.ToInt32(dtUsuario.Rows[0]["Acceso"]) == 0)
                    return false;
                else
                {
                    Session["usr_id"] = usr;
                    //TODO: Validar admin
                    Session["admin"] = true;
                    Session["UserName"] = dtUsuario.Rows[0]["NombreUsuario"].ToString();
                    Session["Acceso"] = Convert.ToInt32(dtUsuario.Rows[0]["Acceso"]);
                    //Session["page"] = "EmpleadosLiquidaciones";
                    return true;
                }
            }
            catch (SoapException ex)
            {
                logger.Error(ex);
            }
            catch (FaultException ex)
            {
                logger.Error(ex);
                logger.Error(ServiceHelper.UltimaRespuesta);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return false;
        }

        private bool ValidarSession()
        {
            if (Session["usr_id"] == null)
                RedirectToAction("Index", "Error");
            return (Session["usr_id"] == null);
        }

        public JsonResult GetEmpleados(long pPer, long pEst)
        {
            try
            {
                WSEmpleadosLiquidaciones.EmpleadosLiquidacionesSoapClient wsClient = new WSEmpleadosLiquidaciones.EmpleadosLiquidacionesSoapClient();
                wsClient.Open();
                DataSet ds = wsClient.GetEmpleados(Convert.ToInt32(Session["usr_id"]), Convert.ToInt32(Session["Acceso"]), pPer, pEst);
                wsClient.Close();

                List<EmpleadoLiquidacion> lstEmpleadosL = new List<EmpleadoLiquidacion>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                    lstEmpleadosL.Add(new EmpleadoLiquidacion { LiqId = Convert.ToInt64(dr["ID"]), Nombre = dr["Nombre"].ToString() });

                return Json(lstEmpleadosL, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error("Fallo la solicitud de lista de empleados. " + ex.Message);
                throw;
            }
        }

        public JsonResult GetPeriodos()
        {
            WSEmpleadosLiquidaciones.EmpleadosLiquidacionesSoapClient wsClient = new WSEmpleadosLiquidaciones.EmpleadosLiquidacionesSoapClient();
            wsClient.Open();
            DataSet ds;
            ds = wsClient.GetPeriodos();
            wsClient.Close();

            List<FiltroPeriodos> lstPeriodos = new List<FiltroPeriodos>();
            foreach (DataRow dr in ds.Tables[0].Rows)
                lstPeriodos.Add(new FiltroPeriodos(dr["Periodo"].ToString(), dr["PeriodoStr"].ToString()));

            return Json(lstPeriodos, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLiquidacionDetalle(long pLiqId, long pEstado)
        {
            try
            {
                logger.Info("GetLiquidacionDetalle-wsClient");
                WSEmpleadosLiquidaciones.EmpleadosLiquidacionesSoapClient wsClient = new WSEmpleadosLiquidaciones.EmpleadosLiquidacionesSoapClient();
                logger.Info("GetLiquidacionDetalle-Open");
                wsClient.Open();
                logger.Info("GetLiquidacionDetalle-GetLiquidacionDetalle");
                DataSet ds = wsClient.GetLiquidacionDetalle(pLiqId, pEstado);
                logger.Info("GetLiquidacionDetalle-Close");
                wsClient.Close();

                List<EmpleadoLiquidacionDetalle> lstLiquidaciones = new List<EmpleadoLiquidacionDetalle>();
                logger.Info("GetLiquidacionDetalle-foreach");
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    logger.Info("GetLiquidacionDetalle-ITEM");
                    lstLiquidaciones.Add(new EmpleadoLiquidacionDetalle(item));
                }

                logger.Info("GetLiquidacionDetalle-return");
                return Json(lstLiquidaciones, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error("GetLiquidacionDetalle-" + ex.Message);
               throw;
            }
        }

        public JsonResult GetConformidad(string pItmLiq)
        {
            try
            {
                ConformidadFichada oConformidad = null;
                WSEmpleadosLiquidaciones.EmpleadosLiquidacionesSoapClient wsClient = new WSEmpleadosLiquidaciones.EmpleadosLiquidacionesSoapClient();
                wsClient.Open();
                DataSet ds = wsClient.GetConformidad(pItmLiq);
                wsClient.Close();
                if (ds.Tables[0].Rows.Count > 0)
                    oConformidad = new ConformidadFichada(ds.Tables[0].Rows[0]);

                //oConformidad = new ConformidadFichada();
                //oConformidad.SetFakeConformidadFichada();

                return Json(oConformidad, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public JsonResult GetResumen(long pMov)
        {
            ResumenLiquidacionEmpleados oResumen = new ResumenLiquidacionEmpleados();
            WSEmpleadosLiquidaciones.EmpleadosLiquidacionesSoapClient wsClient = new WSEmpleadosLiquidaciones.EmpleadosLiquidacionesSoapClient();
            wsClient.Open();
            DataSet ds = wsClient.GetResumen(pMov);
            wsClient.Close();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                switch (dr["Grupo"].ToString())
                {
                    case "1":
                        oResumen.Trabajado.Add(new ResumenLiquidacionEmpleadosItem(dr));
                        break;
                    case "2":
                        oResumen.Esperado.Add(new ResumenLiquidacionEmpleadosItem(dr));
                        break;
                    case "3":
                        oResumen.Incumplimientos.Add(new ResumenLiquidacionEmpleadosItem(dr));
                        break;
                    default:
                        break;
                }
            }

            return Json(oResumen, JsonRequestBehavior.AllowGet);
        }

        public string EsAdmin()
        {
            return (Session["admin"] == null) ? "" : Session["admin"].ToString();
        }
        public string GetAcceso()
        {
            return (Session["acceso"] == null) ? "0" : Session["acceso"].ToString();
        }

        public JsonResult GetMotivosReclamos()
        {
            List<MotivoReclamo> lstMotivoReclamo = new List<MotivoReclamo>();

            WSEmpleadosLiquidaciones.EmpleadosLiquidacionesSoapClient wsClient = new WSEmpleadosLiquidaciones.EmpleadosLiquidacionesSoapClient();
            wsClient.Open();
            DataSet ds = wsClient.GetMotivosReclamos();
            wsClient.Close();

            foreach (DataRow item in ds.Tables[0].Rows)
                lstMotivoReclamo.Add(new MotivoReclamo(item));

            return Json(lstMotivoReclamo, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SetConformidad(string pItm, long pCnf, long pMot, string pHEnt, string pMEnt, string pHSal, string pMSal, string pObs)
        {
            ConformidadResult oConformidadResult = null;
            long pUsr = Convert.ToInt32(Session["usr_id"]);
            WSEmpleadosLiquidaciones.EmpleadosLiquidacionesSoapClient wsClient = new WSEmpleadosLiquidaciones.EmpleadosLiquidacionesSoapClient();
            string[] array = pItm.Split(new string[] { "||" }, StringSplitOptions.None);
            //long pLiqId = Convert.ToInt64(array[0]);
            //string pItmID = array[1];
            wsClient.Open();
            DataSet ds = wsClient.SetConformidad(pItm, pCnf, pMot, pHEnt, pMEnt, pHSal, pMSal, pObs, pUsr);
            wsClient.Close();

            if (ds.Tables[0].Rows.Count > 0)
                oConformidadResult = new ConformidadResult(ds.Tables[0].Rows[0]);
            return Json(oConformidadResult, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SetRespuesta(string pItm, long pSta, string pRta)
        {
            ConformidadResult oConformidadResult = null;
            long pUsr = Convert.ToInt32(Session["usr_id"]);
            WSEmpleadosLiquidaciones.EmpleadosLiquidacionesSoapClient wsClient = new WSEmpleadosLiquidaciones.EmpleadosLiquidacionesSoapClient();
            wsClient.Open();
            DataSet ds = wsClient.SetRespuesta(pItm, pSta, pRta, pUsr);
            wsClient.Close();

            if (ds.Tables[0].Rows.Count > 0)
                oConformidadResult = new ConformidadResult(ds.Tables[0].Rows[0]);
            return Json(oConformidadResult, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Reliquidar(long pLiqId)
        {
            long lReturn = 0;
            long pUsr = Convert.ToInt32(Session["usr_id"]);
            WSEmpleadosLiquidaciones.EmpleadosLiquidacionesSoapClient wsClient = new WSEmpleadosLiquidaciones.EmpleadosLiquidacionesSoapClient();
            wsClient.Open();
            lReturn = wsClient.Reliquidar(pLiqId, pUsr);
            wsClient.Close();
            return Json(lReturn, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSession(string pVar)
        {
            return Json(Session[pVar].ToString(), JsonRequestBehavior.AllowGet);
        }
    }
}
