using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace WebApplication1.Models
{
       public class Mailer
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        //Constructor
        public Mailer(string from, string to, string subject, string body)
        {
            From = from; To = to; Subject = subject; Body = body;
        }

         public bool Send()
        {
            try
            {
                var msg = Body;
                MailMessage mail = new MailMessage();
                mail.To.Add(To);
                mail.From = new MailAddress(From, "SpeedNet");
                mail.Subject = Subject;
                mail.Body = string.Format(msg, mail);
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential
                ("noreply.easymarketing@gmail.com", "noreply369*123");
                smtp.EnableSsl = true;
                smtp.Send(mail);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}