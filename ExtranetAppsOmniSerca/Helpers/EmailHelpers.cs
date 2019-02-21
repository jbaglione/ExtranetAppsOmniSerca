using System;
using System.Net;
using System.Net.Mail;
using System.Collections.Specialized;
using NLog;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace ExtranetAppsOmniSerca.Helpers
{
    public class EmailHelpers
    {
        public static bool Send(string To, string Subject, string Body, List<String> PathFiles, Attachment atachment = null)
        {
            Logger log = LogManager.GetCurrentClassLogger();

            if (!string.IsNullOrEmpty(To) && new MailAddress(To).Address == To)
            {
                //log.Info("Preparando para el envio a: " + To);

                if (ConfigurationManager.AppSettings.Count > 0)
                {
                    //Preparo el cliente SMTP
                    SmtpClient smtpSerca = new SmtpClient();

                    smtpSerca.Host = ConfigurationManager.AppSettings["MailServer"];
                    smtpSerca.Port = int.Parse(ConfigurationManager.AppSettings["MailPort"]);
                    smtpSerca.EnableSsl = bool.Parse(ConfigurationManager.AppSettings["MailSSL"]);
                    smtpSerca.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpSerca.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["MailAddress"], ConfigurationManager.AppSettings["MailPassword"]);

                    //Preparo el EMAIL
                    string FromAdrress = ConfigurationManager.AppSettings["MailAddress"];
                    string FromName = ConfigurationManager.AppSettings["MailFrom"];
                    MailMessage eMail = new MailMessage();
                    eMail.To.Add(new MailAddress(To));
                    eMail.From = new MailAddress(FromAdrress, FromName, Encoding.UTF8);
                    eMail.Subject = Subject;
                    eMail.SubjectEncoding = Encoding.UTF8;
                    eMail.Body = Body;
                    eMail.BodyEncoding = Encoding.UTF8;
                    eMail.IsBodyHtml = true;
                    eMail.Priority = MailPriority.High;

                    //Adjunto los archivos que son de los comprobantes
                    if (PathFiles != null)
                    {
                        foreach (string item in PathFiles)
                            eMail.Attachments.Add(new Attachment(item));
                    }
                    else if (atachment != null)
                        eMail.Attachments.Add(atachment);


                    //Envio de Email
                    try
                    {
                        log.Info("Enviando email");
                        smtpSerca.Send(eMail);
                        log.Info("Envio OK");
                        return true;
                    }
                    catch (Exception ex)
                    {
                        string mensaje = "Error enviando email de " + To + " - REF: " + ex.Message;
                        log.Error(mensaje);
                    }
                }
            }
            return false;
        }
    }
}