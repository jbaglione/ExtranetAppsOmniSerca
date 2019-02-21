using NLog;
using ExtranetAppsOmniSerca.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Services.Protocols;
using ExtranetAppsOmniSerca.WSClientesDocumentos;

namespace ExtranetAppsOmniSerca.Helpers
{
	public class ServiceHelper
	{
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static string UltimaRespuesta { get; set; }

        public static HashSet<Notificacion> getNotificaciones(long usr_id)
		{
			var dt = getNotificacionesFromWebService(usr_id);
			return getNotificacionesFormatted(dt);

		}

        public static HashSet<Notificacion> getNotificacionesFormatted(DataTable dt)
		{
			var result = new HashSet<Notificacion>();
			if(dt!=null)
			foreach (DataRow item in dt.Rows)
			{
				result.Add(new Notificacion(item,false));
			}

			return result;
		}

        public HashSet<Usuario> getUsuarios(long grupo_id)
		{
			var dt = getUsuariosFromWebService(grupo_id);
			return getUsuariosFormated(dt);
		}

        public HashSet<Usuario> getUsuariosFormated(DataTable dt)
		{
			var result = new HashSet<Usuario>();
			foreach (DataRow item in dt.Rows)
			{
				result.Add(new Usuario(item));
			}

			return result;
		}

