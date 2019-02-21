using ExtranetAppsOmniSerca.Helpers;
using ExtranetAppsOmniSerca.Models;
using ExtranetAppsOmniSerca.ViewModels;
using vm = ExtranetAppsOmniSerca.ViewModels;
using m = ExtranetAppsOmniSerca.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;
using System.Net.Mail;
using System.Web.Services.Protocols;
using NLog;

namespace ExtranetAppsOmniSerca.Controllers
{
    public class MedicosController : Controller
    {
        private Logger logger = LogManager.GetCurrentClassLogger();
        //
        // GET: /Medicos/
        // Reviso si me viene usr por parametro (aca hay que meter seguridad, despues vemos como)
        // Si no me viene nada, tiro 404. Si me viene un parametro usr, lo valido, y si esta ok, retorno el index comun.

        public ActionResult Index()
        {
            ViewBag.Title = "Honorarios Medicos";

            int usr = Convert.ToInt32(Session["usr_id"]);

            if (usr != 0 && validarUsuario(usr))
            {
                ViewBag.NotificacionesNuevas = getNotificacionesNuevas(usr);
                return View();
            }
            else
            {
                //Session.RemoveAll();
                return RedirectToAction("Index", "Error");
            }
        }

        private dynamic getNotificacionesNuevas(int p)
        {
            int cantidad = ServiceHelper.getNotificacionesCount(p);
            if (cantidad > 0)
            {
                return string.Format("<span class=\"badge\">{0}</span>", cantidad);
            }

            return "";
        }

        //
        // Acá valido el usuario contra el web service. En ViewBags pongo algunas variables que me van a servir
        // del lado del cliente por el tema del select de medicos en modo administrador, etc.

        public bool validarUsuario(long usr)
        {
            try
            {
                WSContratadosLiquidaciones.ContratadosLiquidacionesSoapClient wsClient = new WSContratadosLiquidaciones.ContratadosLiquidacionesSoapClient();
                wsClient.Open();
                DataTable dtUsuario = wsClient.GetUsuarioValidacion(usr).Tables[0];
                wsClient.Abort();
                // 1,2 receptor
                // 3 administrador
                if (Convert.ToInt32(dtUsuario.Rows[0]["Acceso"]) == 0)
                {
                    return false;
                }
                else
                {
                    Session["usr_id"] = usr;
                    Session["UserName"] = dtUsuario.Rows[0]["NombreUsuario"].ToString();
                    Session["MedicoName"] = dtUsuario.Rows[0]["NombreMedico"].ToString();
                    Session["Acceso"] = Convert.ToInt32(dtUsuario.Rows[0]["Acceso"]);
                    //Session["page"] = "Medicos";
                    return true;
                }
            }
            catch (Exception ex)
            {

                return false;

            }

        }

        //
        // Obtengo los datos para el filtro de periodos (obtengo el actual y los dos anteriores)

