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
using ParamedicMedicosPrestaciones.Models;

namespace ExtranetAppsOmniSerca.Controllers
{
    public class LiquidacionesController : Controller
    {
        private Logger logger = LogManager.GetCurrentClassLogger();
        //
        // GET: /Liquidacion/

        public ActionResult Index(int empresa = 0)
        {
            ViewBag.Title = "Gestión de Liquidaciones";
            int usr = Convert.ToInt32(Session["usr_id"]);
            if (usr != 0 && validarUsuario(usr, empresa))
                return View();
            else
            {
                //Session.RemoveAll();
                return RedirectToAction("Index", "Error");
            }
        }

        public bool validarUsuario(long usr, int empresa)
        {
            try
            {
                WSTercerosLiquidaciones.TercerosLiquidacionesSoapClient wsClient = new WSTercerosLiquidaciones.TercerosLiquidacionesSoapClient();
                wsClient.Open();
                DataTable dtUsuario = wsClient.GetUsuarioValidacion(usr, empresa).Tables[0];
                wsClient.Abort();
                if (Convert.ToInt32(dtUsuario.Rows[0]["Acceso"]) == 0)
                    return false;
                else
                {
                    Session["usr_id"] = usr;
                    Session["empresa"] = empresa;
                    Session["UserName"] = dtUsuario.Rows[0]["NombreUsuario"].ToString();
                    Session["Acceso"] = Convert.ToInt32(dtUsuario.Rows[0]["Acceso"]);
                    //Session["page"] = "Liquidaciones";
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
            if (Session["usr_id"] == null || Convert.ToInt32(Session["usr_id"]) == 0)
                RedirectToAction("Index", "Error");
            return (Session["usr_id"] == null || Convert.ToInt32(Session["usr_id"]) == 0);
        }
        //JsonResult - ActionResult
        public JsonResult GetMoviles(long pPer, long pEst, long pTip)
        {
            //if (!ValidarSession())
            //    return null;
            WSTercerosLiquidaciones.TercerosLiquidacionesSoapClient wsClient = new WSTercerosLiquidaciones.TercerosLiquidacionesSoapClient();
            wsClient.Open();
            DataSet ds;
            if (Convert.ToInt32(Session["empresa"]) == 1)
                ds = wsClient.GetEmpresas(Convert.ToInt32(Session["usr_id"]), Convert.ToInt32(Session["Acceso"]), pPer, pEst, pTip);
            else
                ds = wsClient.GetMoviles(Convert.ToInt32(Session["usr_id"]), Convert.ToInt32(Session["Acceso"]), pPer, pEst);
            wsClient.Close();
            //hago la lista de objetos para retornar
            List<FiltroMoviles> lstMoviles = new List<FiltroMoviles>();
            foreach (DataRow dr in ds.Tables[0].Rows)
                lstMoviles.Add(new FiltroMoviles(dr["ID"].ToString(), dr["Nombre"].ToString()));

            return Json(lstMoviles, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetIncidentes(long pMov, long pPer, long pDia, long pEst) {
            try
            {
                logger.Info("GetIncidentes-wsClient");
                WSTercerosLiquidaciones.TercerosLiquidacionesSoapClient wsClient = new WSTercerosLiquidaciones.TercerosLiquidacionesSoapClient();
                logger.Info("GetIncidentes-Open");
                wsClient.Open();
                logger.Info("GetIncidentes-GetIncidentes");
                DataSet ds = wsClient.GetIncidentes(pMov, pPer, pDia, pEst);
                logger.Info("GetIncidentes-Close");
                wsClient.Close();

                List<Incidente> lstIncidentes = new List<Incidente>();
                logger.Info("GetIncidentes-foreach");
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    logger.Info("GetIncidentes-ITEM");
                    lstIncidentes.Add(new Incidente(item));
                }

                logger.Info("GetIncidentes-return");
                return Json(lstIncidentes, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error("GetIncidentes-" + ex.Message);
               throw;
            }
        }

        public JsonResult GetAsistencia(long pMov, long pPer)
        {
            List<Asistencia> lstAsistencias = new List<Asistencia>();
            WSTercerosLiquidaciones.TercerosLiquidacionesSoapClient wsClient = new WSTercerosLiquidaciones.TercerosLiquidacionesSoapClient();
            try
            {
                wsClient.Open();
                DataSet ds = wsClient.GetAsistencia(pMov);
                wsClient.Close();
                foreach (DataRow dr in ds.Tables[0].Rows)
                    lstAsistencias.Add(new Asistencia(dr));
            }
            catch (System.ServiceModel.FaultException ex)
            {
                logger.Error(ex.Message);
                wsClient.Abort();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                wsClient.Abort();
            }
            return Json(lstAsistencias, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetConformidad(string pItmLiq)
        {
            Conformidad oConformidad = null;
            WSTercerosLiquidaciones.TercerosLiquidacionesSoapClient wsClient = new WSTercerosLiquidaciones.TercerosLiquidacionesSoapClient();
            wsClient.Open();
            DataSet ds = wsClient.GetConformidad(pItmLiq);
            wsClient.Close();
            if (ds.Tables[0].Rows.Count > 0)
                oConformidad = new Conformidad(ds.Tables[0].Rows[0]);
            return Json(oConformidad, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetResumen(long pMov)
        {
            Resumen oResumen = new Resumen();
            Conformidad oConformidad = null;
            WSTercerosLiquidaciones.TercerosLiquidacionesSoapClient wsClient = new WSTercerosLiquidaciones.TercerosLiquidacionesSoapClient();
            wsClient.Open();
            DataSet ds = wsClient.GetResumen(pMov);
            wsClient.Close();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                switch (dr["GrupoId"].ToString())
                {
                    case "0":
                        oResumen.Productividad.Add(new ResumenItem(dr));
                        break;
                    case "1":
                        oResumen.Factura.Add(new ResumenItem(dr));
                        break;
                    case "2":
                        oResumen.Retenciones.Add(new ResumenItem(dr));
                        break;
                    case "3":
                        oResumen.Descuentos.Add(new ResumenItem(dr));
                        break;
                    case "4":
                        oResumen.Pagos.Add(new ResumenItem(dr));
                        break;
                    default:
                        break;
                }

            }
            return Json(oResumen, JsonRequestBehavior.AllowGet);
        }

        public string EsEmpresa()
        {
            return (Session["empresa"] == null) ? "" : Session["empresa"].ToString();
        }
        public string GetAcceso()
        {
            return (Session["acceso"] == null) ? "0" : Session["acceso"].ToString();
        }

        public JsonResult GetMotivosReclamos()
        {
            WSTercerosLiquidaciones.TercerosLiquidacionesSoapClient wsClient = new WSTercerosLiquidaciones.TercerosLiquidacionesSoapClient();
            wsClient.Open();
            DataSet ds = wsClient.GetMotivosReclamos();
            wsClient.Close();

            List<MotivoReclamo> lstMotivoReclamo = new List<MotivoReclamo>();
            foreach (DataRow item in ds.Tables[0].Rows)
                lstMotivoReclamo.Add(new MotivoReclamo(item));

            return Json(lstMotivoReclamo, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPeriodos()
        {
            WSTercerosLiquidaciones.TercerosLiquidacionesSoapClient wsClient = new WSTercerosLiquidaciones.TercerosLiquidacionesSoapClient();
            wsClient.Open();
            DataSet ds = wsClient.GetPeriodos(Convert.ToInt32(EsEmpresa()));
            wsClient.Close();

            List<Periodo> lstPeriodo = new List<Periodo>();
            //List<string> lstPeriodoStr = new List<string>();
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                Periodo per = new Periodo(item);
                lstPeriodo.Add(per);
                //lstPeriodoStr.Add(per.PeriodoStr);
            }
            return Json(lstPeriodo, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetIncidenteDetalle(string pItmLiq, long pLiqId, long pInc)
        {
            WSTercerosLiquidaciones.TercerosLiquidacionesSoapClient wsClient = new WSTercerosLiquidaciones.TercerosLiquidacionesSoapClient();
            wsClient.Open();
            DataSet dsDetalle = wsClient.GetIncidenteDetalle(pItmLiq);
            DataSet dsCalculo = wsClient.GetIncidenteCalculo(pLiqId, pInc);
            wsClient.Close();
            Models.IncidenteDetalle oIncidenteDetalle = null;
            if (dsDetalle.Tables[0].Rows.Count > 0)
                oIncidenteDetalle = new IncidenteDetalle(dsDetalle.Tables[0].Rows[0], dsCalculo.Tables[0]);
            return Json(oIncidenteDetalle, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SetConformidad(string pItm, long pRpl, long pCnf, long pMot, decimal pDif, decimal pLiq, decimal pNue, string pObs)
        {
            long pUsr = Convert.ToInt32(Session["usr_id"]);
            WSTercerosLiquidaciones.TercerosLiquidacionesSoapClient wsClient = new WSTercerosLiquidaciones.TercerosLiquidacionesSoapClient();
            string[] array = pItm.Split(new string[] { "||" }, StringSplitOptions.None);
            long pLiqId = Convert.ToInt64(array[0]);
            long pFec = Convert.ToInt64(array[1]);
            string pNro = array[2];
            wsClient.Open();
            DataSet ds = wsClient.SetConformidad(pLiqId, pFec, pNro, pRpl, pCnf, pMot, pDif, pLiq, pNue, pObs, pUsr);
            wsClient.Close();
            ConformidadResult oConformidadResult = null;
            if (ds.Tables[0].Rows.Count > 0)
                oConformidadResult = new ConformidadResult(ds.Tables[0].Rows[0]);
            return Json(oConformidadResult, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SetRespuesta(string pItm, long pSta, string pRta)
        {
            long pUsr = Convert.ToInt32(Session["usr_id"]);
            WSTercerosLiquidaciones.TercerosLiquidacionesSoapClient wsClient = new WSTercerosLiquidaciones.TercerosLiquidacionesSoapClient();
            string[] array = pItm.Split(new string[] { "||" }, StringSplitOptions.None);
            long pLiqId = Convert.ToInt64(array[0]);
            long pFec = Convert.ToInt64(array[1]);
            string pNro = array[2];
            wsClient.Open();
            DataSet ds = wsClient.SetRespuesta(pLiqId, pFec, pNro, pSta, pRta, pUsr);
            wsClient.Close();
            ConformidadResult oConformidadResult = null;
            if (ds.Tables[0].Rows.Count > 0)
                oConformidadResult = new ConformidadResult(ds.Tables[0].Rows[0]);
            return Json(oConformidadResult, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetResumenDetalle(long pLiqId, long pLiqMovId, int link)
        {
            DataSet ds;
            List<ResumenItemDetalle> oList = new List<ResumenItemDetalle>();
            WSTercerosLiquidaciones.TercerosLiquidacionesSoapClient wsClient = new WSTercerosLiquidaciones.TercerosLiquidacionesSoapClient();
            wsClient.Open();
            switch (link)
            {
                case 0://No tiene link
                    ds = null;
                    break;
                case 1://Productividad
                    ds = wsClient.GetResumenProductividad(pLiqMovId);
                    break;
                case 2://Premios
                    ds = wsClient.GetResumenPremios(pLiqMovId);
                    break;
                case 3://Insumos
                    ds = wsClient.GetResumenInsumos(pLiqId);
                    break;
                case 4://Otros Descuentos
                    ds = wsClient.GetResumenOtrosDescuentos(pLiqId);
                    break;
                default://No está parametrizado
                    ds = null;
                    break;
            }
            wsClient.Close();
            if (ds != null)
                foreach (DataRow dr in ds.Tables[0].Rows)
                    oList.Add(new ResumenItemDetalle(dr));

            return Json(oList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Reliquidar(long pLiqId)
        {
            WSTercerosLiquidaciones.TercerosLiquidacionesSoapClient wsClient = new WSTercerosLiquidaciones.TercerosLiquidacionesSoapClient();
            wsClient.Open();
            long lReturn = wsClient.Reliquidar(pLiqId);
            wsClient.Close();
            return Json(lReturn, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UploadOrdenServicio(HttpPostedFileBase file, string ArchivoOrden, long pLiqId, long IncidenteId)
        {
            MemoryStream ms = new MemoryStream();

            file.InputStream.CopyTo(ms);
            file.InputStream.Position = ms.Position = 0;

            byte[] fileData = null;

            using (var binaryReader = new BinaryReader(ms))
                fileData = binaryReader.ReadBytes(Request.Files[0].ContentLength);

            if( !(Helper.GetImageFormat(fileData) == Helper.ImageFormat.jpeg))
                return Json(false, JsonRequestBehavior.AllowGet);


            Session["ArchivoOrden"] = "";
            Session["Error"] = "";
            if (string.IsNullOrEmpty(ArchivoOrden)) //si esta vacio es un alta (Subir archivo y llamar al método)
            {
                long pUsr = Convert.ToInt32(Session["usr_id"]);
                WSTercerosLiquidaciones.TercerosLiquidacionesSoapClient wsClient = new WSTercerosLiquidaciones.TercerosLiquidacionesSoapClient();
                wsClient.Open();
                DataSet ds = wsClient.SetOrdenServicio(pLiqId, IncidenteId, pUsr, 1);
                wsClient.Close();
                if (ds.Tables[0].Rows[0]["Resultado"].ToString() == "0")
                {
                    Session["Error"] = ds.Tables[0].Rows[0]["AlertaError"].ToString();
                    return Json(ds.Tables[0].Rows[0]["AlertaError"].ToString(), JsonRequestBehavior.AllowGet);
                }
                ArchivoOrden = ds.Tables[0].Rows[0]["Archivo"].ToString();
            }
            //Subir el archivo
            string path = "~/liquidacionesImagenes/";
            if (!Directory.Exists(Server.MapPath(path) + ArchivoOrden.Substring(0, ArchivoOrden.LastIndexOf('\\'))))
                Directory.CreateDirectory(Server.MapPath(path) + ArchivoOrden.Substring(0, ArchivoOrden.LastIndexOf('\\')));
            string fileName = ArchivoOrden.Substring(ArchivoOrden.LastIndexOf('\\') + 1);
            file.SaveAs(Path.Combine(Server.MapPath(path) + ArchivoOrden.Substring(0, ArchivoOrden.LastIndexOf('\\')), fileName));
            Session["ArchivoOrden"] = ArchivoOrden;
            return Json(ArchivoOrden, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSession(string pVar)
        {
            return Json(Session[pVar].ToString(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetOrdenServicio(long pLiqId, long IncidenteId)
        {
            string ServerFileSystem = Helpers.Config.ServerFileSystem;
            DirectoryInfo oDirectoryInfo = new DirectoryInfo("\\\\" + ServerFileSystem + "\\shamanadj$\\liquidaciones\\");
            FileInfo[] oFileInfo = oDirectoryInfo.GetFiles(pLiqId + "_" + IncidenteId + ".*");
            return null;
        }
    }
}