        public static DataTable getUsuariosFromWebService(long grupo_id)
		{
            var wsClient = GetNotificacionesWS();
			try
			{
				wsClient.Open();
				DataSet dsResult = wsClient.GetUsuarios(grupo_id,0);

				wsClient.Abort();
				return dsResult.Tables[0];
			}
            catch (SoapException ex)
            {
                logger.Error(ex);
            }
            catch (FaultException ex)
            {
                logger.Error(ex);
                logger.Error(UltimaRespuesta);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return null;

        }

        public static DataTable sendNotificacionesToWebService(Notificacion valor, long usr_id)
		{

			var wsClient = GetNotificacionesWS();
			try
			{
				wsClient.Open();
				DataSet dsResult = wsClient.SetMensaje(valor.Id, valor.Mensaje, usr_id, valor.Mail ? 1 : 0);
				foreach (var item in valor.Usuarios)
				{
					dsResult = wsClient.SetMensajeUsuarios(valor.Id, item.ToString());
				}

				wsClient.Abort();
				return dsResult.Tables[0];
			}
            catch (SoapException ex)
            {
                logger.Error(ex);
            }
            catch (FaultException ex)
            {
                logger.Error(ex);
                logger.Error(UltimaRespuesta);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return null;

        }

        public static DataTable getNotificacionesFromWebService(long usr_id)
		{

			var wsClient = GetNotificacionesWS();
			try
			{
				wsClient.Open();
				DataSet dsResult = wsClient.GetMensajesRecibidos(usr_id);
				wsClient.Abort();
				return dsResult.Tables[0];
			}
            catch (SoapException ex)
            {
                logger.Error(ex);
            }
            catch (FaultException ex)
            {
                logger.Error(ex);
                logger.Error(UltimaRespuesta);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return null;

        }

        public static int getNotificacionesCount(long usr_id)
		{
			var dt = getNotificacionesFromWebService(usr_id);
			var notificaciones = getNotificacionesFormatted(dt);
			return notificaciones.Where(i => i.EstadoId == (int)Notificacion.Estados.Pendiente).Count();

		}

        public static HashSet<Comprobante> getComprobantes(long usr_id)
		{
			var dt = getComprobantesFromWebService(usr_id);
			return getComprobantesFormatted(dt);

		}

        public static HashSet<Comprobante> getComprobantesFormatted(DataTable dt)
		{
			var result = new HashSet<Comprobante>();
			if (dt != null)
				foreach (DataRow item in dt.Rows)
				{
					result.Add(new Comprobante(item));
				}

			return result;
		}

        public static DataTable getComprobantesFromWebService(long usr_id)
		{
            var wsClient = GetClientesDocumentosWS();

            try
			{
				wsClient.Open();
				DataSet dsResult = wsClient.GetComprobantes(usr_id);
				wsClient.Abort();
				return dsResult.Tables[0];
			}
            catch (SoapException ex)
            {
                logger.Error(ex);
            }
            catch (FaultException ex)
            {
                logger.Error(ex);
                logger.Error(UltimaRespuesta);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return null;
        }

        public static HashSet<ComprobanteServicioRenglon> getComprobanteServicioRenglon(long usr_id, long doc_id, long servicio)
		{
			var dt = getComprobanteServicioRenglonFromWebService(usr_id, doc_id, servicio);
			return getComprobanteServicioRenglonFormatted(dt);

		}

        public static HashSet<ComprobanteServicioRenglon> getComprobanteServicioRenglonFormatted(DataTable dt)
		{
			var result = new HashSet<ComprobanteServicioRenglon>();
			if (dt != null)
				foreach (DataRow item in dt.Rows)
				{
					result.Add(new ComprobanteServicioRenglon(item));
				}

			return result;
		}

        public static DataTable getComprobanteServicioRenglonFromWebService(long usr_id, long doc_id, long servicio)
		{
			var wsClient = GetClientesDocumentosWS();
			try
			{
				wsClient.Open();
				DataSet dsResult = wsClient.GetServicioRenglon(usr_id, doc_id, servicio );
				wsClient.Abort();
				return dsResult.Tables[0];
			}
            catch (SoapException ex)
            {
                logger.Error(ex);
            }
            catch (FaultException ex)
            {
                logger.Error(ex);
                logger.Error(UltimaRespuesta);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return null;
        }

        public static HashSet<ComprobanteServicio> getComprobanteServicios(long usr_id, long doc_id)
		{
			var dt = getComprobanteServiciosFromWebService(usr_id, doc_id);
			return getComprobanteServiciosFormatted(dt);

		}

        public static HashSet<ComprobanteServicio> getComprobanteServiciosFormatted(DataTable dt)
		{
			var result = new HashSet<ComprobanteServicio>();
			if (dt != null)
				foreach (DataRow item in dt.Rows)
				{
					result.Add(new ComprobanteServicio(item));
				}

			return result;
		}

        public static DataTable getComprobanteServiciosFromWebService(long usr_id, long doc_id)
		{
			var wsClient = GetClientesDocumentosWS();
			try
			{
				wsClient.Open();
				DataSet dsResult = wsClient.GetServiciosComprobantes(usr_id, doc_id);
				wsClient.Abort();
				return dsResult.Tables[0];
			}
            catch (SoapException ex)
            {
                logger.Error(ex);
            }
            catch (FaultException ex)
            {
                logger.Error(ex);
                logger.Error(UltimaRespuesta);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return null;
		}

        public static bool SetComprobante(Factura value, long usr_id,out string error)
        {
            var wsClient = GetProveedoresCuentasWS();
            error = "";
            try
            {
                wsClient.Open();
                DataSet dsResult = wsClient.SetComprobante(value.ID, value.CAE, value.Fecha.FormatedDateToAnsi(), value.Tipo, value.Numero, value.Monto, usr_id);
                wsClient.Abort();
                if (!string.IsNullOrEmpty(dsResult.Tables[0].Rows[0][1] as string)){
                    error = dsResult.Tables[0].Rows[0][1] as string;
                    return false;
                }
                return true;
            }
            catch (SoapException ex)
            {
                error = ex.Message;
                logger.Error(ex);
            }
            catch (FaultException ex)
            {
                error = ex.Message;
                logger.Error(ex);
                logger.Error(UltimaRespuesta);
            }
            catch (Exception ex)
            {
            error = ex.Message;
                logger.Error(ex);
            }
                return false;
        }


        public static bool SetComprobanteV2(Factura value, long usr_id, out string error)
        {
            WSProveedoresCuentas.ProveedoresCuentasSoapClient wsClient = GetProveedoresCuentasWS();
            error = "";
            try
            {
                wsClient.Open();
                DataSet dsResult = wsClient.SetComprobanteV2(value.NroOP, value.Tipo, value.Numero, value.Fecha.FormatedDateToAnsi(), value.CAE, value.Monto, usr_id);
                wsClient.Close();
                if (!string.IsNullOrEmpty(dsResult.Tables[0].Rows[0][1] as string))
                {
                    error = dsResult.Tables[0].Rows[0][1] as string;
                    return false;
                }
                return true;
            }
            catch (SoapException ex)
            {
                error = ex.Message;
                logger.Error(ex);
            }
            catch (FaultException ex)
            {
                error = ex.Message;
                logger.Error(ex);
                logger.Error(UltimaRespuesta);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                logger.Error(ex);
            }
            return false;
        }


        public static HashSet<CuentaCorriente> getCuentaCorriente(long usr_id)
        {
            var dt = getCuentaCorrienteFromWebService(usr_id);
            return getCuentaCorrienteFormatted(dt);
        }

        public static HashSet<Proveedor> getProveedores()
        {
            var dt = getProveedoresFromWebService();
            return getProveedoresFormatted(dt);
        }

        public static HashSet<CuentaCorriente> getCuentaCorrienteFormatted(DataTable dt)
        {
            var result = new HashSet<CuentaCorriente>();
            
            
            if (dt != null)
            {
                int i = 0;
                while (i < dt.Rows.Count)
                {
                    CuentaCorriente obj;
                    obj = new CuentaCorriente(i, dt.Rows[i], dt.Select("NroOrdenPago='" + dt.Rows[i]["NroOrdenPago"].ToString() + "'").Length > 1);
                    if (dt.Select("NroOrdenPago='" + dt.Rows[i]["NroOrdenPago"].ToString() + "'").Length > 1)
                    {
                        obj.NroComprobante += "...";
                        foreach (DataRow dr in dt.Select("NroOrdenPago='" + dt.Rows[i]["NroOrdenPago"].ToString() + "'"))
                            obj.CuentaCorrientes.Add(new CuentaCorriente(i, dr, false));
                        i += dt.Select("NroOrdenPago='" + dt.Rows[i]["NroOrdenPago"].ToString() + "'").Length;
                    }
                    else
                        i++;
                    result.Add(obj);
                }

            }
            return result;
        }

        public static DataTable getCuentaCorrienteFromWebService(long usr_id)
        {
            //var wsClient = GetProveedoresCuentasWS();
            //wsClient.Endpoint.Behaviors.Add(new MyFaultLogger());
            try
            {
                WSProveedoresCuentas.ProveedoresCuentasSoapClient wsClient = new WSProveedoresCuentas.ProveedoresCuentasSoapClient();
                wsClient.Open();
                DataSet ds = wsClient.GetCuentaCorrienteV2(usr_id);
                //DataSet dsResult = wsClient.GetCuentaCorrienteV2(usr_id); //.GetCuentaCorriente(usr_id);
                wsClient.Close();
                return ds.Tables[0];
            }
            catch (SoapException ex)
            {
                logger.Error(ex);
            }
            catch (FaultException ex)
            {
                logger.Error(ex);
                logger.Error(UltimaRespuesta);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return null;
        }

        public static DataTable getProveedoresFromWebService()
        {
            try
            {
                WSProveedoresCuentas.ProveedoresCuentasSoapClient wsClient = new WSProveedoresCuentas.ProveedoresCuentasSoapClient();
                wsClient.Open();
                DataSet ds = wsClient.GetProveedores();
                wsClient.Close();
                return ds.Tables[0];
            }
            catch (SoapException ex)
            {
                logger.Error(ex);
            }
            catch (FaultException ex)
            {
                logger.Error(ex);
                logger.Error(UltimaRespuesta);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return null;
        }

        public static HashSet<Proveedor> getProveedoresFormatted(DataTable dt)
        {
            var result = new HashSet<Proveedor>();
            if (dt != null)
                foreach (DataRow item in dt.Rows)
                {
                    result.Add(new Proveedor(item));
                }

            return result;
        }


        public static WSProveedoresCuentas.ProveedoresCuentasSoapClient GetProveedoresCuentasWS()
        {
            var ws = new WSProveedoresCuentas.ProveedoresCuentasSoapClient();
            ws.Endpoint.Behaviors.Add(new MyFaultLogger());
            return ws;
        }

        public static WSNotificaciones.NotificacionesSoapClient GetNotificacionesWS()
        {
            var ws = new WSNotificaciones.NotificacionesSoapClient();
            ws.Endpoint.Behaviors.Add(new MyFaultLogger());
            return ws;
        }

        public static ClientesDocumentosSoapClient GetClientesDocumentosWS()
        {
            var ws = new ClientesDocumentosSoapClient();
            ws.Endpoint.Behaviors.Add(new MyFaultLogger());
            return ws;
        }

    }
}