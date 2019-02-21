using NLog;
using ExtranetAppsOmniSerca.Helpers;
using ExtranetAppsOmniSerca.Models;
using System;
using System.Data;
using System.ServiceModel;
using System.Web.Mvc;
using System.Web.Services.Protocols;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using ExtranetAppsOmniSerca.ViewModels;
using Newtonsoft.Json;
using System.Collections.Specialized;
using System.Net.Mail;
using System.Net.Mime;

namespace ExtranetAppsOmniSerca.Controllers
{
    public class AfiliacionesController : Controller
    {

        private Logger logger = LogManager.GetCurrentClassLogger();

        public enum Accesos
        {
            //1: vendedor => Create/Edit/View (No ve afiliar).
            //2: visualizador => Puede ver todo, no puede modificar
            //3: administrador => View(Afiliar habilitado si los registros están completos)
            SinAcceso = 0, Vendedor = 1, Visualizador = 2, Administrador = 3
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Gestión de Afiliaciones";

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
                WSVendedores.VendedoresSoapClient wsClient = new WSVendedores.VendedoresSoapClient();
                wsClient.Open();
                DataTable dtUsuario = wsClient.GetUsuarioValidacion(usr).Tables[0];
                wsClient.Abort();
                if (Convert.ToInt32(dtUsuario.Rows[0]["Acceso"]) == 0)
                    return false;
                else
                {
                    Session["usr_id"] = usr;
                    Session["UserName"] = dtUsuario.Rows[0]["NombreUsuario"].ToString();
                    //1: vendedor => Create/Edit/View (No ve afiliar).
                    //2: visualizador => Puede ver todo, no puede modificar
                    //3: administrador => View(Afiliar habilitado si los registros están completos)

                    Session["Acceso"] = Convert.ToInt32(dtUsuario.Rows[0]["Acceso"]);

                    Session["page"] = "Afiliaciones";
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

        //public string GetAccesoDesc()
        //{
        //    return ((Accesos)((Session["acceso"] == null) ? 0 : Session["acceso"])).ToString();
        //}
        public Accesos GetAccesoDesc()
        {
            try
            {
                //Session["acceso"] = 1;
                return (Accesos)((Session["acceso"] == null) ? 0 : Session["acceso"]);
            }
            catch (Exception ex)
            {
                return (Accesos)0;
            }
        }

        public JsonResult ValidateCUIT(string cuit, int clienteId)
        {
            try
            {
                WSVendedores.VendedoresSoapClient wsClient = new WSVendedores.VendedoresSoapClient();
                wsClient.Open();
                DataSet ds = wsClient.ValidateCUIT(Convert.ToInt32(Session["usr_id"]), cuit, clienteId);
                wsClient.Close();

                //if (Convert.ToInt64(ds.Tables[0].Rows[0]["Resultado"]) == 1)
                //    return Json(new { Success = true, Message = "" }, JsonRequestBehavior.AllowGet);
                //else //Convert.ToInt64(ds.Tables[0].Rows[0]["RazonSocial"]);
                //    return Json(new { Success = false, Message = ds.Tables[0].Rows[0]["AlertaError"].ToString() }, JsonRequestBehavior.AllowGet);

                string razonSocial = ds.Tables[0].Rows[0]["RazonSocial"].ToString();
                if (Convert.ToInt64(ds.Tables[0].Rows[0]["Resultado"]) == 1)
                {
                    if (string.IsNullOrEmpty(razonSocial))
                        return Json(new { Success = true, Message = "El CUIT es valido para un nuevo cliente." }, JsonRequestBehavior.AllowGet);
                    else
                        return Json(new { Success = true, Message = string.Format("El CUIT es valido, corresponde al cliente {0} que se dio de baja.", razonSocial) }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { Success = false, Message = string.Format("Este CUIT ya existe (cliente activo o potencial / preparado). Corresponde al cliente {0}.", razonSocial) }, JsonRequestBehavior.AllowGet);
            }
            catch (SoapException ex)
            {
                logger.Error(ex);
                return Json(new { Success = false, Message = "No se pudo validar el Cuit. Error de Servicio." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return Json(new { Success = false, Message = "No se pudo validar el Cuit." }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult GetClientesSegunEstado(int pTipoCliente, string pDescripcion, int pUsuarioId)
        {
            int usuarioId = Convert.ToInt32(Session["usr_id"]);
            int acceso = Convert.ToInt32(Session["acceso"]);
            if (pUsuarioId != 0)
            {
                usuarioId = pUsuarioId;
                acceso = (int)Accesos.Vendedor;
            }

            WSVendedores.VendedoresSoapClient wsClient = new WSVendedores.VendedoresSoapClient();
            wsClient.Open();

            DataSet ds = wsClient.GetClientes(usuarioId, acceso, pTipoCliente);
            wsClient.Close();

            List<Cliente> lstClientes = new List<Cliente>();
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                    lstClientes.Add(new Cliente(item));

                //if (pTipoCliente != (int)Estados.Todos)
                //    lstClientes = lstClientes.Where(c => c.Estado == pTipoCliente).ToList();

                if (!string.IsNullOrEmpty(pDescripcion))
                    lstClientes = lstClientes.Where(c => c.RazonSocial.ToUpper().Contains(pDescripcion.Trim().ToUpper())
                                            || c.NombreComercial.ToUpper().Contains(pDescripcion.Trim().ToUpper())
                                            || c.CredencialID.ToUpper().Contains(pDescripcion.Trim().ToUpper())).ToList();
            }
            return Json(lstClientes, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetSolicitudDigitalizada(int pClienteId)
        {
            SolicitudDigitalizada oSolicitudDigitalizada = new SolicitudDigitalizada();
            WSVendedores.VendedoresSoapClient wsClient = new WSVendedores.VendedoresSoapClient();
            try
            {
                wsClient.Open();
                DataRowCollection dr = wsClient.GetProductos(pClienteId).Tables[0].Rows;

                oSolicitudDigitalizada.Productos = new List<Producto>();
                foreach (DataRow item in dr)
                    oSolicitudDigitalizada.Productos.Add(new Producto(item));

                if (pClienteId != 0)
                {
                    try { oSolicitudDigitalizada.EntidadCliente = new Cliente(wsClient.GetClienteById(Convert.ToInt32(Session["usr_id"]), pClienteId, Convert.ToInt32(Session["acceso"])).Tables[0].Rows[0]); }
                    catch (Exception) { wsClient.Close(); return null; }

                    DataSet dsOriginal = wsClient.GetSolicitudDigitalizada(pClienteId);
                    wsClient.Close();

                    List<string> prefixes = SolicitudDigitalizada.GetAllPrefix();
                    DataSet ds = Helpers.Data.GetDataSetByPrefixes(dsOriginal, prefixes);

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt;

                        dt = ds.Tables[SolicitudDigitalizadaBasico.Prefix];
                        if (dt != null)
                            oSolicitudDigitalizada.SolicitudDigitalizadaBasico = new SolicitudDigitalizadaBasico(dt.Rows[0]);


                        dt = ds.Tables[AreaProtegida.Prefix];
                        if (dt != null)
                            oSolicitudDigitalizada.AreaProtegida = new AreaProtegida(dt.Rows[0]);

                        dt = ds.Tables[DatosAdministrativos.Prefix];
                        if (dt != null)
                        {
                            oSolicitudDigitalizada.DatosAdministrativos = new DatosAdministrativos(dt.Rows[0]);

                            dt = ds.Tables[TarjetaDeCredito.Prefix];
                            if (dt != null)
                            {
                                oSolicitudDigitalizada.DatosAdministrativos.TarjetaDeCredito = new TarjetaDeCredito(dt.Rows[0]);
                                if (!string.IsNullOrEmpty(oSolicitudDigitalizada.DatosAdministrativos.TarjetaDeCredito.Numero))
                                {
                                    if (lstTipoDeTarjeta.Count == 0) GetTipoDeTarjetas();
                                    string mascara = lstTipoDeTarjeta.Where(x => x.ID == oSolicitudDigitalizada.DatosAdministrativos.TarjetaDeCredito.TipoDeTarjeta.ID).FirstOrDefault().MascaraIngreso.Replace(" ", "-");
                                    oSolicitudDigitalizada.DatosAdministrativos.TarjetaDeCredito.TipoDeTarjeta.MascaraIngreso = mascara;
                                    oSolicitudDigitalizada.DatosAdministrativos.TarjetaDeCredito.Numero = Convert.ToInt64(oSolicitudDigitalizada.DatosAdministrativos.TarjetaDeCredito.Numero).ToString(mascara.Replace("9", "0"));
                                }
                            }


                            dt = ds.Tables[DebitoEnCuenta.Prefix];
                            if (dt != null)
                                oSolicitudDigitalizada.DatosAdministrativos.DebitoEnCuenta = new DebitoEnCuenta(dt.Rows[0]);

                            if (oSolicitudDigitalizada.DatosAdministrativos.Firmado)
                            {
                                //oSolicitudDigitalizada.DatosAdministrativos.FirmaBase64 = "data:image/png;base64,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";
                                oSolicitudDigitalizada.DatosAdministrativos.FirmaBase64 = GetFirma(oSolicitudDigitalizada.EntidadCliente.ClienteId);
                            }
                        }

                        dt = ds.Tables[ClienteContacto.Prefix];
                        if (dt != null)
                            oSolicitudDigitalizada.ClienteContacto = new ClienteContacto(dt.Rows[0]);
                    }
                    else wsClient.Close();
                }
                else wsClient.Close();
            }
            catch (SoapException ex)
            {
                wsClient.Close();
                logger.Error(ex);
                return Json(new { Success = false, Message = "Error al obtener los datos del Cliente. Error de Servicio." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                wsClient.Close();
                logger.Error(ex);
                return Json(new { Success = false, Message = "Error al obtener los datos del Cliente." }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Success = true, Message = "OK.", Solicitud = oSolicitudDigitalizada }, JsonRequestBehavior.AllowGet);
        }

        //private DataSet GetDataSetByPrefixes(DataSet ds, List<string> listPrefixes)
        //{
        //    DataSet dsSplit = new DataSet();

        //    foreach (string prefix in listPrefixes)
        //    {
        //        DataTable dt = GetTableByPrefix(ds.Tables[0], prefix);
        //        if (dt != null)
        //            dsSplit.Tables.Add(dt);
        //    }
        //    return dsSplit;
        //}

        //private DataTable GetTableByPrefix(DataTable table, string prefix)
        //{
        //    string[] cols = (from dc in table.Columns.Cast<DataColumn>()
        //                     where dc.ColumnName.Contains(prefix)
        //                     select dc.ColumnName)
        //        .ToArray();

        //    if (cols.Count() != 0)
        //    {
        //        DataView view = new DataView(table);
        //        DataTable selected = view.ToTable(prefix, false, cols);

        //        foreach (DataColumn column in selected.Columns)
        //            column.ColumnName = column.ColumnName.Replace(prefix, "");

        //        return selected;
        //    }
        //    else
        //        return null;
        //}

        #region Combos
        public JsonResult GetRubros()
        {
            List<Rubro> lstRubro = new List<Rubro>();

            WSVendedores.VendedoresSoapClient wsClient = new WSVendedores.VendedoresSoapClient();
            wsClient.Open();
            DataSet ds = wsClient.GetRubros();
            wsClient.Close();
            foreach (DataRow item in ds.Tables[0].Rows)
                lstRubro.Add(new Rubro(item));

            return Json(lstRubro, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCondicionIVAs()
        {
            try
            {
                List<CondicionIVA> lstCondicionIVA = new List<CondicionIVA>();

                WSVendedores.VendedoresSoapClient wsClient = new WSVendedores.VendedoresSoapClient();
                wsClient.Open();
                DataSet ds = wsClient.GetCondicionesIva();
                wsClient.Close();

                foreach (DataRow item in ds.Tables[0].Rows)
                    lstCondicionIVA.Add(new CondicionIVA(item));

                return Json(lstCondicionIVA, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        static List<FormaDePago> lstFormaDePago;
        public JsonResult GetFormaDePagos()
        {

            WSVendedores.VendedoresSoapClient wsClient = new WSVendedores.VendedoresSoapClient();
            wsClient.Open();
            DataSet ds = wsClient.GetFormasPago();
            wsClient.Close();
            lstFormaDePago = new List<FormaDePago>();

            foreach (DataRow item in ds.Tables[0].Rows)
                lstFormaDePago.Add(new FormaDePago(item));

            return Json(lstFormaDePago, JsonRequestBehavior.AllowGet);
        }

        private string GetFirma(long clienteId)
        {
            try
            {
                WSVendedores.VendedoresSoapClient wsClient = new WSVendedores.VendedoresSoapClient();
                wsClient.Open();
                return wsClient.GetFirma(clienteId);
            }
            catch (Exception)
            {
                return "";
            }
        }
        static List<TipoDeTarjeta> lstTipoDeTarjeta;
        public JsonResult GetTipoDeTarjetas()
        {


            lstTipoDeTarjeta = new List<TipoDeTarjeta>();
            WSVendedores.VendedoresSoapClient wsClient = new WSVendedores.VendedoresSoapClient();
            wsClient.Open();
            DataSet ds = wsClient.GetTarjetasCredito();
            wsClient.Close();

            foreach (DataRow item in ds.Tables[0].Rows)
                lstTipoDeTarjeta.Add(new TipoDeTarjeta(item));

            return Json(lstTipoDeTarjeta, JsonRequestBehavior.AllowGet);
        }

        static List<TipoCuenta> lstTipoCuenta;
        public JsonResult GetTipoCuentas()
        {

            lstTipoCuenta = new List<TipoCuenta>();
            WSVendedores.VendedoresSoapClient wsClient = new WSVendedores.VendedoresSoapClient();
            wsClient.Open();
            DataSet ds = wsClient.GetTiposCuentas();
            wsClient.Close();

            foreach (DataRow item in ds.Tables[0].Rows)
                lstTipoCuenta.Add(new TipoCuenta(item));

            return Json(lstTipoCuenta, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBancos()
        {
            List<Banco> lstBanco = new List<Banco>();

            WSVendedores.VendedoresSoapClient wsClient = new WSVendedores.VendedoresSoapClient();
            wsClient.Open();
            DataSet ds = wsClient.GetBancos();
            wsClient.Close();
            foreach (DataRow item in ds.Tables[0].Rows)
                lstBanco.Add(new Banco(item));

            return Json(lstBanco, JsonRequestBehavior.AllowGet);
        }
        #endregion
        [HttpPost]
        public JsonResult Afiliar(long pCli, string pEmail)
        {
            //TODO: Terminar, ver mensaje
            try
            {
                WSVendedores.VendedoresSoapClient wsClient = new WSVendedores.VendedoresSoapClient();
                wsClient.Open();
                DataSet ds = wsClient.SetClienteActivo(pCli, Convert.ToInt32(Session["usr_id"]));
                wsClient.Close();
                string mensaje = "";

                if (Convert.ToInt64(ds.Tables[0].Rows[0]["Resultado"]) == 1 || (Convert.ToInt64(ds.Tables[0].Rows[0]["ClienteId"]) > 0))
                {
                    mensaje = "Cliente afiliado con Exito";

                    Tuple<bool, string> res = EnviarEmail(ds.Tables[0].Rows[0]["CodigoCliente"].ToString(), ds.Tables[0].Rows[0]["RazonSocial"].ToString(), TipoNoficacion.activo);
                    if (!res.Item1)
                        mensaje += "( Email a Responsable: " + res.Item2.ToUpper() + ")";

                    if (!string.IsNullOrEmpty(pEmail))
                        res = EnviarEmail(pCli.ToString(), "", TipoNoficacion.contrato, pEmail);

                    if (!res.Item1)
                        mensaje += " ( Email a Cliente: " + res.Item2.ToUpper() + ")";

                    return Json(new { Success = true, Message = mensaje }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    mensaje = ds.Tables[0].Rows[0]["AlertaError"].ToString();
                    return Json(new { Success = false, Message = mensaje }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return Json(new { Success = false, Message = "No se pudo afiliar al Cliente." }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetLocalidades()
        {
            return Json(Localidad.ListLocalidades, JsonRequestBehavior.AllowGet);
        }


        //No funciona si el objeto no tiene un constructor vacio.
        [HttpPost]
        public ActionResult GuardarSolicitud(SolicitudDigitalizada solicitud)
        {
            try
            {
                if (Convert.ToInt64(Session["usr_id"]) == 0)
                    return Json(new { Success = false, Message = "No se pudo guardar el cliente. Sesion invalida." }, JsonRequestBehavior.AllowGet);

                long cienteId = solicitud.EntidadCliente.ClienteId;
                var mensaje = "";

                //string result = ValidateCUIT(solicitud.EntidadCliente.Cuit).Data.ToString();
                ////MyJsonResult result = jsonResult as MyJsonResult;

                //if (!result.Contains("Success = True"))
                //    return Json(new { Success = false, Message = string.Format("No se pudo guardar el cliente. {0}", result.Substring(result.IndexOf("Message") + 10)) }, JsonRequestBehavior.AllowGet);

                //Buscar error en Localidad.
                if (solicitud.EntidadCliente.Localidad == null || solicitud.AreaProtegida.Localidad == null)
                    mensaje = solicitud.EntidadCliente.Localidad == null && solicitud.AreaProtegida.Localidad == null ?
                                "(verificar localidad del Cliente y el Area Protegida). " :
                                solicitud.EntidadCliente.Localidad == null ?
                                    "(verificar localidad del Cliente). " :
                                    "(verificar localidad del Area Protegida). ";


                DataSet ds = SetClientePotencial(solicitud);

                //TODO: Ver resultado ambiguo (resultado=0, newClienteId=xxx, MensajeError=1)
                if (Convert.ToInt64(ds.Tables[0].Rows[0]["Resultado"]) == 1 || (Convert.ToInt64(ds.Tables[0].Rows[0]["ClienteId"]) > 0 && solicitud.EntidadCliente.ClienteId == 0))
                {
                    if (solicitud.EntidadCliente.ClienteId == 0)
                    {
                        solicitud.EntidadCliente.ClienteId = Convert.ToInt64(ds.Tables[0].Rows[0]["ClienteId"]);
                        mensaje += "Se creo con exito el cliente Nro: " + solicitud.EntidadCliente.ClienteId;
                    }
                    else
                        mensaje += "Se actualizo con exito el cliente Nro: " + solicitud.EntidadCliente.ClienteId;
                    if (solicitud.EntidadCliente.EsPreparado == 1)
                    {
                        Tuple<bool, string> res = EnviarEmail(null, solicitud.EntidadCliente.RazonSocial, TipoNoficacion.preparado);
                        if (!res.Item1)
                            mensaje += ". (" + res.Item2.ToUpper() + ")";
                    }

                    return Json(new { Success = true, Message = mensaje, ClienteId = solicitud.EntidadCliente.ClienteId }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    mensaje = ds.Tables[0].Rows[0]["AlertaError"].ToString();
                    return Json(new { Success = false, Message = mensaje }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (SoapException ex)
            {
                logger.Error(ex);
                return Json(new { Success = false, Message = "No se pudo guardar el cliente. Error de Servicio." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return Json(new { Success = false, Message = "No se pudo guardar el cliente." }, JsonRequestBehavior.AllowGet);
            }
        }

        internal enum TipoNoficacion { preparado, activo, contrato }

        internal Tuple<bool, string> EnviarEmail(string codigoCliente, string razonSocial, TipoNoficacion tipoNotificacion, string clienteEmail = "")
        {
            string error = "";
            bool result = true;

            if (result)
            {
                try
                {
                    NameValueCollection appSettings = System.Web.Configuration.WebConfigurationManager.AppSettings;

                    string To;
                    string Subject;
                    string Body;
                    List<string> PathFiles = new List<string>();

                    if (tipoNotificacion == TipoNoficacion.preparado)
                    {
                        To = appSettings.Get("AfiliacionesPreparadoMailTo");
                        Subject = appSettings.Get("AfiliacionesPreparadoMailSubject");
                        Body = string.Format(appSettings.Get("AfiliacionesPreparadoMailBody"), razonSocial, Session["UserName"].ToString());
                        EmailHelpers.Send(To, Subject, Body, PathFiles);
                    }
                    else if (!string.IsNullOrEmpty(codigoCliente) && tipoNotificacion == TipoNoficacion.activo)
                    {
                        To = appSettings.Get("AfiliacionesActivoMailTo");
                        Subject = string.Format(appSettings.Get("AfiliacionesActivoMailSubject"));
                        Body = string.Format(appSettings.Get("AfiliacionesActivoMailBody"), razonSocial, codigoCliente, Session["UserName"].ToString());

                        EmailHelpers.Send(To, Subject, Body, PathFiles);
                    }
                    else if (!string.IsNullOrEmpty(codigoCliente) && tipoNotificacion == TipoNoficacion.contrato && !string.IsNullOrEmpty(clienteEmail))
                    {
                        To = clienteEmail;
                        Subject = appSettings.Get("AfiliacionesContratoMailSubject");// "Contratación GRUPO SERCA";
                        Body = appSettings.Get("AfiliacionesContratoMailBody"); //"Por medio del presente correo le adjuntamos la constancia de contratación de nuestros servicios. Si tiene alguna duda al respecto, no dude en comunicarse con su ejecutivo de cuentas.<br /><br /> Le agradecemos por confiar en GRUPO SERCA. <br />Bienvenido!";

                        WSShamanFecae.WSShamanFECAESoapClient client = new WSShamanFecae.WSShamanFECAESoapClient();
                        byte[] fileByte = client.GetContratoVenta(codigoCliente);//clienteId
                        if (fileByte == null)
                        {
                            error = "Error al obtener contrato";
                            result = false;
                        }
                        else
                        {
                            string attachmentTitle = appSettings.Get("AfiliacionesContratoAttachmentTitle");

                            FileContentResult filePdf = File(fileByte, MediaTypeNames.Application.Pdf, attachmentTitle);
                            ContentType contentType = new ContentType(MediaTypeNames.Application.Pdf);
                            MemoryStream attachmentStream = new MemoryStream(filePdf.FileContents);
                            Attachment attachment = new Attachment(attachmentStream, attachmentTitle, contentType.ToString());

                            EmailHelpers.Send(To, Subject, Body, null, attachment);
                        }
                    }
                    else
                    {
                        error = "Error al enviar email, los datos del cliente no son validos.";
                        result = false;
                    }

                }
                catch (Exception ex)
                {
                    logger.Error("Fallo al enviar el email. " + ex.Message);
                    error = "Fallo al enviar el email.";
                    result = false;
                }
            }
            return Tuple.Create(result, error);
        }

        private DataSet SetClientePotencial(SolicitudDigitalizada solicitud)
        {

            long pUsr = Convert.ToInt64(Session["usr_id"]);

            long pId = solicitud.EntidadCliente.ClienteId;//Si es 0 hace insert.

            //SolicitudDigitalizadaBasico
            //if (solicitud.SolicitudDigitalizadaBasico == null)
            //    solicitud.SolicitudDigitalizadaBasico = new SolicitudDigitalizadaBasico();
            //if (solicitud.EntidadCliente.ClienteId == 0)
            if (solicitud.SolicitudDigitalizadaBasico.FechaIngreso == DateTime.Parse("01/01/0001 00:00:00"))
                solicitud.SolicitudDigitalizadaBasico.FechaIngreso = DateTime.Now;
            if (solicitud.SolicitudDigitalizadaBasico.FechaVencimiento == DateTime.Parse("01/01/0001 00:00:00"))
                solicitud.SolicitudDigitalizadaBasico.FechaVencimiento = DateTime.Now.AddYears(5);

            long pFecIng = solicitud.SolicitudDigitalizadaBasico.FechaIngreso.ToString("dd/MM/yyyy").FormatedDateToAnsi();
            long pFecVto = solicitud.SolicitudDigitalizadaBasico.FechaVencimiento.ToString("dd/MM/yyyy").FormatedDateToAnsi();
            string pNroCto = ToStringNotNull(solicitud.SolicitudDigitalizadaBasico.NroSolicitud.ToString());

            #region Cliente
            string pCodCli = ToStringNotNull(solicitud.EntidadCliente.CredencialID);
            string pTitRaz = ToStringNotNull(solicitud.EntidadCliente.RazonSocial);
            string pTitFan = ToStringNotNull(solicitud.EntidadCliente.NombreComercial);
            string pCuit = ToStringNotNull(solicitud.EntidadCliente.Cuit);
            long pActCli = solicitud.EntidadCliente.Rubro == null ? 0 : solicitud.EntidadCliente.Rubro.ID;
            string pSitIva = solicitud.EntidadCliente.CondicionIVA == null ? "" : ToStringNotNull(solicitud.EntidadCliente.CondicionIVA.ID);
            long pNroDoc = solicitud.EntidadCliente.CuitNumber;
            long pPrep = solicitud.EntidadCliente.EsPreparado;

            //Domicilio Legal
            string pFacDm1 = ToStringNotNull(solicitud.EntidadCliente.Calle);
            long pFacDm2 = solicitud.EntidadCliente.Altura;
            long pFacDm3 = solicitud.EntidadCliente.Piso;
            string pFacDm4 = ToStringNotNull(solicitud.EntidadCliente.Depto);
            string pFacRef = ToStringNotNull(solicitud.EntidadCliente.Referencia);
            string pFacEt1 = ToStringNotNull(solicitud.EntidadCliente.EntreCalle1);
            string pFacEt2 = ToStringNotNull(solicitud.EntidadCliente.EntreCalle2);
            string pFacLat = ToStringNotNull(solicitud.EntidadCliente.Latitud).Replace(",", ".");// Convert.ToDecimal(solicitud.AreaProtegida.Latitud);
            string pFacLon = ToStringNotNull(solicitud.EntidadCliente.Longitud).Replace(",", ".");//Convert.ToDecimal(solicitud.AreaProtegida.Longitud);
            string pFacCP = ToStringNotNull(solicitud.EntidadCliente.CP);
            long pFacLoc = solicitud.EntidadCliente.Localidad == null ? 1 : Convert.ToInt64(solicitud.EntidadCliente.Localidad.value);

            #endregion

            //ClienteContacto
            string pccNom = ToStringNotNull(solicitud.ClienteContacto.NombreCompleto);
            string pccCar = ToStringNotNull(solicitud.ClienteContacto.Cargo);
            string pccTel = ToStringNotNull(solicitud.ClienteContacto.Telefono);
            string pccMail = ToStringNotNull(solicitud.ClienteContacto.Email);
            string pccTel2 = ToStringNotNull(solicitud.ClienteContacto.TelefonoAlternativo);
            string pccMail2 = ToStringNotNull(solicitud.ClienteContacto.EmailAlternativo);

            //Area Protegida
            int pDomLegCob = solicitud.AreaProtegida.FlgDomicilioLegalCobertura;
            string pTitDm1 = ToStringNotNull(solicitud.AreaProtegida.Calle);
            long pTitDm2 = solicitud.AreaProtegida.Altura;
            long pTitDm3 = solicitud.AreaProtegida.Piso;
            string pTitDm4 = ToStringNotNull(solicitud.AreaProtegida.Depto);
            string pTitRef = ToStringNotNull(solicitud.AreaProtegida.Referencia);
            string pTitEt1 = ToStringNotNull(solicitud.AreaProtegida.EntreCalle1);
            string pTitEt2 = ToStringNotNull(solicitud.AreaProtegida.EntreCalle2);
            string pTitLat = ToStringNotNull(solicitud.AreaProtegida.Latitud).Replace(",", ".");// Convert.ToDecimal(solicitud.EntidadCliente.Latitud);
            string pTitLon = ToStringNotNull(solicitud.AreaProtegida.Longitud).Replace(",", ".");// Convert.ToDecimal(solicitud.EntidadCliente.Longitud);
            string pTitCP = ToStringNotNull(solicitud.AreaProtegida.CP);//DEBE SETEARSE DEL MAPA??
            long pTitLoc = solicitud.AreaProtegida.Localidad == null ? 1 : Convert.ToInt64(solicitud.AreaProtegida.Localidad.value);
            string pTitTel = ToStringNotNull(solicitud.AreaProtegida.Telefono);

            #region DatosAdministrativos

            decimal pImpMen = solicitud.DatosAdministrativos.ImporteMensual;
            decimal pImpAdl = solicitud.DatosAdministrativos.Adelanto;
            long pPrxPag = solicitud.DatosAdministrativos.ProximoPago == null ? DateTime.Now.ToString("dd/MM/yyyy").FormatedDateToAnsi(4) : solicitud.DatosAdministrativos.ProximoPago.FormatedDateToAnsi(4);//Ver formato requerido ==>29991231?
            long pFpg = solicitud.DatosAdministrativos.FormaDePago == null ? 1 : solicitud.DatosAdministrativos.FormaDePago.ID;

            string pTarId = string.Empty;
            string pTarNro = string.Empty;
            string pTarTit = string.Empty;
            string pTarDoc = string.Empty;
            long pTarVto = 0;


            if (solicitud.DatosAdministrativos.FormaDePago != null && lstFormaDePago.Where(x => x.ID == solicitud.DatosAdministrativos.FormaDePago.ID).FirstOrDefault().Descripcion == "TARJETA DE CREDITO")
            {
                pTarId = ToStringNotNull(solicitud.DatosAdministrativos.TarjetaDeCredito.TipoDeTarjeta.ID);
                pTarNro = ToStringNotNull(solicitud.DatosAdministrativos.TarjetaDeCredito.Numero.Replace("-", ""));
                pTarVto = solicitud.DatosAdministrativos.TarjetaDeCredito.Vencimiento.FormatedDateToAnsi(4);//Ver formato requerido ==>29991231 (OK)
                pTarTit = ToStringNotNull(solicitud.DatosAdministrativos.TarjetaDeCredito.NombreTitular);
                pTarDoc = ToStringNotNull(solicitud.DatosAdministrativos.TarjetaDeCredito.NroDNI);
            }

            string pCtaBco = string.Empty;
            string pCtaNro = string.Empty;
            string pCtaRaz = string.Empty;
            string pCtaCti = string.Empty;
            string pCtaCbu = string.Empty;
            long pCtaTip = 0;
            if (solicitud.DatosAdministrativos.FormaDePago != null && lstFormaDePago.Where(x => x.ID == solicitud.DatosAdministrativos.FormaDePago.ID).FirstOrDefault().Descripcion == "DEBITO AUTOMATICO")
            {
                pCtaBco = ToStringNotNull(solicitud.DatosAdministrativos.DebitoEnCuenta.Banco.ID.ToString());//??
                pCtaTip = Convert.ToInt64(solicitud.DatosAdministrativos.DebitoEnCuenta.TipoCuenta.ID);
                pCtaNro = ToStringNotNull(solicitud.DatosAdministrativos.DebitoEnCuenta.NroCuenta);
                pCtaRaz = solicitud.DatosAdministrativos.DebitoEnCuenta.NombreTitular;
                pCtaCti = solicitud.DatosAdministrativos.DebitoEnCuenta.NroDNI;
                pCtaCbu = ToStringNotNull(solicitud.DatosAdministrativos.DebitoEnCuenta.CBU);
            }

            //long pFirm = Convert.ToInt32(solicitud.DatosAdministrativos.Firmado);

            string pLisPro = "";
            foreach (var producto in solicitud.Productos)
            {
                pLisPro += producto.ProductoId + ";"
                            + producto.ClienteProductoId + ";"
                            + producto.Cantidad + ";"
                            + producto.Bonificacion + ";"
                            + producto.Final + ";"
                            + producto.Observaciones + "#";
            }
            if (pLisPro.Contains("#"))
                pLisPro = pLisPro.Remove(pLisPro.Length - 1);

            #endregion
            WSVendedores.VendedoresSoapClient wsClient = new WSVendedores.VendedoresSoapClient();
            wsClient.Open();
            //----------------------------------(513,"FAP0010X","ACCESO NORTE","ACCESO NORTE","30-66174060-0",17,"RI", 30, 10101, 10101, 29,"0", "GDOR. UGARTE ",3534, 0, null, "CLI REFE", "CLI  Calle 1", "CLI  Calle 2", -345242372M,-5851169500000003M,1605", 352, "Cli Responsable", "CLI Cargo", "1122334455", "baglio@mail.com",null,null, "GDOR. UGARTE ", 3534, 0, null, "AREA Referencia", "AREA Entre Calle 1", "AREA Entre Calle 2", -345242372M, -5851169500000003M, "1608", 0, 0, 0, 0, 20000101, 0,0,0,0,1,"","",0,"","","",0,"","","","") -------------->,"ALFEREZ HIPÓLITO BOUCHARD ", 4192, 5,"","area REFE","AREA Entre Calle 1", "AREA Entre Calle 2", 564,0,0,0,0,200001,0,0,0,0,1,"AMER","0",12319929, "","","0",0,"","",0,"")-->pId = 23934, vVend=116 -->Vendedor OK
            //DataSet ds = wsClient.SetClientePotencial(pId, pCodCli, pTitRaz, pTitFan, pCuit, pActCli, pSitIva, pNroDoc, pFecIng, pFecVto, pUsr, pNroCto, pTitDm1, pTitDm2, pTitDm3, pTitDm4, pTitRef, pTitEt1, pTitEt2, pTitLat, pTitLon, pTitCP, pTitLoc, pccNom, pccCar, pccTel, pccMail, pccTel2, pccMail2, pFacDm1, pFacDm2, pFacDm3, pFacDm4, pFacRef, pFacEt1, pFacEt2, pFacLat, pFacLon, pFacCP, pFacLoc, pImpMen, pImpAdl, pPrxPag, pFpg, pTarId, pTarNro, pTarVto, pTarTit, pTarDoc, pCtaBco, pCtaTip, pCtaNro, pCtaRaz, pCtaCti, pCtaCbu);
            DataSet ds = wsClient.SetClientePotencial(
                    pId,
                    pCodCli,
                    pTitRaz.ToUpper(),
                    pTitFan.ToUpper(),
                    pCuit,
                    pActCli,
                    pSitIva,
                    pNroDoc,
                    pFecIng,
                    pFecVto,
                    pUsr,
                    pNroCto,
                    pTitDm1.ToUpper(),
                    pTitDm2,
                    pTitDm3,
                    pTitDm4.ToUpper(),
                    pTitRef.ToUpper(),
                    pTitEt1.ToUpper(),
                    pTitEt2.ToUpper(),
                    pTitLat,
                    pTitLon,
                    pTitCP.ToUpper(),
                    pTitLoc,
                    pTitTel,
                    pccNom.ToUpper(),
                    pccCar.ToUpper(),
                    pccTel,
                    pccMail.ToUpper(),
                    pccTel2,
                    pccMail2.ToUpper(),
                    pFacDm1.ToUpper(),
                    pFacDm2,
                    pFacDm3,
                    pFacDm4.ToUpper(),
                    pFacRef.ToUpper(),
                    pFacEt1.ToUpper(),
                    pFacEt2.ToUpper(),
                    pFacLat,
                    pFacLon,
                    pFacCP.ToUpper(),
                    pFacLoc,
                    pImpMen,
                    pImpAdl,
                    pPrxPag,
                    pFpg,
                    pTarId,
                    pTarNro,
                    pTarVto,
                    pTarTit.ToUpper(),
                    pTarDoc,
                    pCtaBco.ToUpper(),
                    pCtaTip,
                    pCtaNro,
                    pCtaRaz.ToUpper(),
                    pCtaCti.ToUpper(),
                    pCtaCbu,
                    pDomLegCob,
                    pPrep,
                    pLisPro);
            wsClient.Close();
            return ds;

        }

        private string ToStringNotNull(string v)
        {
            return string.IsNullOrEmpty(v) || string.IsNullOrWhiteSpace(v) ? "" : v;
        }

        private bool ValidarSolicitud(SolicitudDigitalizada solicitud)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public JsonResult GuardarFirma(string pFirma, string pCli)
        {
            try
            {
                if (!string.IsNullOrEmpty(pFirma))
                {
                    string path = "~/AfiliacionesFirmas/";
                    if (!Directory.Exists(Server.MapPath(path)))
                        Directory.CreateDirectory(Server.MapPath(path));
                    string fileName = "firma-contrato_" + pCli + ".png";
                    string filename = Path.GetFileName(fileName);
                    string pathFinal = Path.Combine(Server.MapPath(path), filename);

                    string pFirmaDraw = pFirma.Replace("data:image/png;base64,", "");
                    var bytes = Convert.FromBase64String(pFirmaDraw);

                    using (var imageFile = new FileStream(pathFinal, FileMode.Create))
                    {
                        imageFile.Write(bytes, 0, bytes.Length);
                        imageFile.Flush();
                    }
                }

                WSVendedores.VendedoresSoapClient wsClient = new WSVendedores.VendedoresSoapClient();
                wsClient.Open();
                DataSet ds = wsClient.SetFirma(Convert.ToInt64(pCli), pFirma, "png", Convert.ToInt64(Session["usr_id"]));
                wsClient.Close();

                return Json(new { Success = true, Message = "Firma aceptada" }, JsonRequestBehavior.AllowGet);
            }
            catch (SoapException ex)
            {
                logger.Error(ex);
                return Json(new { Success = false, Message = "Firma rechazada. Error de Servicio." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return Json(new { Success = false, Message = "Firma rechazada." }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetVendedores()
        {
            try
            {
                WSVendedores.VendedoresSoapClient wsClient = new WSVendedores.VendedoresSoapClient();
                wsClient.Open();
                //DataSet ds = wsClient.GetVendedores();
                DataSet ds = wsClient.GetVendedoresV2(Convert.ToInt64(Session["usr_id"]));
                wsClient.Close();

                Vendedores vendedor = new Vendedores { UsuarioId = 0, Vendedor = "Todos" };

                List<Vendedores> lstVendedores = new List<Vendedores>();
                lstVendedores.Add(vendedor);
                foreach (DataRow dr in ds.Tables[0].Rows)
                    lstVendedores.Add(new Vendedores { UsuarioId = Convert.ToInt64(dr["UsuarioId"]), Vendedor = dr["Vendedor"].ToString() });

                return Json(lstVendedores, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error("Fallo la solicitud de lista de vendedores. " + ex.Message);
                throw;
            }
        }

        public ActionResult GetContrato(int pClienteId)
        {
            try
            {
                WSShamanFecae.WSShamanFECAESoapClient client = new WSShamanFecae.WSShamanFECAESoapClient();
                var result = client.GetContratoVenta(pClienteId.ToString());
                if (result == null)
                    return Json("Error al obtener contrato", JsonRequestBehavior.AllowGet);

                return File(result, MediaTypeNames.Application.Pdf, "contrato_" + pClienteId + ".pdf");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Mock
        //private Cliente CreateMock(DataRow item)
        //{
        //    Cliente nc = new Cliente();

        //    nc.dataRowToObject(item);
        //    int estadoAux = nc.Estado;
        //    nc.Estado = nc.Estado % 2;
        //    if (estadoAux % 3 == 0)
        //        nc.Estado = 2;
        //    return nc;
        //}

        //private DataTable GenerateDataTable<T>(int rows)
        //{
        //    var datatable = new DataTable(typeof(T).Name);
        //    typeof(T).GetProperties().ToList().ForEach(
        //        x => datatable.Columns.Add(x.Name));
        //    Builder<T>.CreateListOfSize(rows).Build()
        //        .ToList().ForEach(
        //            x => datatable.LoadDataRow(x.GetType().GetProperties().Select(
        //                y => y.GetValue(x, null)).ToArray(), true));
        //    return datatable;
        //}
        #endregion

    }

    public class Vendedores
    {
        public long UsuarioId { get; set; }
        public string Vendedor { get; set; }
    }
}
