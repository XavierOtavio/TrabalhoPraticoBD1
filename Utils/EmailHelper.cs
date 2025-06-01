using System;
using System.Configuration;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;

namespace TrabalhoFinal3.Utils
{
    public static class EmailHelper
    {
        /// <summary>Envia um e-mail HTML simples.</summary>
        public static void Send(string to, string subject, string body)
        {
            // 1) Lê definições do Web.config  <system.net> → <mailSettings>
            var section = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp") ?? throw new InvalidOperationException("SMTP configuration missing.");
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            var smtp = new SmtpClient
            {
                Host = section.Network.Host,
                Port = section.Network.Port,
                EnableSsl = section.Network.EnableSsl,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(
                                   section.Network.UserName,
                                   section.Network.Password)
            };

            // 2) Endereço "From"
            var msg = new MailMessage(section.From, to, subject, body)
            {
                IsBodyHtml = true
            };

            smtp.Send(msg);
        }
    }
}
