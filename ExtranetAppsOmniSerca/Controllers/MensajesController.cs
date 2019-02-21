using Newtonsoft.Json;
using NLog;
using ExtranetAppsOmniSerca.Helpers;
using ExtranetAppsOmniSerca.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Protocols;

namespace ExtranetAppsOmniSerca.Controllers
{
    public class MensajesController : Controller
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

        public ActionResult Index()
        {
            return PartialView();
        }
		public ActionResult Gestion()
		{
			return PartialView();
		}
		public ActionResult Borrar(string id)
		{
			long mensaje=long.Parse(id);
            try
            {
                var wsClient = ServiceHelper.GetNotificacionesWS();
                wsClient.Open();

                //var result = wsClient.DeleteMensajeUsuarios(mensaje);
                var result = wsClient.DeleteMensaje(mensaje);
                wsClient.Abort();
            }
            catch (Exception)
            {

                throw;
            }
			return Json("OK", JsonRequestBehavior.AllowGet);
		}

        private bool ValidarSession()
        {
            if (Session["usr_id"] == null || Convert.ToInt32(Session["usr_id"]) == 0)
                RedirectToAction("Index", "Error");
            return (Session["usr_id"] == null || Convert.ToInt32(Session["usr_id"]) == 0);
        }

        public ActionResult Edicion(string id, string padre)
		{
            try
            {
                if(ValidarSession())
                    return RedirectToAction("Index", "Error");

                //Session.Timeout = 40;
                //this.HttpContext.Session.Timeout; // ASP.NET MVC controller
                //Page.Session.Timeout // ASP.NET Web Forms code-behind
                //HttpContext.Current.Session.Timeout // Elsewhere

                var wsClient = ServiceHelper.GetNotificacionesWS();
                Session["page"] = padre;
                wsClient.Open();

                var model = new Notificacion();
                model.UsuarioList = new List<Usuario>();
                model.Grupos = new List<GrupoUsuario>();
                if (!string.IsNullOrEmpty(id) && id != "0")
                {
                    var result = (List<Notificacion>)this.Session["NotificacionesEnviadas"];
                    model = result.Where(i => i.Id.ToString() == id).First();
                    if (model.Id > 0)
                        model.Mensaje = wsClient.GetMensaje(model.Id);
                    model.UsuarioList = new List<Usuario>();
                    model.Grupos = new List<GrupoUsuario>();

                    DataSet dsNotificados = wsClient.GetUsuariosNotificados(model.Id);

                    foreach (DataRow dtRow in dsNotificados.Tables[0].Rows)
                    {
                        model.UsuarioList.Add(new Usuario(dtRow));
                    }
                }

                DataTable dtGrupos;
                switch (Session["page"].ToString().ToLower())
                {
                    case "medicos":
                        dtGrupos = wsClient.GetGruposUsuarios().Tables[0];
                        foreach (DataRow dtRow in dtGrupos.Rows)
                            model.Grupos.Add(new GrupoUsuario(dtRow));
                        break;
                    case "empleadosliquidaciones":
                        dtGrupos = wsClient.GetGruposGerencias().Tables[0];
                        foreach (DataRow dtRow in dtGrupos.Rows)
                            model.Grupos.Add(new GrupoUsuario(dtRow));
                        break;
                    case "liquidaciones":
                        model.Grupos.Add(new GrupoUsuario(0, "Prestadores de Móviles"));
                        model.Grupos.Add(new GrupoUsuario(1, "Empresas Prestadoras"));
                        model.Grupos.Add(new GrupoUsuario(2, "Empresas del Interior"));
                        break;
                    default:
                        model = null;
                        break;
                }

                wsClient.Abort();

                return PartialView(model);
            }
            catch (Exception ex)
            {
                throw;
            }
		}

