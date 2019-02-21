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
using NLog;
using System.Collections.Specialized;
using ExtranetAppsOmniSerca.Helpers;
using Newtonsoft.Json;
using System.Net.Http;

namespace ExtranetAppsOmniSerca.Controllers
{
    public class LoginController : Controller
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

        public ActionResult Index()
        {
            if (Request.QueryString["logout"] == "true")
            {
                Session["usr_id"] = 0;
                return RedirectToAction("Index", "Login");
            }

            int usr = Convert.ToInt32(Session["usr_id"]);

            if (usr == 0)
                Session.RemoveAll();
            ViewBag.Title = "Login";
            return View();
        }

        public string GetSession()
        {
            try
            {
                return Session["UsuarioExtranetId"].ToString();
            }
            catch (Exception)
            {
                return "0";
            }
        }

        public async System.Threading.Tasks.Task<JsonResult> Login(string pUsr, string pPass)
        {
            try
            {
                WSWebApps.WebAppsSoapClient wsClient = new WSWebApps.WebAppsSoapClient();
                wsClient.Open();
                DataSet ds = wsClient.Login(pUsr, pPass);
                wsClient.Close();

                LoginSession loginSession = new LoginSession();
                loginSession.LoginUsuarios = new List<LoginUsuario>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                    loginSession.LoginUsuarios.Add(
                        new LoginUsuario
                        {
                            Jerarquia = dr["Jerarquia"].ToString(),
                            Titulo = dr["Titulo"].ToString(),
#if DEBUG
                            URL = (dr["URL"].ToString().Contains("token") ? dr["URL"].ToString().Replace("?token", "").Replace("http://remote.omnisaludsa.com.ar:5567", "http://localhost:4200") + await GetToken(pUsr, pPass, dr["Jerarquia"].ToString()) : dr["URL"].ToString()),
#else
                            URL = (dr["URL"].ToString().Contains("token") ? dr["URL"].ToString().Replace("?token", "") + await GetToken(pUsr, pPass, dr["Jerarquia"].ToString()) : dr["URL"].ToString()),
#endif
                            UsuarioNombre = dr["UsuarioNombre"].ToString()
                        });

                if (loginSession.LoginUsuarios.Count > 0)
                    loginSession.MySessionData = GetSessionData(pUsr);                  

                return Json(loginSession, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error("Fallo el Login. " + ex.Message);
                throw;
            }
        }

        private async System.Threading.Tasks.Task<string> GetToken(string pUsr, string pPass, string jerarquia)
        {
            string token = "";
            try
            {
                NameValueCollection appSettings = System.Web.Configuration.WebConfigurationManager.AppSettings;
                using (var httpClient = new HttpClient())
                {
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                    httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");
                    httpClient.DefaultRequestHeaders.Add("Identificacion", pUsr);
                    httpClient.DefaultRequestHeaders.Add("Password", pPass);
                    httpClient.DefaultRequestHeaders.Add("Jerarquia", jerarquia);

                    //var a = httpClient.PostAsync(new Uri(appSettings.Get("URLSecurity") + "authenticateBase64"), new StringContent("application/json"));
                    var result = await httpClient.PostAsync(new Uri(appSettings.Get("URLSecurity") + "authenticateByUserPass"), new StringContent("application/json"));
                    var user = JsonConvert.DeserializeObject<User>(await result.Content.ReadAsStringAsync());
                    token = user.Token;

                    //var response = httpClient.PostAsJsonAsync(new Uri(appSettings.Get("URLSecurity") + "authenticateBase64"), new StringContent("application/json")).Result;

                    //OkObjectResult r = httpClient.PostAsJsonAsync(new Uri(Configuration["URLSecurity"] + "authenticateBase64"), new StringContent("application/json")).Result;
                }

            }
            catch (Exception ex)
            {

            }
            return token;

        }

        private SessionData GetSessionData(string pIde)
        {
            try
            {
                WSWebApps.WebAppsSoapClient wsClient = new WSWebApps.WebAppsSoapClient();
                wsClient.Open();
                DataSet ds = wsClient.GetSessionData(pIde);
                wsClient.Close();

                var session = new SessionData(ds.Tables[0].Rows[0]);
                Session["UsuarioExtranetId"] = session.UsuarioExtranetId;
                //new session.
                Session["usr_id"] = session.UsuarioExtranetId;
                Session["UsuarioShamanId"] = session.UsuarioShamanId;
                Session["Identificacion"] = session.Identificacion;
                Session["Nombre"] = session.Nombre;
                Session["Email"] = session.Email;
                Session.Timeout = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings.Get("SessionTimeOut"));
                return session;
            }
            catch (Exception ex)
            {
                logger.Error("Fallo el GetSessionData. " + ex.Message);
                throw;
            }
        }