        public JsonResult GetFiltroHorarioCoordinacionList()
        {
            WSContratadosLiquidaciones.ContratadosLiquidacionesSoapClient wsClient = new WSContratadosLiquidaciones.ContratadosLiquidacionesSoapClient();
            DataSet dsCoordinaciones = wsClient.GetGrillaCoordinaciones(Convert.ToInt32(Session["usr_id"]));

            List<FiltroCoordinaciones> lstCoord = new List<FiltroCoordinaciones>();

            DataTable dtCoordinaciones = dsCoordinaciones.Tables[0];

            foreach (DataRow dtRow in dtCoordinaciones.Rows)
            {
                FiltroCoordinaciones coord = new FiltroCoordinaciones(Convert.ToInt32(dtRow["ID"]), dtRow["Nombre"].ToString());
                lstCoord.Add(coord);
            }

            return Json(lstCoord, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetHorarioCoordinacion(string coordinacion, string fecha)
        {
            var result = new List<vm.HorarioCoordinacion>();
            WSContratadosLiquidaciones.ContratadosLiquidacionesSoapClient wsClient = new WSContratadosLiquidaciones.ContratadosLiquidacionesSoapClient();
            var desde = DateTime.ParseExact(fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var hasta = desde.AddDays(6);
            DataSet dsCoordinaciones = wsClient.GetGrillaCoordinacionesHorarios(long.Parse(coordinacion), long.Parse(desde.ToString("yyyyMMdd")), long.Parse(hasta.ToString("yyyyMMdd")));


            DataTable dtCoordinaciones = dsCoordinaciones.Tables[0];
            var source = new List<m.HorarioCoordinacion>();
            m.HorarioCoordinacion item;
            foreach (DataRow dtRow in dtCoordinaciones.Rows)
            {
                item = new m.HorarioCoordinacion();
                item.Id = Convert.ToInt64(dtRow["ID"]);
                item.PersonalId = dtRow["PersonalId"].ToString();
                item.Apellido = dtRow["Apellido"].ToString();
                if (item.Id > 0)
                {
                    item.FecEntrada = dtRow["FecEntrada"].ToString();
                    item.HorEntrada = dtRow["HorEntrada"].ToString();
                    item.FecSalida = dtRow["FecSalida"].ToString();
                    item.HorSalida = dtRow["HorSalida"].ToString();
                    item.MovilId = dtRow["MovilId"].ToString();
                    item.SituacionId = Convert.ToInt64(dtRow["SituacionId"]);
                }
                source.Add(item);
            }


            result.AddRange(source.Select(i => new { i.PersonalId, i.Apellido }).Distinct().Select(i => new vm.HorarioCoordinacion() { Legajo = i.PersonalId, Medico = i.Apellido }));
            result.AddRange(source.Select(i => new { i.PersonalId, i.Apellido }).Distinct().Select(i => new vm.HorarioCoordinacion() { Legajo = i.PersonalId, Medico = i.Apellido }));
            result.AddRange(source.Select(i => new { i.PersonalId, i.Apellido }).Distinct().Select(i => new vm.HorarioCoordinacion() { Legajo = i.PersonalId, Medico = i.Apellido }));

            int posicion = 0;
            DateTime fechaActual;
            foreach (var row in source.Where(i => i.Id > 0))
            {
                fechaActual = DateTime.ParseExact(row.FecEntrada, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                posicion = Convert.ToInt32((fechaActual - desde).TotalDays) + 1;
                var xx = result.Where(i => i.Legajo == row.PersonalId && i.Libre(posicion));
                if (xx.Any())
                {
                    xx.First().Set(posicion, row);
                }
                else
                {
                    // errorr
                }
            }


            result = result.OrderBy(i => i.Medico).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getFiltroPeriodos()
        {
            List<FiltroPeriodos> lstPeriodos = new List<FiltroPeriodos>();
            DateTime fecActual = DateTime.Now;
            DateTime fecAnterior = fecActual.AddMonths(-1);
            DateTime fecAntePenultimo = fecActual.AddMonths(-2);
            lstPeriodos.Add(new FiltroPeriodos(getFormattedPeriod(fecAntePenultimo), getFormattedDescriptionOfPeriod(fecAntePenultimo)));
            lstPeriodos.Add(new FiltroPeriodos(getFormattedPeriod(fecAnterior), getFormattedDescriptionOfPeriod(fecAnterior)));
            lstPeriodos.Add(new FiltroPeriodos(getFormattedPeriod(fecActual), getFormattedDescriptionOfPeriod(fecActual)));

            return Json(lstPeriodos, JsonRequestBehavior.AllowGet);
        }

        //
        // Obtengo los datos para el filtro de coordinaciones (recibe el id del medico para buscar todas las coordinaciones de él) 	ExtranetAppsOmniSerca.dll!ExtranetAppsOmniSerca.Helpers.ServiceHelper.getNotificacionesFormatted(System.Data.DataTable dt) Line 21	C#


        public JsonResult getFiltroCoordinaciones(long usr_id)
        {
            try
            {
                List<FiltroCoordinaciones> lstCoord = new List<FiltroCoordinaciones>();
                ////TODO: BORRAR
                //lstCoord.Add(new FiltroCoordinaciones(1, "descripcion"));
                //return Json(lstCoord, JsonRequestBehavior.AllowGet);
                
                WSContratadosLiquidaciones.ContratadosLiquidacionesSoapClient wsClient = new WSContratadosLiquidaciones.ContratadosLiquidacionesSoapClient();
                DataSet dsCoordinaciones = wsClient.GetCoordinaciones(usr_id);

                DataTable dtCoordinaciones = dsCoordinaciones.Tables[0];

                foreach (DataRow dtRow in dtCoordinaciones.Rows)
                {
                    FiltroCoordinaciones coord = new FiltroCoordinaciones(Convert.ToInt32(dtRow["CoordinacionMedicaId"]), dtRow["Nombre"].ToString());
                    lstCoord.Add(coord);
                }

                return Json(lstCoord, JsonRequestBehavior.AllowGet);
            }
            catch (SoapException ex)
            {
                logger.Error(ex);
                throw;
            }
        }


        //
        // Obtengo los datos para el filtro de motivos de reclamo (popup guardia / servicios)
        public JsonResult getFiltroMotivoReclamo(int flgTipoReclamo)
        {
            try
            {

                WSContratadosLiquidaciones.ContratadosLiquidacionesSoapClient wsClient = new WSContratadosLiquidaciones.ContratadosLiquidacionesSoapClient();
                wsClient.Open();
                DataSet dsReclamos = wsClient.GetMotivosReclamo(flgTipoReclamo);
                wsClient.Abort();
                List<FiltroReclamo> lstFtrReclamo = new List<FiltroReclamo>();

                DataTable dtReclamos = dsReclamos.Tables[0];

                foreach (DataRow dtRow in dtReclamos.Rows)
                {
                    //Esto lo hago para no tener que ir a la base de datos cada vez que hago un select en un motivo
                    //Entonces tengo id/difIngreso en el value del select, y si esta en 1, habilito para modificar horario.
                    string idConDifIngreso = dtRow["ID"].ToString() + "/" + dtRow["flgDifIngreso"].ToString();
                    FiltroReclamo reclamo = new FiltroReclamo(idConDifIngreso, dtRow["Descripcion"].ToString());
                    lstFtrReclamo.Add(reclamo);
                }

                return Json(lstFtrReclamo, JsonRequestBehavior.AllowGet);
            }
            catch (FaultException ex)
            {
                logger.Error(ex);
                throw;
            }
            catch (SoapException ex)
            {
                logger.Error(ex);
                throw;
            }

        }

        //
        // Obtengo los datos para el filtro de medicos (Si me llega el id de un medico, es porque es un medico el que está solicitando
        // el filtro, entonces solamente accede a su registro.

        public JsonResult getFiltroMedicos(long usr_id, long selPeriodo, int selEstado, int esMedico = 0)
        {
            try
            {
                WSContratadosLiquidaciones.ContratadosLiquidacionesSoapClient wsClient = new WSContratadosLiquidaciones.ContratadosLiquidacionesSoapClient();
                wsClient.Open();
                DataSet dsMedicos = wsClient.GetMedicos(usr_id, selPeriodo, selEstado);
                wsClient.Abort();
                List<Medico> lstMedicos = new List<Medico>();

                DataTable dtMedicos = dsMedicos.Tables[0];

                foreach (DataRow dtRow in dtMedicos.Rows)
                {
                    Medico medico = new Medico();
                    medico.UsuarioID = Convert.ToInt64(dtRow["UsuarioId"]);
                    medico.Nombre = dtRow["Nombre"].ToString();
                    lstMedicos.Add(medico);
                }

                if (esMedico == 1)
                {
                    lstMedicos = lstMedicos.Where(x => x.UsuarioID == usr_id).ToList();
                }
                return Json(lstMedicos, JsonRequestBehavior.AllowGet);
            }
            catch (FaultException ex)
            {
                logger.Error(ex);
                throw;
            }
            catch (SoapException ex)
            {
                logger.Error(ex);
                throw;
            }
        }

        //
        // Obtengo los datos para la grilla de guardias (los parametros me llegan por querystring)

        public JsonResult GetGuardias()
        {
            var query = Request.QueryString;
            long periodo = Convert.ToInt64(query.GetValues("periodo")[0]);
            int dia = Convert.ToInt32(query.GetValues("dia")[0]);
            int coordinacion = Convert.ToInt32(query.GetValues("coordinacion")[0]);
            long medico = Convert.ToInt64(query.GetValues("medico")[0]);
            int estado = Convert.ToInt32(query.GetValues("estado")[0]);
            DataSet dsGuardias = getGuardiasFromWebService(periodo, coordinacion, medico, estado);
            List<Guardia> guardias = new List<Guardia>();
            if (dsGuardias == null)
            {
                guardias = null;
            }
            else
            {
                guardias = getGuardiasFormatted(dsGuardias, dia);
            }

            return Json(guardias, JsonRequestBehavior.AllowGet);
        }

        //
        // Obtengo los datos para la grilla de servicios (los parametros me llegan por querystring)

        public JsonResult GetServicios()
        {
            var query = Request.QueryString;
            long periodo = Convert.ToInt64(query.GetValues("periodo")[0]);
            int dia = Convert.ToInt32(query.GetValues("dia")[0]);
            int coordinacion = Convert.ToInt32(query.GetValues("coordinacion")[0]);
            long medico = Convert.ToInt64(query.GetValues("medico")[0]);
            int estado = Convert.ToInt32(query.GetValues("estado")[0]);
            DataSet dsServicios = getServiciosFromWebService(periodo, coordinacion, medico, estado);
            List<Servicio> servicios = new List<Servicio>();
            if (dsServicios == null)
            {
                servicios = null;
            }
            else
            {
                servicios = getServiciosFormatted(dsServicios, dia);
            }

            return Json(servicios, JsonRequestBehavior.AllowGet);
        }

        //
        // Obtengo los datos para la grilla de resumen de liquidacion (los parametros me llegan por querystring)

        public JsonResult GetResumenLiquidacion()
        {
            var query = Request.QueryString;
            long periodo = Convert.ToInt64(query.GetValues("periodo")[0]);
            int coordinacion = Convert.ToInt32(query.GetValues("coordinacion")[0]);
            long medico = Convert.ToInt64(query.GetValues("medico")[0]);
            DataSet dsResumen = getResumenFromWebService(periodo, coordinacion, medico);
            List<ResumenLiquidacion> resLiquidacion = new List<ResumenLiquidacion>();
            if (dsResumen == null)
            {
                resLiquidacion = null;
            }
            else
            {
                resLiquidacion = getResumenLiquidacionFormatted(dsResumen);
            }

            GetHorarios();

            return Json(resLiquidacion, JsonRequestBehavior.AllowGet);
        }

        //
        // Obtengo los datos para la grilla de horarios del medico (los parametros me llegan por querystring)

        public JsonResult GetHorarios()
        {
            var query = Request.QueryString;
            long periodo = Convert.ToInt64(query.GetValues("periodo")[0]);
            long medico = Convert.ToInt64(query.GetValues("medico")[0]);
            DataSet dsHorarios = getHorariosFromWebService(periodo, medico);
            List<Horario> lstHorarios = new List<Horario>();

            if (dsHorarios == null)
            {
                lstHorarios = null;
            }
            else
            {
                lstHorarios = getHorariosFormatted(dsHorarios);
            }

            return Json(lstHorarios, JsonRequestBehavior.AllowGet);
        }

        //
        // Obtengo los datos de estado del reclamo de la guardia. Para eso me llega el id de la guardia

        public JsonResult GetEstadoReclamo(string id, int pMode)
        {
            DataTable dtEstadoReclamo = getEstadoReclamoFromWebService(id, pMode);

            if (pMode == 0)
            {
                // --> Si es una guardia..
                EstadoReclamoGuardia estadoReclamo = new EstadoReclamoGuardia();
                if (dtEstadoReclamo == null)
                {
                    estadoReclamo = null;
                }
                else
                {
                    estadoReclamo = getEstadoReclamoGuardiaFormatted(dtEstadoReclamo);
                }
                return Json(estadoReclamo, JsonRequestBehavior.AllowGet);
            }
            else
            {
                // --> Si es un incidente..
                EstadoReclamoServicio estadoServicio = new EstadoReclamoServicio();
                if (dtEstadoReclamo == null)
                {
                    estadoServicio = null;
                }
                else
                {
                    estadoServicio = getEstadoReclamoServicioFormatted(dtEstadoReclamo);
                }
                return Json(estadoServicio, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult ValidateMovil(string movil, string fecha)
        {
            string result = "Movil no encontrado";
            var wsClient = new WSContratadosLiquidaciones.ContratadosLiquidacionesSoapClient();
            try
            {
                wsClient.Open();
                DataSet ds = wsClient.ValidateMovil(movil, fecha.FormatedDateToAnsi());
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    result = ds.Tables[0].Rows[0][1].ToString();
                }
                wsClient.Abort();
            }
            catch
            {
                //result = ""
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //
        // Obtengo datos de los diferentes webservices

        private DataTable getEstadoReclamoFromWebService(string id, int pMode)
        {
            try
            {
                WSContratadosLiquidaciones.ContratadosLiquidacionesSoapClient wsClient = new WSContratadosLiquidaciones.ContratadosLiquidacionesSoapClient();
                wsClient.Open();
                DataSet dsEstReclamo = wsClient.GetEstadoReclamo(id, pMode);
                wsClient.Abort();
                return dsEstReclamo.Tables[0];
            }
            catch (FaultException ex)
            {
                logger.Error(ex);
            }
            catch (SoapException ex)
            {
                logger.Error(ex);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return null;
        }

        private DataSet getGuardiasFromWebService(long periodo, int coord, long usr_id, int estado)
        {
            try
            {
                WSContratadosLiquidaciones.ContratadosLiquidacionesSoapClient wsClient = new WSContratadosLiquidaciones.ContratadosLiquidacionesSoapClient();
                wsClient.Open();
                DataSet ds = wsClient.GetGuardiasDetalle(usr_id, periodo, coord, estado);
                wsClient.Abort();
                return ds;
            }
            catch (FaultException ex)
            {
                logger.Error(ex);
            }
            catch (SoapException ex)
            {
                logger.Error(ex);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return null;
        }

        private DataSet getServiciosFromWebService(long periodo, int coord, long medico, int estado)
        {

            try
            {
                WSContratadosLiquidaciones.ContratadosLiquidacionesSoapClient wsClient = new WSContratadosLiquidaciones.ContratadosLiquidacionesSoapClient();
                wsClient.Open();
                DataSet ds = wsClient.GetIncidentes(medico, periodo, coord, estado);
                wsClient.Abort();
                return ds;
            }
            catch (FaultException ex)
            {
                logger.Error(ex);
            }
            catch (SoapException ex)
            {
                logger.Error(ex);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return null;
        }

        private DataSet getResumenFromWebService(long periodo, int coord, long medico)
        {
            try
            {
                WSContratadosLiquidaciones.ContratadosLiquidacionesSoapClient wsClient = new WSContratadosLiquidaciones.ContratadosLiquidacionesSoapClient();
                DataSet ds = wsClient.GetResumen(medico, periodo, coord);
                return ds;

            }
            catch (FaultException ex)
            {
                logger.Error(ex);
            }
            catch (SoapException ex)
            {
                logger.Error(ex);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return null;
        }

        private DataSet getHorariosFromWebService(long periodo, long medico)
        {
            try
            {
                WSContratadosLiquidaciones.ContratadosLiquidacionesSoapClient wsClient = new WSContratadosLiquidaciones.ContratadosLiquidacionesSoapClient();
                wsClient.Open();
                DataSet ds = wsClient.GetHorarios2(medico, periodo);
                wsClient.Abort();
                return ds;
            }
            catch (FaultException ex)
            {
                logger.Error(ex);
            }
            catch (SoapException ex)
            {
                logger.Error(ex);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return null;
        }

        //Formateo la datarow del estadoreclamo para pasarselo a una instancia de EstadoReclamoGuardia

        private EstadoReclamoGuardia getEstadoReclamoGuardiaFormatted(DataTable dt)
        {
            DataRow dr = dt.Rows[0];
            EstadoReclamoGuardia estReclamo = new EstadoReclamoGuardia();
            estReclamo.Cerrado = Convert.ToInt32(dr["Cerrado"]);
            estReclamo.Conforme = Convert.ToInt32(dr["Conforme"]);
            estReclamo.Entrada = dr["Entrada"].ToString();
            estReclamo.Salida = dr["Salida"].ToString();
            estReclamo.Reclamo = dr["Reclamo"].ToString();
            estReclamo.Respuesta = dr["Respuesta"].ToString();
            estReclamo.MotivoId = dr["MotivoId"].ToString();
            estReclamo.Estado = Convert.ToInt32(dr["Estado"]);

            return estReclamo;

        }

        //Formateo la datarow del estadoreclamo para pasarselo a una instancia de EstadoReclamoServicio

        private EstadoReclamoServicio getEstadoReclamoServicioFormatted(DataTable dt)
        {
            //editar con los datos reales del getEstadoReclamoServicio
            DataRow dr = dt.Rows[0];
            EstadoReclamoServicio estServicio = new EstadoReclamoServicio();
            estServicio.Cerrado = Convert.ToInt32(dr["Cerrado"]);
            estServicio.Conforme = Convert.ToInt32(dr["Conforme"]);
            estServicio.Reclamo = dr["Reclamo"].ToString();
            estServicio.Respuesta = dr["Respuesta"].ToString();
            estServicio.MotivoId = dr["MotivoId"].ToString();
            estServicio.Estado = Convert.ToInt32(dr["Estado"]);

            return estServicio;

        }

        //Formateo los dataset traidos de los webservices para armar los datos estructurados en objetos de distintas clases

        private List<Guardia> getGuardiasFormatted(DataSet dsGuardias, int dia)
        {

            List<Guardia> lstGuardias = new List<Guardia>();

            DataTable dtGuardias = dsGuardias.Tables[0];

            foreach (DataRow dtRow in dtGuardias.Rows)
            {
                Guardia guardia = new Guardia();
                guardia.ID = dtRow["ID"].ToString();
                guardia.Dia = Convert.ToInt32((dtRow["FecMovimiento"].ToString()).Substring(6, 2));
                guardia.DiaDeLaSemana = getGuardiaFechaFormatted(dtRow["FecMovimiento"].ToString(), 1);
                guardia.Periodo = getGuardiaFechaFormatted(dtRow["FecMovimiento"].ToString(), 2);
                guardia.Tarifa = dtRow["TipoLiquidacionId"].ToString();
                guardia.HorarioEntrada = dtRow["HorDesde"].ToString();
                guardia.MinutosLlegadaTarde = Convert.ToInt32(dtRow["minTarde"]);
                guardia.HorarioSalida = dtRow["HorHasta"].ToString();
                guardia.MinutosRetiroAnticipado = Convert.ToInt32(dtRow["minRetAnticipado"]);
                guardia.Movil = dtRow["MovilId"].ToString();
                guardia.HorasTrabajadas = dtRow["TotalHoras"].ToString();
                guardia.Rojos = Convert.ToInt32(dtRow["Rojos"]);
                guardia.Amarillos = Convert.ToInt32(dtRow["Amarillos"]);
                guardia.Verdes = Convert.ToInt32(dtRow["Verdes"]);
                guardia.TrasladosProgramados = Convert.ToInt32(dtRow["Traslados"]);
                guardia.ImpTotalHoras = Convert.ToDouble(dtRow["ImpHora"]);
                guardia.ImpEspecialidad = Convert.ToDouble(dtRow["ImpEspecialidad"]);
                guardia.ImpPrestacion = Convert.ToDouble(dtRow["ImpPrestacion"]);
                guardia.ImpPrestacionExcedente = Convert.ToDouble(dtRow["ImpPrestacionExcedente"]);
                guardia.ImpAnticipo = Convert.ToDouble(dtRow["ImpAnticipos"]);
                guardia.ImpFinal = Convert.ToDouble(dtRow["ImpFinal"]);
                guardia.Estado = Convert.ToInt32(dtRow["Estado"]);
                guardia.FecMovimiento = getGuardiaFechaFormatted(dtRow["FecMovimiento"].ToString(), 3);

                lstGuardias.Add(guardia);

            }

            if (dia != 0)
            {
                lstGuardias = lstGuardias.Where(x => x.Dia == dia).ToList();
            }

            return lstGuardias;

        }

        private List<Servicio> getServiciosFormatted(DataSet dsServicios, int dia)
        {

            List<Servicio> lstServicios = new List<Servicio>();

            DataTable dtServicios = dsServicios.Tables[0];

            foreach (DataRow dtRow in dtServicios.Rows)
            {
                string fecInc = dtRow["FecIncidente"].ToString();
                Servicio servicio = new Servicio();
                servicio.IncidenteID = dtRow["ID"].ToString();
                servicio.NroInc = dtRow["NroIncidente"].ToString();
                servicio.Fecha = getGuardiaFechaFormatted(fecInc, 1);
                servicio.Iva = dtRow["Iva"].ToString();
                servicio.Paciente = dtRow["Paciente"].ToString();
                servicio.Localidad = dtRow["Localidad"].ToString();
                servicio.Cdn = dtRow["Cdn"].ToString();
                servicio.Tarifa = dtRow["Tar"].ToString();
                servicio.DiaDeLaSemana = dtRow["Dia"].ToString();
                servicio.Tur = dtRow["Tur"].ToString();
                servicio.Grado = dtRow["Grado"].ToString();
                servicio.CoPago = Convert.ToDouble(dtRow["CoPago"]);
                servicio.Importe = Convert.ToDouble(dtRow["Importe"]);
                servicio.Dia = Convert.ToInt32((dtRow["FecIncidente"].ToString()).Substring(6, 2));
                servicio.MesDia = fecInc.Substring(6, 2) + "/" + fecInc.Substring(4, 2);
                servicio.Estado = Convert.ToInt32(dtRow["Estado"]);
                lstServicios.Add(servicio);

            }

            if (dia != 0)
            {
                lstServicios = lstServicios.Where(x => x.Dia == dia).ToList();
            }

            return lstServicios;

        }

        private List<ResumenLiquidacion> getResumenLiquidacionFormatted(DataSet dsResumenLiq)
        {

            List<ResumenLiquidacion> lstResumenLiq = new List<ResumenLiquidacion>();

            DataTable dtResumenLiq = dsResumenLiq.Tables[0];

            foreach (DataRow dtRow in dtResumenLiq.Rows)
            {
                Double valImporte = Convert.ToDouble(dtRow["Importe"]);
                if (valImporte != 0)
                {
                    ResumenLiquidacion resLiq = new ResumenLiquidacion();
                    resLiq.Item = dtRow["Item"].ToString();
                    resLiq.Importe = Convert.ToDouble(dtRow["Importe"]);

                    lstResumenLiq.Add(resLiq);
                }
            }

            return lstResumenLiq;

        }

        private List<Horario> getHorariosFormatted(DataSet dsHorarios)
        {

            List<Horario> lstHorarios = new List<Horario>() {
                new Horario() { DiaNumero = 1, DiaDeLaSemana = "Lunes" },
                new Horario() { DiaNumero = 2, DiaDeLaSemana = "Martes" } ,
                new Horario() { DiaNumero = 3, DiaDeLaSemana = "Miércoles" } ,
                new Horario() { DiaNumero = 4, DiaDeLaSemana = "Jueves" } ,
                new Horario() { DiaNumero = 5, DiaDeLaSemana = "Viernes" } ,
                new Horario() { DiaNumero = 6, DiaDeLaSemana = "Sábado" } ,
                new Horario() { DiaNumero = 7, DiaDeLaSemana = "Domingo" }
            };
            //Dictionary<int, string> Dias = new Dictionary<int, string>() { {1,"Lunes"}, {2,"Martes"}, {3, "Miércoles"}, {4, "Jueves"}, {5, "Viernes"}, {6, "Sábado"}, {7, "Domingo" }};
            //foreach (string dia in Dias)
            //{
            //	Horario horario = new Horario();
            //	horario.DiaDeLaSemana = dia;
            //	lstHorarios.Add(horario);
            //}

            DataTable dt = dsHorarios.Tables[0];

            foreach (DataRow row in dt.Rows)
            {
                //string diaDeLaSemana = Convert.ToDateTime(row["FecEntrada"]).ToString("dddd", CultureInfo.GetCultureInfo("es-AR"));
                //diaDeLaSemana = char.ToUpper(diaDeLaSemana[0]) + diaDeLaSemana.Substring(1);
                try
                {
                    string entrada = Convert.ToDateTime(row["HorEntrada"]).ToString("hh:mm");
                    string salida = Convert.ToDateTime(row["HorSalida"]).ToString("hh:mm");
                    string movil = row["MovilId"].ToString();

                    var hor = lstHorarios.SingleOrDefault(x => x.DiaNumero == Convert.ToInt16(row["DiaSemana"]));
                    if (hor.Entrada1 != null)
                    {
                        hor.Entrada2 = entrada;
                        hor.Salida2 = salida;
                        hor.Movil2 = movil;
                    }
                    else
                    {
                        hor.Entrada1 = entrada;
                        hor.Salida1 = salida;
                        hor.Movil1 = movil;
                        hor.Disponibilidad = Convert.ToByte(row["flgDisponibilidad"]);
                    }
                }
                catch (Exception)
                {

                }

            }

            return lstHorarios;

        }

        //Métodos auxiliares


        private string getFormattedDescriptionOfPeriod(DateTime fec)
        {
            string mes = fec.ToString("MMMM");
            mes = char.ToUpper(mes[0]) + mes.Substring(1);
            string year = fec.ToString("yyyy");
            return mes + " " + year;
        }

        private string getFormattedPeriod(DateTime fec)
        {

            int month = fec.Month;
            string strMonth = month.ToString();

            if (month < 10)
            {
                strMonth = "0" + month.ToString();
            }

            return fec.ToString("yyyy") + strMonth;
        }

        private string getGuardiaFechaFormatted(string fecha, int pOpcion)
        {

            string retVal = "";
            int dia = Convert.ToInt32(fecha.Substring(6, 2));
            int mes = Convert.ToInt32(fecha.Substring(4, 2));
            int año = Convert.ToInt32(fecha.Substring(0, 4));

            DateTime fecFormatted = new DateTime(año, mes, dia);
            string diaDeLaSemana = fecFormatted.ToString("ddd");

            switch (pOpcion)
            {
                case 1:
                    retVal = diaDeLaSemana + " " + dia;
                    break;
                case 2:
                    retVal = mes + "/" + (año.ToString()).Substring(2, 2);
                    break;
                case 3:
                    retVal = dia + "/" + mes + "/" + año;
                    break;
            }

            return retVal;

        }

        [HttpPost]
        public int setRespuestaReclamoGuardia(EstadoReclamoGuardia estadoReclamoGuardia)
        {
            string[] pItm = estadoReclamoGuardia.GuardiaID.Split('|');
            int pSta = estadoReclamoGuardia.Estado;
            string pRta = estadoReclamoGuardia.Respuesta;
            long pUsr = Convert.ToInt32(Session["usr_id"]);
            long pLiq = 0;
            long pDetId = 0;
            long.TryParse(pItm[0], out pLiq);
            long.TryParse(pItm[2], out pDetId);

            // sacar comillas y demas al texto de respuesta
            try
            {
                WSProduccionContratadosLiquidaciones.ContratadosLiquidacionesSoapClient wsCliente = new WSProduccionContratadosLiquidaciones.ContratadosLiquidacionesSoapClient();
                //wsCliente.Endpoint.Behaviors.Add(new MyFaultLogger());
                wsCliente.Open();

                //DataTable dtResultadoReclamo = wsCliente.SetRespuesta($"{pLiq}_{pDetId}", 0, pSta, pRta, pUsr).Tables[0];
                DataTable dtResultadoReclamo = wsCliente.SetRespuestaV2(pLiq, pDetId, 0, pSta, pRta, pUsr).Tables[0];
                //DataTable dtResultadoReclamo = wsCliente.SetRespuestaV2(0, 0, 0, 0, "dsfsd", 1396).Tables[0];
                wsCliente.Abort();
                if (Convert.ToInt32(dtResultadoReclamo.Rows[0]["Resultado"]) == 1)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return 0;
            }

        }

        [HttpPost]
        public int setRespuestaReclamoServicio(EstadoReclamoServicio estadoReclamoServicio)
        {
            string pItm = estadoReclamoServicio.ServicioID;
            int pSta = estadoReclamoServicio.Estado;
            string pRta = estadoReclamoServicio.Respuesta;
            int pUsr = Convert.ToInt32(Session["usr_id"]);

            // sacar comillas y demas al texto de respuesta
            try
            {
                WSProduccionContratadosLiquidaciones.ContratadosLiquidacionesSoapClient wsCliente = new WSProduccionContratadosLiquidaciones.ContratadosLiquidacionesSoapClient();
                wsCliente.Open();
                DataTable dtResultadoReclamo = wsCliente.SetRespuesta(pItm, 1, pSta, pRta, pUsr).Tables[0];
                wsCliente.Abort();
                if (Convert.ToInt32(dtResultadoReclamo.Rows[0]["Resultado"]) == 1)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (FaultException ex)
            {
                logger.Error(ex);
            }
            catch (SoapException ex)
            {
                logger.Error(ex);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return 0;
        }


        // --> Set reclamo de guardia
        [HttpPost]
        public int setReclamoGuardia(EstadoReclamoGuardia estadoReclamoGuardia)
        {
            string pHEnt = "", pMEnt = "", pHSal = "", pMSal = "", pObs = "", pItm = "";
            int pMot = 0, motConDifHoraria = 0, pCnf = 0, pUsr = 0;

            try
            {
                if (estadoReclamoGuardia.Conforme == 0)
                {
                    if (estadoReclamoGuardia.MotivoId == "" || estadoReclamoGuardia.Reclamo == "")
                    {
                        return 0;
                    }

                    pMot = getMotivoDiferencia(estadoReclamoGuardia.MotivoId, 0);
                    motConDifHoraria = getMotivoDiferencia(estadoReclamoGuardia.MotivoId, 1);
                    if (motConDifHoraria == 1)
                    {

                        pHEnt = estadoReclamoGuardia.Entrada.Substring(0, 2);
                        pMEnt = estadoReclamoGuardia.Entrada.Substring(3, 2);
                        pHSal = estadoReclamoGuardia.Salida.Substring(0, 2);
                        pMSal = estadoReclamoGuardia.Salida.Substring(3, 2);

                    }

                    pObs = estadoReclamoGuardia.Reclamo;

                }

                pUsr = Convert.ToInt32(Session["usr_id"]);
                pItm = estadoReclamoGuardia.GuardiaID;
                pCnf = estadoReclamoGuardia.Conforme;

                WSProduccionContratadosLiquidaciones.ContratadosLiquidacionesSoapClient wsClient = new WSProduccionContratadosLiquidaciones.ContratadosLiquidacionesSoapClient();
                wsClient.Open();
                DataTable dtResultadoReclamo = wsClient.SetReclamo(pItm, 0, pCnf, pMot, pHEnt, pMEnt, pHSal, pMSal, pObs, pUsr).Tables[0];
                wsClient.Abort();
                if (Convert.ToInt32(dtResultadoReclamo.Rows[0]["Resultado"]) == 1)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }

            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return 0;
            }

        }


        // --> Set reclamo de guardia
        [HttpPost]
        public int setReclamoServicio(EstadoReclamoServicio estadoReclamoServicio)
        {
            string pObs = "", pItm = "";
            int pCnf = 0, pUsr = 0;

            try
            {
                if (estadoReclamoServicio.Conforme == 0)
                {
                    if (estadoReclamoServicio.Reclamo == "")
                    {
                        return 0;
                    }

                    pObs = estadoReclamoServicio.Reclamo;

                }

                pUsr = Convert.ToInt32(Session["usr_id"]);
                pItm = estadoReclamoServicio.ServicioID;
                pCnf = estadoReclamoServicio.Conforme;

                WSProduccionContratadosLiquidaciones.ContratadosLiquidacionesSoapClient wsClient = new WSProduccionContratadosLiquidaciones.ContratadosLiquidacionesSoapClient();
                wsClient.Open();
                DataTable dtResultadoReclamo = wsClient.SetReclamo(pItm, 1, pCnf, 0, "", "", "", "", pObs, pUsr).Tables[0];
                wsClient.Abort();
                if (Convert.ToInt32(dtResultadoReclamo.Rows[0]["Resultado"]) == 1)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }

            }
            catch (FaultException ex)
            {
                logger.Error(ex);
            }
            catch (SoapException ex)
            {
                logger.Error(ex);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return 0;
        }

        [HttpPost]
        public int setHorarioDisponible(Horario horario)
        {
            MailMessage mail = new MailMessage(Config.MailAddress, Config.MailAddress);
            SmtpClient client = new SmtpClient("smtp.fibertel.com.ar");
            client.Port = 25;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(Config.MailAccount, Config.MailPassword);

            var Dias = new Dictionary<int, string>() { { 1, "Lunes" }, { 2, "Martes" }, { 3, "Miércoles" }, { 4, "Jueves" }, { 5, "Viernes" }, { 6, "Sábado" }, { 7, "Domingo" } };

            mail.Subject = "Serca WEB – Disponibilidad médica";
            mail.Body = string.Format("El Dr. {0} ha notificado su disponibilidad para tomar guardia los días {1} en la franja {2} / {3}{4}{4}Serca WEB", Session["UserName"], Dias[horario.DiaNumero], horario.Entrada1, horario.Salida1, Environment.NewLine);
            client.Send(mail);
            try
            {
                WSProduccionContratadosLiquidaciones.ContratadosLiquidacionesSoapClient wsClient = new WSProduccionContratadosLiquidaciones.ContratadosLiquidacionesSoapClient();
                wsClient.Open();
                DataSet ds = wsClient.SetHorariosDisponibilidad(Convert.ToInt32(Session["usr_id"]), horario.DiaNumero, horario.Entrada1, horario.Salida1);
                wsClient.Abort();
                return 1;
            }
            catch (FaultException ex)
            {
                logger.Error(ex);
            }
            catch (SoapException ex)
            {
                logger.Error(ex);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return 0;
        }

        [HttpPost]
        public int delHorarioDisponible(Horario horario)
        {
            try
            {
                WSProduccionContratadosLiquidaciones.ContratadosLiquidacionesSoapClient wsClient = new WSProduccionContratadosLiquidaciones.ContratadosLiquidacionesSoapClient();
                wsClient.Open();
                DataSet ds = wsClient.DelHorariosDisponibilidad(Convert.ToInt32(Session["usr_id"]), horario.DiaNumero);
                wsClient.Abort();
                return 1;
            }
            catch (FaultException ex)
            {
                logger.Error(ex);
            }
            catch (SoapException ex)
            {
                logger.Error(ex);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return 0;
        }

        private int getMotivoDiferencia(string pMot, int idx)
        {
            return Convert.ToInt32(pMot.Split('/')[idx]);
        }

        [HttpPost]
        public int SetCoordinacionHorario(m.HorarioCoordinacion horario)
        {
            try
            {
                WSProduccionContratadosLiquidaciones.ContratadosLiquidacionesSoapClient wsClient = new WSProduccionContratadosLiquidaciones.ContratadosLiquidacionesSoapClient();
                wsClient.Open();
                DataSet ds = wsClient.SetHorarioGrilla(horario.Id, horario.PersonalId, horario.FecEntrada.FormatedDateToAnsi(), horario.HorEntrada, horario.FecSalida.FormatedDateToAnsi(), horario.HorSalida, horario.MovilId, 0, "", Convert.ToInt32(Session["usr_id"]));
                wsClient.Abort();
                return 1;
            }
            catch (FaultException ex)
            {
                logger.Error(ex);
            }
            catch (SoapException ex)
            {
                logger.Error(ex);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return 0;
        }

        [HttpPost]
        public int DelCoordinacionHorario(int id)
        {
            try
            {
                WSProduccionContratadosLiquidaciones.ContratadosLiquidacionesSoapClient wsClient = new WSProduccionContratadosLiquidaciones.ContratadosLiquidacionesSoapClient();
                wsClient.Open();
                DataSet ds = wsClient.DelHorarioGrilla(id, Convert.ToInt32(Session["usr_id"]));
                wsClient.Abort();
                return 1;
            }
            catch (FaultException ex)
            {
                logger.Error(ex);
            }
            catch (SoapException ex)
            {
                logger.Error(ex);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return 0;
        }


        public JsonResult GetGuardiasPotenciales()
        {
            DataSet dsPotenciales = getGuardiasPotencialesFromWebService();
            GuardiaPotencialViewModel guardiaPotencialViewModel = new GuardiaPotencialViewModel();
            if (dsPotenciales == null)
            {
                guardiaPotencialViewModel = null;
            }
            else
            {
                guardiaPotencialViewModel = getGuardiaPotencialViewModelFormatted(dsPotenciales);
            }

            return Json(guardiaPotencialViewModel, JsonRequestBehavior.AllowGet);
        }

        private DataSet getGuardiasPotencialesFromWebService()
        {
            try
            {
                WSContratadosLiquidaciones.ContratadosLiquidacionesSoapClient wsClient = new WSContratadosLiquidaciones.ContratadosLiquidacionesSoapClient();
                wsClient.Open();
                DataSet ds = wsClient.GetGuardiasPotenciales();
                wsClient.Abort();
                return ds;
            }
            catch (FaultException ex)
            {
                logger.Error(ex);
            }
            catch (SoapException ex)
            {
                logger.Error(ex);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return null;
        }


        private GuardiaPotencialViewModel getGuardiaPotencialViewModelFormatted(DataSet dsPotenciales)
        {
            GuardiaPotencialViewModel guardiaPotencialViewModel = new GuardiaPotencialViewModel();
            List<GuardiaPotencial> lstPotenciales = new List<GuardiaPotencial>();

            DataTable dtPotenciales = dsPotenciales.Tables[0];

            foreach (DataRow dtRow in dtPotenciales.Rows)
            {
                GuardiaPotencial potencial = new GuardiaPotencial();
                potencial.PotencialID = Convert.ToInt32(dtRow["PotencialID"]);

                potencial.Modalidad = new Filtrable();
                potencial.Modalidad.Id = Convert.ToInt32(dtRow["ModalidadId"]);
                potencial.Modalidad.Descripcion = dtRow["ModalidadDescripcion"].ToString();

                potencial.Medico = new Filtrable();
                potencial.Medico.Id = Convert.ToInt32(dtRow["MedicoId"]);
                potencial.Medico.Descripcion = dtRow["MedicoDescripcion"].ToString();

                potencial.Dia = new Filtrable();
                potencial.Dia.Id = Convert.ToInt32(dtRow["DiaSemana"]);
                potencial.Dia.Descripcion = DateTimeFormatInfo.CurrentInfo.DayNames[potencial.Dia.Id == 7 ? 0 : potencial.Dia.Id].ToUpper();

                potencial.Zona = new Filtrable();
                potencial.Zona.Id = Convert.ToInt32(dtRow["ZonaId"]);
                potencial.Zona.Descripcion = dtRow["ZonaDescripcion"].ToString();

                potencial.Localidad = dtRow["Localidad"].ToString();
                potencial.HoraEntrada = dtRow["HorEntrada"].ToString();
                potencial.HoraSalida = dtRow["HorSalida"].ToString();
                potencial.FechaCarga = dtRow["FechaCarga"].ToString();
                lstPotenciales.Add(potencial);
            }

            //lstPotenciales = new List<GuardiaPotencial> {
            //    new GuardiaPotencial
            //    {
            //        PotencialID = 1,
            //        Modalidad = new Filtrable { Id=1, Descripcion = "Modalidad1"},
            //        Medico = new Filtrable { Id = 1, Descripcion = "Medico1" },
            //        Dia = "Jueves",
            //        Zona = new Filtrable { Id = 1, Descripcion = "GBA NOE" },
            //        Localidad = "Jose C Paz",
            //        HoraEntrada = "10:00",
            //        HoraSalida = "18:00"
            //    },
            //    new GuardiaPotencial
            //    {
            //        PotencialID = 2,
            //        Modalidad = new Filtrable { Id = 2, Descripcion = "Modalidad2" },
            //        Medico = new Filtrable { Id = 2, Descripcion = "Medico2" },
            //        Dia = "Domingo",
            //        Zona = new Filtrable { Id = 2, Descripcion = "GBA SE" },
            //        Localidad = "Lanus",
            //        HoraEntrada = "09:00",
            //        HoraSalida = "17:00"
            //    },
            //    new GuardiaPotencial
            //    {
            //        PotencialID = 3,
            //        Modalidad = new Filtrable { Id = 3, Descripcion = "Modalidad3" },
            //        Medico = new Filtrable { Id = 3, Descripcion = "Medico3" },
            //        Dia = "Martes",
            //        Zona = new Filtrable { Id = 3, Descripcion = "GBA O" },
            //        Localidad = "Moreno",
            //        HoraEntrada = "11:00",
            //        HoraSalida = "19:00",
            //    }
            //};

            guardiaPotencialViewModel.GuardiasPotenciales = lstPotenciales;

            guardiaPotencialViewModel.Modalidades = lstPotenciales.GroupBy(i => i.Modalidad.Id, (key, group) => group.First().Modalidad).ToList();
            guardiaPotencialViewModel.Medicos = lstPotenciales.GroupBy(i => i.Medico.Id, (key, group) => group.First().Medico).ToList();
            guardiaPotencialViewModel.Zonas = lstPotenciales.GroupBy(i => i.Zona.Id, (key, group) => group.First().Zona).ToList();
            guardiaPotencialViewModel.Dias = lstPotenciales.GroupBy(i => i.Dia.Id, (key, group) => group.First().Dia).OrderBy(dia=>dia.Id).ToList();

            guardiaPotencialViewModel.Modalidades.Insert(0, new Filtrable { Id = 0, Descripcion = "Seleccione una Modalidad" });
            guardiaPotencialViewModel.Medicos.Insert(0, new Filtrable { Id = 0, Descripcion = "Seleccione un Medico" });
            guardiaPotencialViewModel.Zonas.Insert(0, new Filtrable { Id = 0, Descripcion = "Seleccione una Zona" });
            guardiaPotencialViewModel.Dias.Insert(0, new Filtrable { Id = 0, Descripcion = "Seleccione un Dia" });

            return guardiaPotencialViewModel;

        }
    }
}
