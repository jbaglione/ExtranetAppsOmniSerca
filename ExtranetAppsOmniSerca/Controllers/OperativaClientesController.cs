using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExtranetAppsOmniSerca.Models;
using System.Text.RegularExpressions;
using NLog;

namespace ExtranetAppsOmniSerca.Controllers
{
    public class OperativaClientesController : Controller
    {

       private Helpers.Helpers hlp = new Helpers.Helpers();
       private Logger logger = LogManager.GetCurrentClassLogger();

        //
        // GET: /OperativaClientes/

        public ActionResult Index()
        {
            ViewBag.Title = "Consulta de Servicios";

            int usr = Convert.ToInt32(Session["usr_id"]);
            if (usr != 0 && validarUsuario(usr))
                return View();
            else
            {
                //Session.RemoveAll();
                return RedirectToAction("Index", "Error");
            }
        }

        //
        // Acá valido el usuario contra el web service. En ViewBags pongo algunas variables que me van a servir
        // del lado del cliente por el tema del select de medicos en modo administrador, etc.

        public bool validarUsuario(long usr)
        {
            try
            {
                WSOperativaClientes.ClientesOperativosSoapClient wsClient = new WSOperativaClientes.ClientesOperativosSoapClient();
                wsClient.Open();
                DataTable dtUsuario = wsClient.GetUsuarioValidacion(usr).Tables[0];
                wsClient.Abort();

                if ((dtUsuario.Rows[0]["NombreUsuario"]).ToString() == "")
                {
                    return false;
                }
                else
                {

                    Session["usr_id"] = usr;
                    Session["UserName"] = dtUsuario.Rows[0]["NombreUsuario"].ToString();
                    Session["acceso_curso"] = Convert.ToInt32(dtUsuario.Rows[0]["tabEnCurso"]);
                    Session["acceso_finalizados"] = Convert.ToInt32(dtUsuario.Rows[0]["tabFinalizados"]);
                    Session["acceso_erroneos"] = Convert.ToInt32(dtUsuario.Rows[0]["tabErroneos"]);
                    Session["acceso_denuncias"] = Convert.ToInt32(dtUsuario.Rows[0]["tabDenuncias"]);
                    //Session["prm_modo_publicacion"] = Convert.ToInt32(dtUsuario.Rows[0]["prmModoPublicacion"]);
                    return true;
                }
            }
            catch(Exception ex)
            {
                logger.Error(ex.ToString());

                return false;

            }

        }


        [HttpGet]
        public long IsReclamado(long idInc)
        {
            // --> Si la llamada es via ajax..
            if (Request.IsAjaxRequest())
            {
                // --> Declaro web service
                WSOperativaClientes.ClientesOperativosSoapClient wsClient = new WSOperativaClientes.ClientesOperativosSoapClient();

                try
                {
                    // --> Abro web service, mando id de incidente, obtengo si está reclamado o no..
                    wsClient.Open();
                    long isRec = Convert.ToInt32(wsClient.IsReclamado(idInc).Tables[0].Rows[0]["Reclamado"]);
                    wsClient.Abort();
                    return isRec;
                }
                catch
                {
                    return -1;
                }

            }

            return -1;
        }

        [HttpPost]
        public string SetReclamoEnCurso(long idInc, string observ)
        {
            // --> Si la llamada es via ajax..
            if (Request.IsAjaxRequest())
            {
                observ = hlp.StringEscape(observ);
                observ = observ.ToUpper();
                // --> Declaro web service
                WSProduccionOperativaClientes.ClientesOperativosSoapClient wsClient = new WSProduccionOperativaClientes.ClientesOperativosSoapClient();

                try
                {
                    //--> Abro web service, mando id de incidente, observaciones y idUsr y seteo el reclamo..
                    wsClient.Open();
                    string result = wsClient.SetReclamo(idInc, observ, getUserID()).Tables[0].Rows[0]["Resultado"].ToString();
                    wsClient.Abort();
                    return result;
                }
                catch 
                {
                    return "Error con la conexión del web service";
                }

            }

            return "Error con la conexión del web service";

        }

        [HttpPost]
        public string CorregirErroneo(long incID, string nOrden, string nAfiliado)
        {
            // --> Si la llamada es via ajax..
            if (Request.IsAjaxRequest())
            {
                nOrden = hlp.StringEscape(nOrden);
                nAfiliado = hlp.StringEscape(nAfiliado);

                if (nOrden == "" || nAfiliado == "") return "Debe completar Nro. de Orden y Nro. de Afiliado para corregir el servicio";
                Regex regex = new Regex("^[0-9]+$");
                if (!regex.IsMatch(nOrden) || !regex.IsMatch(nAfiliado)) return "Sólo puede ingresar números";


                // --> Declaro web service
                WSProduccionOperativaClientes.ClientesOperativosSoapClient wsClient = new WSProduccionOperativaClientes.ClientesOperativosSoapClient();

                try
                {
                    //--> Abro web service, mando id de incidente, observaciones y idUsr y seteo el reclamo..
                    wsClient.Open();
                    string result = wsClient.SetCorreccion(incID, nOrden, nAfiliado, getUserID()).Tables[0].Rows[0]["Resultado"].ToString();
                    wsClient.Abort();
                    return result;
                }
                catch
                {
                    return "Error con la conexión del web service";
                }

            }

            return "Error con la conexión del web service";

        }

