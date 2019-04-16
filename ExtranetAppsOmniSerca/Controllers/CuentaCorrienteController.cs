using NLog;
using ExtranetAppsOmniSerca.Helpers;
using ExtranetAppsOmniSerca.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Protocols;

namespace ExtranetAppsOmniSerca.Controllers
{
    public class CuentaCorrienteController : Controller
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

        public ActionResult Index()
        {
            ViewBag.Title = "Gestión de Cuentas Corrientes";

            int usr = Convert.ToInt32(Session["usr_id"]);
            if (usr != 0 && validarUsuario(usr))
                return View();
            else
            {
                //Session.RemoveAll();
                return RedirectToAction("Index", "Error");
            }
        }

        [HttpPost]
        public ActionResult Guardar(Factura valor)
        {
            string error;
            var result = ServiceHelper.SetComprobante(valor, Convert.ToInt32(Session["usr_id"]), out error);
            return Json(new { result, error }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GuardarV2(Factura comprobante)
        {
            string error = "";
            bool result = true;
            if (Convert.ToInt32(Session["usr_id"]) <= 0)
            {
                result = false;
                error = "Sessión vencida";
            }
            if (comprobante.NroOP == null)
                result = true;
            else
                result = ServiceHelper.SetComprobanteV2(comprobante, Convert.ToInt32(Session["usr_id"]), out error);

            if (result)
            {
                try
                {
                    NameValueCollection appSettings = System.Web.Configuration.WebConfigurationManager.AppSettings;
                    string To = appSettings.Get("CtaCteMailTo");
                    string Subject = string.Format(appSettings.Get("CtaCteMailSubject"), comprobante.ProveedorTangoId, comprobante.Tipo, comprobante.Sucursal, comprobante.Numero);
                    string Body = appSettings.Get("CtaCteMailBody");
                    Body += "\n" + "Nro OP: " + comprobante.NroOP;
                    Body += "\n" + "Tipo y Nro Comprobante: " + comprobante.Tipo + " " + comprobante.Sucursal + "-" + comprobante.Numero;
                    Body += "\n" + "Fecha Emisión: " + comprobante.Fecha;
                    Body += "\n" + "Monto: $" + comprobante.Monto;
                    Body += "\n" + "Código Comprobante Tango: " + comprobante.ProveedorTangoId;
                    Body += "\n" + "CAE: " + comprobante.CAE;
                    List<String> PathFiles = new List<String>();
                    //Mover archivos a carpeta fija y adjuntar al correo
                    string sourceFile = "";
                    string destinationFile = "";
                    if (comprobante.pdf != null){
                        sourceFile = Path.Combine(Server.MapPath("~/Shamanadj/cuentascorrientes/tempCtaCte/"), $"{Session["usr_id"]}_{comprobante.ProveedorTangoId}_PDF_{Path.GetExtension(comprobante.pdf)}");
                        destinationFile = Path.Combine(Server.MapPath("~/Shamanadj/cuentascorrientes/"), $"{Session["usr_id"]}_{comprobante.ProveedorTangoId}_PDF_{comprobante.Tipo}_{comprobante.Sucursal}{comprobante.Numero}{Path.GetExtension(comprobante.pdf)}");
                        System.IO.File.Move(sourceFile, destinationFile);
                        PathFiles.Add(destinationFile);
                    }
                    if (comprobante.detalle != null)
                    {
                        sourceFile = Path.Combine(Server.MapPath("~/Shamanadj/cuentascorrientes/tempCtaCte/"), $"{Session["usr_id"]}_{comprobante.ProveedorTangoId}_Detalle_{Path.GetExtension(comprobante.pdf)}");
                        destinationFile = Path.Combine(Server.MapPath("~/Shamanadj/cuentascorrientes/"), $"{Session["usr_id"]}_{comprobante.ProveedorTangoId}_Detalle_{comprobante.Tipo}_{comprobante.Sucursal}{comprobante.Numero}{Path.GetExtension(comprobante.pdf)}");
                        System.IO.File.Move(sourceFile, destinationFile);
                        PathFiles.Add(destinationFile);
                    }
                    if (comprobante.venta != null)
                    {
                        sourceFile = Path.Combine(Server.MapPath("~/Shamanadj/cuentascorrientes/tempCtaCte/"), $"{Session["usr_id"]}_{comprobante.ProveedorTangoId}_Venta_{Path.GetExtension(comprobante.pdf)}");
                        destinationFile = Path.Combine(Server.MapPath("~/Shamanadj/cuentascorrientes/"), $"{Session["usr_id"]}_{comprobante.ProveedorTangoId}_Venta_{comprobante.Tipo}_{comprobante.Sucursal}{comprobante.Numero}{Path.GetExtension(comprobante.pdf)}");
                        System.IO.File.Move(sourceFile, destinationFile);
                        PathFiles.Add(destinationFile);
                    }
                    if (comprobante.cabecera != null)
                    {
                        sourceFile = Path.Combine(Server.MapPath("~/Shamanadj/cuentascorrientes/tempCtaCte/"), $"{Session["usr_id"]}_{comprobante.ProveedorTangoId}_Cabecera_{Path.GetExtension(comprobante.pdf)}");
                        destinationFile = Path.Combine(Server.MapPath("~/Shamanadj/cuentascorrientes/"), $"{Session["usr_id"]}_{comprobante.ProveedorTangoId}_Cabecera_{comprobante.Tipo}_{comprobante.Sucursal}{comprobante.Numero}{Path.GetExtension(comprobante.pdf)}");
                        System.IO.File.Move(sourceFile, destinationFile);
                        PathFiles.Add(destinationFile);
                    }
                    EmailHelpers.Send(To, Subject, Body, PathFiles);
                }
                catch (Exception ex)
                {
                    logger.Error("Fallo al enviar el email. " + ex.Message);
                }
            }
            return Json(new { result, error }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UploadPdf(HttpPostedFileBase file, string id)
        {
            string path = Server.MapPath("~/Shamanadj/cuentascorrientes/tempCtaCte/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            file.SaveAs(Path.Combine(path, $"{Session["usr_id"]}_{id}_PDF_{Path.GetExtension(file.FileName)}"));
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UploadCabecera(HttpPostedFileBase file, string id)
        {
            string path = Server.MapPath("~/Shamanadj/cuentascorrientes/tempCtaCte/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            file.SaveAs(Path.Combine(Server.MapPath("~/Shamanadj/cuentascorrientes/tempCtaCte/"), $"{Session["usr_id"]}_{id}_Cabecera_{Path.GetExtension(file.FileName)}"));
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UploadDetalle(HttpPostedFileBase file, string id)
        {
            string path = Server.MapPath("~/Shamanadj/cuentascorrientes/tempCtaCte/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            file.SaveAs(Path.Combine(Server.MapPath("~/Shamanadj/cuentascorrientes/tempCtaCte/"), $"{Session["usr_id"]}_{id}_Detalle_{Path.GetExtension(file.FileName)}"));
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UploadVenta(HttpPostedFileBase file, string id)
        {
            string path = Server.MapPath("~/Shamanadj/cuentascorrientes/tempCtaCte/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            file.SaveAs(Path.Combine(Server.MapPath("~/Shamanadj/cuentascorrientes/tempCtaCte/"), $"{Session["usr_id"]}_{id}_Venta_{Path.GetExtension(file.FileName)}"));
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCuentaCorriente(long usr_id = 0)
        {
            if (usr_id == 0)
                usr_id = Convert.ToInt32(Session["usr_id"]);
            var result = ServiceHelper.getCuentaCorriente(usr_id);
            //var result = ServiceHelper.getCuentaCorriente(974);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public bool validarUsuario(long usr)
        {
            //TODO: WS no existe
            //try
            //{
            //    var wsClient = ServiceHelper.GetProveedoresCuentasWS();
            //    wsClient.Open();
            //    DataTable dtUsuario = wsClient.GetUsuarioValidacion(usr).Tables[0];
            //    wsClient.Abort();
            //    // 1,2 receptor
            //    // 3 administrador
            //    //dtUsuario.Rows[0]["Acceso"] = 3;
            //    if (Convert.ToInt32(dtUsuario.Rows[0]["Acceso"]) == 0)
            //    {
            //        return false;
            //    }
            //    else
            //    {
            //        Session["usr_id"] = usr;
            //        Session["UserName"] = dtUsuario.Rows[0]["NombreUsuario"].ToString();
            //        Session["Acceso"] = Convert.ToInt32(dtUsuario.Rows[0]["Acceso"]);
            //        Session["FacturaSinOP"] = Convert.ToInt32(dtUsuario.Rows[0]["FacturaSinOP"]);
            //        Session["ProveedorTangoDefaultId"] = dtUsuario.Rows[0]["ProveedorTangoDefaultId"].ToString();
            //        return true;
            //    }
            //}
            //catch (SoapException ex)
            //{
            //    logger.Error(ex);
            //}
            //catch (FaultException ex)
            //{
            //    logger.Error(ex);
            //    logger.Error (ServiceHelper.UltimaRespuesta);
            //}
            //catch (Exception ex)
            //{
            //    logger.Error(ex);
            //}
            return false;

        }

        public string GetAccesoPlus()
        {
            return (Session["acceso"] == null) ? "0" : Session["acceso"].ToString() + ";" + Session["FacturaSinOP"].ToString() + ";" + Session["ProveedorTangoDefaultId"].ToString();
            //< s:element name = "UsuarioId" type = "s:long" minOccurs = "0" />
            //< s:element name = "NombreProveedor" type = "s:string" minOccurs = "0" />
        }

        public ActionResult GetProveedores()
        {
            var result = ServiceHelper.getProveedores();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCertificados(string OP, string tipoComprobante)
        {
            try
            {
                WSShamanFecae.Certificado oTipoCertificado;
                switch (tipoComprobante.ToUpper())
                {
                    case "ARBA":
                        oTipoCertificado = WSShamanFecae.Certificado.crtArba;
                        break;
                    case "AGIP":
                        oTipoCertificado = WSShamanFecae.Certificado.crtAgip;
                        break;
                    case "CAJAPREVISIONAL":
                        oTipoCertificado = WSShamanFecae.Certificado.crtCajaPrevisional;
                        break;
                    case "GANANCIAS":
                        oTipoCertificado = WSShamanFecae.Certificado.crtGanancias;
                        break;
                    case "IVA":
                        oTipoCertificado = WSShamanFecae.Certificado.crtIVA;
                        break;
                    default:
                        return null;
                }
                WSShamanFecae.WSShamanFECAESoapClient client = new WSShamanFecae.WSShamanFECAESoapClient();
                var result = client.GetCertificadoRetencion_Tango(OP, oTipoCertificado);
                if (result == null)
                    return Json("Error al obtener comprobante", JsonRequestBehavior.AllowGet);

                return File(result, System.Net.Mime.MediaTypeNames.Application.Pdf, "comprobante.pdf");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

    }
}