        public JsonResult ChangePassword(long pUsrExtId, string pOld, string pNew)
        {
            try
            {
                if (pUsrExtId != Convert.ToInt64(Session["UsuarioExtranetId"]))
                    return Json("Session invalida", JsonRequestBehavior.AllowGet);

                WSWebApps.WebAppsSoapClient wsClient = new WSWebApps.WebAppsSoapClient();
                wsClient.Open();
                DataSet ds = wsClient.ChangePassword(pUsrExtId, pOld, pNew);
                wsClient.Close();
                ChangePasswordOrEmail changePassword = new ChangePasswordOrEmail(ds.Tables[0].Rows[0]);
                //var json = JsonConvert.SerializeObject(new { status = (ds.Tables[0].Rows[0]["Resultado"].ToString() == "1"? "OK":"Error"), message = ds.Tables[0].Rows[0]["DescripcionError"].ToString()});
                return Json(changePassword, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error("Fallo ChangePassword. " + ex.Message);
                throw;
            }
        }

        public JsonResult ChangeSettings(long pUsrExtId, string pEmail)
        {
            try
            {
                if (pUsrExtId != Convert.ToInt64(Session["UsuarioExtranetId"]))
                    return Json("Session invalida", JsonRequestBehavior.AllowGet);

                WSWebApps.WebAppsSoapClient wsClient = new WSWebApps.WebAppsSoapClient();
                wsClient.Open();
                DataSet ds = wsClient.SetPersonals(pUsrExtId, pEmail);
                wsClient.Close();
                ChangePasswordOrEmail changePassword = new ChangePasswordOrEmail(ds.Tables[0].Rows[0]);
                //var json = JsonConvert.SerializeObject(new { status = (ds.Tables[0].Rows[0]["Resultado"].ToString() == "1"? "OK":"Error"), message = ds.Tables[0].Rows[0]["DescripcionError"].ToString()});
                return Json(changePassword, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error("Fallo ChangeSettings. " + ex.Message);
                throw;
            }
        }

        public JsonResult GetAlertas()
        {
            try
            {
                WSWebApps.WebAppsSoapClient wsClient = new WSWebApps.WebAppsSoapClient();
                wsClient.Open();
                DataSet ds = wsClient.GetAlertas(Convert.ToInt64(Session["UsuarioExtranetId"]));
                wsClient.Close();

                List<Alerta> lstAlertas = new List<Alerta>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                    lstAlertas.Add(new Alerta(dr));

                return Json(lstAlertas, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error("Fallo el Login. " + ex.Message);
                throw;
            }
        }

        public JsonResult ForgotPassword(string pIde)
        {
            string mensaje = "No se pudo recuperar la clave";
            try
            {
                WSWebApps.WebAppsSoapClient wsClient = new WSWebApps.WebAppsSoapClient();
                wsClient.Open();
                DataSet ds = wsClient.ForgotPassword(pIde);
                wsClient.Close();

                if (ds.Tables != null &&
                ds.Tables.Count > 0 &&
                ds.Tables[0].Rows != null)
                {
                    if (SendMailPassword(ds.Tables[0].Rows[0]))
                        mensaje = "Ok";
                    else
                        mensaje = "Fallo el envio de Email";
                }
                else
                    mensaje = "No se pudo recuperar la clave";

                return Json(mensaje, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(mensaje + ex.Message);
                throw;
            }
        }

        public bool SendMailPassword(DataRow dr)
        {
            try
            {
                string email = dr["Email"].ToString();
                string identificacion = dr["Identificacion"].ToString();
                if (email.Length > 0 & email.Contains("@"))
                {
                    string vTitle = "Reposición de contraseña";

                    string vBody = "Estimado " + dr["Nombre"].ToString() + "<br />";

                    vBody = vBody + "<br />";
                    vBody = vBody + "En base a su solicitud, a través del presente reestablecemos su contraseña " + "<br />";
                    vBody = vBody + "<br />";
                    vBody = vBody + "Su usuario de ingreso es: " + "<br />";
                    vBody = vBody + identificacion + "<br />";
                    vBody = vBody + "Su contraseña: " + "<br />";
                    vBody = vBody + dr["Password"].ToString() + "<br />";
                    vBody = vBody + "<br />";
                    vBody = vBody + "También puede ingresar con su e-mail " + email.ToLower() + " en vez de la identificación brindada (" + identificacion + ")" + "<br />";
                    vBody = vBody + "<br />";
                    vBody = vBody + "Quedamos a disposición de cualquier inquietud o sugerencia que tenga sobre nuestro sitio web" + "<br />";
                    vBody = vBody + "<br />";
                    vBody = vBody + "<br />";
                    vBody = vBody + "Atte. Grupo Serca";
                    vBody = vBody + "<br />";
                    vBody = vBody + "<br />";

                    return EmailHelpers.Send(email, vTitle, vBody, null);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Fallo al enviar el email." + ex.Message);
            }
            return false;
        }

        public JsonResult SendMailRegisterUser(string nombre, string relacion, string emailCliente, string labelCliente, string cliente)
        {
            string mensaje = "No se pudo enviar la solicitud";
            try
            {
                NameValueCollection appSettings = System.Web.Configuration.WebConfigurationManager.AppSettings;
                string email = appSettings.Get("MailAddress");

                if (email.Length > 0 & email.Contains("@") || emailCliente.Length > 0 & emailCliente.Contains("@"))
                {
                    string vTitle = "Solicitud de nuevo usuario Extranet";

                    string vBody = "Mediante el presente correo se solicita la creacion de un usuario Extranet referente a la siguiente informacion:";
                    vBody = vBody + "<br />";
                    vBody = vBody + "<br />";
                    vBody = vBody + "Nombre: " + nombre + "<br />";
                    vBody = vBody + "Relacion: " + relacion + "<br />";
                    vBody = vBody + "Email del nuevo usuario: " + emailCliente + "<br />";
                    vBody = vBody + labelCliente + ": " + cliente + "<br />";
                    vBody = vBody + "<br />";
                    vBody = vBody + "<br />";
                    vBody = vBody + "Atte. Grupo Serca";
                    vBody = vBody + "<br />";

                    if (EmailHelpers.Send(email, vTitle, vBody, null))
                        mensaje = "Ok";
                }
                return Json(mensaje, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error("Fallo al enviar el email." + ex.Message);
                throw;
            }
        }

    }
    //TODO: Eliminar, o establecer una clase usuario real para todos los proyectos igual.
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
    }
}