        // --> Obtengo los servicios finalizados del servidor
        public JsonResult GetServiciosClientes()
        {
            var query = Request.QueryString;
            long fecDesde = 0, fecHasta = 0, pCli = 0, pWS;
            string[] vDesde = query.GetValues("fecDesde");
            string[] vHasta = query.GetValues("fecHasta");
            string[] vPCli = query.GetValues("pCli");
            if (vDesde != null)
            {
                fecDesde = Convert.ToInt32(vDesde[0]);
                fecHasta = Convert.ToInt32(vHasta[0]);
            }

            if (vPCli != null)
            {
                pCli = Convert.ToInt32(vPCli[0]);
            }

            pWS = Convert.ToInt32(query.GetValues("pWS")[0]);
            DataTable dt = new DataTable();
            dt = getDataFromWebService(pWS, fecDesde, fecHasta,pCli);

            // --> Si no falla el webservice ..
            if (dt != null)
            {
                JsonResult result = new JsonResult();
                switch (pWS)
                {
                    case 1:
                        result = Json(getOpEnCursoFormatted(dt), JsonRequestBehavior.AllowGet);
                        break;
                    case 2:
                        result = Json(getServFinalizadosFormatted(dt), JsonRequestBehavior.AllowGet);
                        break;
                    case 3:
                        result = Json(getErroneosFormatted(dt), JsonRequestBehavior.AllowGet);
                        break;
                    case 4:
                        result = Json(getFiltroClientesFormatted(dt), JsonRequestBehavior.AllowGet);
                        break;
                    case 5:
                        result = Json(getDenunciasFormatted(dt), JsonRequestBehavior.AllowGet);
                        break;
                }

                return result;

            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

        }

        private DataTable getDataFromWebService(long pWS, long fDesde, long fHasta, long pCli)
        {

            WSOperativaClientes.ClientesOperativosSoapClient wsClient = new WSOperativaClientes.ClientesOperativosSoapClient();
            DataSet ds = new DataSet();

            try
            {
                wsClient.Open();

                switch (pWS)
                {
                    case 1:
                        ds = wsClient.GetOperativaEnCurso(getUserID());
                        break;
                    case 2:
                        ds = wsClient.GetFinalizados(getUserID(), fDesde, fHasta,pCli);
                        break;
                    case 3:
                        ds = wsClient.GetErroresAutorizacion(getUserID(), fDesde, fHasta);
                        break;
                    case 4:
                        ds = wsClient.GetClientesUsuario(getUserID());
                        break;
                    case 5:
                        string strDesde = fDesde.ToString();
                        string strHasta = fHasta.ToString();
                        strDesde = strDesde.Substring(0, 4) + "-" + strDesde.Substring(4, 2) + "-" + strDesde.Substring(6, 2);
                        strHasta = strHasta.Substring(0, 4) + "-" + strHasta.Substring(4, 2) + "-" + strHasta.Substring(6, 2);
                        ds = wsClient.GetDenuncias(getUserID(),strDesde,strHasta);
                        break;
                }

                wsClient.Abort();
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                wsClient.Abort();
                var msg = ex.Message;
                return null;
            }

        }

        // --> Formateo los servicios en curso a una lista de Servicios Finalizados
        private List<ServFinalizados> getServFinalizadosFormatted(DataTable dt)
        {

            List<ServFinalizados> lstFinalizados = new List<ServFinalizados>();

            foreach (DataRow r in dt.Rows)
            {
                ServFinalizados servFinalizado = new ServFinalizados();
                servFinalizado.dataRowToServFinalizados(r);
                lstFinalizados.Add(servFinalizado);

            }

            return lstFinalizados;
        }

        // --> Formateo los servicios en curso a una lista de OperativaEnCurso
        private List<OperativaEnCurso> getOpEnCursoFormatted(DataTable dt)
        {

            List<OperativaEnCurso> lstOpEnCurso = new List<OperativaEnCurso>();

            foreach (DataRow r in dt.Rows)
            {
                OperativaEnCurso opEnCurso = new OperativaEnCurso();
                opEnCurso.dataRowToOperativaEnCurso(r);
                lstOpEnCurso.Add(opEnCurso);

            }

            return lstOpEnCurso;
        }

        // --> Formateo los servicios en curso a una lista de OperativaEnCurso
        private List<ServErroneos> getErroneosFormatted(DataTable dt)
        {

            List<ServErroneos> lst = new List<ServErroneos>();

            foreach (DataRow r in dt.Rows)
            {
                ServErroneos servErroneo = new ServErroneos();
                servErroneo.dataRowToServErroneos(r);
                lst.Add(servErroneo);

            }

            return lst;
        }

        // --> Formateo los servicios en curso a una lista de OperativaEnCurso
        private List<FtrCliente> getFiltroClientesFormatted(DataTable dt)
        {

            List<FtrCliente> lst = new List<FtrCliente>();

            foreach (DataRow r in dt.Rows)
            {
                FtrCliente cli = new FtrCliente();
                cli.dataRowToFtrcliente(r);
                lst.Add(cli);

            }

            return lst;
        }

        // --> Formateo las denuncias
        private List<Denuncia> getDenunciasFormatted(DataTable dt)
        {

            List<Denuncia> lstDenuncias = new List<Denuncia>();

            foreach (DataRow r in dt.Rows)
            {
                Denuncia denuncia = new Denuncia();
                denuncia.dataRowToDenuncia(r);
                lstDenuncias.Add(denuncia);

            }

            return lstDenuncias;
        }


        // --> Obtengo el user_id del cliente
        private int getUserID()
        {
            return Convert.ToInt32(Session["usr_id"]);
        }

        private long getPrmPublicacion()
        {
            return Convert.ToInt32(Session["prm_modo_publicacion"]);
        }

    }
}
