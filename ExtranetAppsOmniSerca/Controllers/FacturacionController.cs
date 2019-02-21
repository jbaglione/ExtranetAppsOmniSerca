using Ionic.Zip;
using NPOI.HSSF.UserModel;
using ExtranetAppsOmniSerca.Helpers;
using ExtranetAppsOmniSerca.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExtranetAppsOmniSerca.Controllers
{
    public class FacturacionController : Controller
    {
        //
        // GET: /Facturacion/

		public ActionResult Index()
        {
			ViewBag.Title = "Consulta de comprobandes";

            int usr = Convert.ToInt32(Session["usr_id"]);
            if (usr != 0 && validarUsuario(usr))
                return View();
            else
            {
                //Session.RemoveAll();
                return RedirectToAction("Index", "Error");
            }
        }

		public ActionResult GetComprobantes()
		{
			var result = ServiceHelper.getComprobantes(Convert.ToInt32(Session["usr_id"]));
			return Json(result, JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetServiciosImagenes()
		{
			var result = (HashSet<Models.ComprobanteServicio>)Session["ordenes"];
			var files = result.SelectMany(i=> i.OrdenesFiles.Split(';')).Select(i => string.Format("\\\\" + Helpers.Config.ServerFileSystem + "\\ordenes$\\{0}", i));

			Response.Clear();
			Response.BufferOutput = false;
			System.Web.HttpContext c = System.Web.HttpContext.Current;
			string archiveName = String.Format("imagenes-{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
			Response.ContentType = "application/zip";
			Response.AddHeader("content-disposition", "filename=" + archiveName);

			using (ZipFile zip = new ZipFile())
			{
				zip.AddFiles(files);
				zip.Save(Response.OutputStream);
			}
			Response.Close();
			return null;
		}

		public ActionResult GetServiciosXls()
		{
			var result = (HashSet<Models.ComprobanteServicio>)Session["ordenes"];
			var workbook = new HSSFWorkbook();
			var sheet = workbook.CreateSheet("Servicios");
			var rowIndex = 0;
			var row = sheet.CreateRow(rowIndex);
			row.CreateCell(0).SetCellValue("NroIncidente");
			row.CreateCell(1).SetCellValue("Concepto");
			row.CreateCell(2).SetCellValue("NroInterno");
			row.CreateCell(3).SetCellValue("Iva");
			row.CreateCell(4).SetCellValue("Arba");
			row.CreateCell(5).SetCellValue("Agip");
			row.CreateCell(6).SetCellValue("NroAfiliado");
			row.CreateCell(7).SetCellValue("Paciente");
			row.CreateCell(8).SetCellValue("Desde");
			row.CreateCell(9).SetCellValue("Hasta");
			row.CreateCell(10).SetCellValue("Kilometros");
			row.CreateCell(11).SetCellValue("Espera");
			row.CreateCell(12).SetCellValue("Importe Base");
			row.CreateCell(13).SetCellValue("Recargo");
			row.CreateCell(14).SetCellValue("Importe");
			foreach (var item in result)
			{
				rowIndex++;
				row = sheet.CreateRow(rowIndex);
				row.CreateCell(0).SetCellValue(item.NroIncidente);
				row.CreateCell(1).SetCellValue(item.ConceptoId);
				row.CreateCell(2).SetCellValue(item.NroInterno);
				row.CreateCell(3).SetCellValue(item.Iva);
				row.CreateCell(4).SetCellValue(item.Arba);
				row.CreateCell(5).SetCellValue(item.Agip);
				row.CreateCell(6).SetCellValue(item.NroAfiliado);
				row.CreateCell(7).SetCellValue(item.Paciente);
				row.CreateCell(8).SetCellValue(item.Desde);
				row.CreateCell(9).SetCellValue(item.Hasta);
				row.CreateCell(10).SetCellValue(item.Kmt);
				row.CreateCell(11).SetCellValue(item.TpoEspera);
				row.CreateCell(12).SetCellValue((double)item.ImporteBase);
				row.CreateCell(13).SetCellValue((double)item.Recargos);
				row.CreateCell(14).SetCellValue((double)item.Importe);
			}

			using (var exportData = new MemoryStream())
			{
				workbook.Write(exportData);
				string saveAsFileName = string.Format("Servicios-{0:d}.xls", DateTime.Now).Replace("/", "-");
				return File(exportData.ToArray(), "application/vnd.ms-excel", saveAsFileName);
			}
		}

		public ActionResult GetServicios(int comprobante)
		{
			var result = ServiceHelper.getComprobanteServicios(Convert.ToInt32(Session["usr_id"]), comprobante);
			Session["ordenes"] = result;
			return Json(result, JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetRenglones(string comprobante, string servicio)
		{
			var result = ServiceHelper.getComprobanteServicioRenglon(Convert.ToInt32(Session["usr_id"]), Convert.ToInt32(comprobante),Convert.ToInt32(servicio));
			if (!result.Any())
				result.Add(new ComprobanteServicioRenglon());
			return Json(result, JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetUrlImagenes(string servicioId)
		{
			var result = new List<string>();
			var lista = (HashSet<Models.ComprobanteServicio>)Session["ordenes"];
			var pservicioid = long.Parse(servicioId);
			var servicio = lista.Where(i => i.ID == pservicioid).FirstOrDefault();

			result.AddRange(servicio.OrdenesFiles.Split(';').Select(i => string.Format("/imagenes/{0}", i.Replace('\\', '/'))));
			return Json(result, JsonRequestBehavior.AllowGet);

		}

		public ActionResult GetImagenes(string servicioId)
		{
			var lista = (HashSet<Models.ComprobanteServicio>)Session["ordenes"];
			var pservicioid = long.Parse(servicioId);
			var result = lista.Where(i => i.ID == pservicioid).FirstOrDefault();
			var files = result.OrdenesFiles.Split(';').Select(i => string.Format("\\\\" + Helpers.Config.ServerFileSystem + "\\ordenes$\\{0}", i));

			Response.Clear();
			Response.BufferOutput = false; 
			System.Web.HttpContext c = System.Web.HttpContext.Current;
			string archiveName = String.Format("imagenes-{0}.zip",DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
			Response.ContentType = "application/zip";
			Response.AddHeader("content-disposition", "filename=" + archiveName);

			using (ZipFile zip = new ZipFile())
			{
				zip.AddFiles(files);
				zip.Save(Response.OutputStream);
			}
			Response.Close();
			return null;
		}

		public ActionResult GetPdf(string docid)
		{
			var client = new WSShamanFecae.WSShamanFECAESoapClient();
			var pdocid = long.Parse(docid);
			var result = client.GetPDF_Cache(pdocid, Convert.ToInt32(Session["usr_id"]));
			return File(result, System.Net.Mime.MediaTypeNames.Application.Pdf, "comprobante.pdf");
		}

		public bool validarUsuario(long usr)
		{
			try
			{
				var wsClient = ServiceHelper.GetClientesDocumentosWS();
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
					Session["Acceso"] = Convert.ToInt32(dtUsuario.Rows[0]["Acceso"]);
					return true;
				}
			}
			catch
			{

				return false;

			}

		}
	}
}