        [HttpPost]
		public ActionResult Guardar(string valor, string mensajeEnriquecido)
		{
            //this.HttpContext.Session.Timeout = 20;

            var error = "";
            var noti = JsonConvert.DeserializeObject<Notificacion>(valor);

            string mensajeEnriquecidoDecodificado = Helper.decodingAtob(mensajeEnriquecido);

            var wsClient = ServiceHelper.GetNotificacionesWS();
            DataSet dsResultado;
            
            if (ValidarSession())
                return RedirectToAction("Index", "Error");

            try
            {
                wsClient.Open();
                dsResultado = wsClient.SetMensajeV2(noti.Id, mensajeEnriquecidoDecodificado, noti.Mensaje, Convert.ToInt32(Session["usr_id"]), noti.Mail ? 1 : 0);
                noti.Id = Convert.ToInt32(dsResultado.Tables[0].Rows[0]["RowID"]);

                if(noti.Id <= 0)
                    return Json(dsResultado.Tables[0].Rows[0]["Resultado"].ToString());

                dsResultado = wsClient.SetMensajeUsuarios(noti.Id, string.Join(";", noti.Usuarios));

                var response = dsResultado.Tables[0].DataTableToList<SetMensajeResult>();
                if (response.Any(i => i.Resultado == 0))
                {
                    error = $"No se pudo entregar el mensaje a los siguientes destinatarios: <BR> {string.Join("<BR>", response.Where(i => i.Resultado == 0).Select(i=>$"{i.Nombre} {i.DescripcionError}"))}";
                    return Json(error);
                }
			    wsClient.Abort();
                return Json("OK");
            }
            catch (SoapException ex)
            {
                logger.Error(ex);
                return Json("Error de servicio.");
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return Json("Error");
            }
        }

        public ActionResult PopupEdicion(string id)
		{

			return PartialView();
		}

        public ActionResult Ver(string id)
		{
			var wsClient = ServiceHelper.GetNotificacionesWS();
			wsClient.Open();
			DataSet dsResultado = wsClient.SetEstadoMensajeUsuario(Convert.ToInt32(id), Convert.ToInt32(Session["usr_id"]), (int)Notificacion.Estados.Leido);
            ViewBag.Mensaje = wsClient.GetMensaje(Convert.ToInt64(id));
            wsClient.Close();
            var result = (List<Notificacion>)this.Session["Notificaciones"];
			var item = result.Where(i => i.Id.ToString() == id).First();
			ViewBag.Fecha = item.FechaConFormato;
            //ViewBag.Mensaje = item.Mensaje;
			return PartialView();
		}

        public ActionResult PopupVer(string id)
		{
			return PartialView();
		}

        public ActionResult GetMensaje(long pMsgId)
        {
            WSNotificaciones.NotificacionesSoapClient wsClient = new WSNotificaciones.NotificacionesSoapClient();
            wsClient.Open();
            string sReturn = wsClient.GetMensaje(pMsgId);
            wsClient.Close();
            return Json(sReturn, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEnviados()
		{
			var wsClient = ServiceHelper.GetNotificacionesWS();
  		wsClient.Open();
			DataSet dsResultado = wsClient.GetMensajesEmitidos(Convert.ToInt32(Session["usr_id"]));
			wsClient.Abort();

			var result = new List<Notificacion>();
			foreach (DataRow item in dsResultado.Tables[0].Rows)
			{
				result.Add(new Notificacion(item, true));
			}
			this.Session["NotificacionesEnviadas"] = result;
			return Json(result, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetRecibidos()
		{
            var wsClient = ServiceHelper.GetNotificacionesWS();
			wsClient.Open();
			DataSet dsResultado = wsClient.GetMensajesRecibidos(Convert.ToInt32(Session["usr_id"]));
			wsClient.Abort();

			var result = new List<Notificacion>();
			foreach (DataRow item in dsResultado.Tables[0].Rows)
			{
				result.Add(new Notificacion(item,false));
			}
			this.Session["Notificaciones"] = result;
			return Json(result, JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetUsuarios(string grupoId)
		{
            var result = new List<Usuario>();
            try
            {
                int grupo;
                if (int.TryParse(grupoId, out grupo))
                {
                    WSNotificaciones.NotificacionesSoapClient wsClient = ServiceHelper.GetNotificacionesWS();
                    wsClient.Open();
                    DataSet dsUsuarios;
                    if (Session["page"] != null && Session["page"].ToString() == "Liquidaciones")
                        dsUsuarios = wsClient.GetUsuariosPrestadores(grupo);
                    else if (Session["page"] != null && Session["page"].ToString() == "EmpleadosLiquidaciones")
                        dsUsuarios = wsClient.GetUsuariosGerencias(grupo);
                    else
                        dsUsuarios = wsClient.GetUsuarios(grupo, 0);
                    wsClient.Close();
                    DataTable dtUsuarios = dsUsuarios.Tables[0];
                    var model = new Notificacion();

                    foreach (DataRow dtRow in dtUsuarios.Rows)
                    {
                        result.Add(new Usuario(dtRow));
                    }

                }
            }
            catch(System.ServiceModel.CommunicationException ex)
            {

            }
            catch (Exception ex)
            {

                //throw;
            }
			return Json(result, JsonRequestBehavior.AllowGet);
		}

	}
}
