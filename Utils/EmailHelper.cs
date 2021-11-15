using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;

namespace LMS_Api.Utils
{
    public static class EmailHelper
    {
        public static bool SendMailToOneUser(string recipient, string subject, string token = "", string url = "")
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com");

            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            System.Net.NetworkCredential credentials =
                new System.Net.NetworkCredential("mm.aleskerov93@gmail.com", "niyazi0502106331");
            client.EnableSsl = true;
            client.Credentials = credentials;

            try
            {
                var mail = new MailMessage("mm.aleskerov93@gmail.com", recipient.Trim());
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                string body = File.ReadAllText("/Users/niyazibabayev/Desktop/LMS_FrontEnd/templates/mail.html");
                body = body.Replace("#MailTopic#", subject);
                body = body.Replace("#Action#", subject);
                body = body.Replace("#Link#", url);
                body = body.Replace("#Token#", token);
                mail.Body = body;
                client.Send(mail);

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public static bool SendMailToManyUsers(List<string> recipients, string subject, string token = "", string url = "")
        {
            var smtp = new System.Net.Mail.SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("mm.aleskerov93@gmail.com", "niyazi0502106331")
            };

            try
            {
                var mail = new MailMessage();

                foreach (var recipient in recipients)
                {
                    mail.To.Add(recipient);
                }

                mail.From = new MailAddress("mm.aleskerov93@gmail.com", "LMS");
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                string body = File.ReadAllText("/Users/niyazibabayev/Desktop/LMS_FrontEnd/templates/mail.html");
                body = body.Replace("#MailTopic#", subject);
                body = body.Replace("#Action#", subject);
                body = body.Replace("#Link#", url);
                body = body.Replace("#Token#", token);
                mail.Body = body;


                smtp.Send(mail);

                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool SendConfirmationEmail(string confirmationToken, string email, string userId)
        {
            var encodedToken = Encoding.UTF8.GetBytes(confirmationToken);
            var validEmailToken = WebEncoders.Base64UrlEncode(encodedToken);
            string url = $"http://localhost:3000/ConfirmEmail/{userId}/{validEmailToken}";
            var succeeded = SendMailToOneUser(email, "Confirm your email", "", url);

            if (succeeded)
                return true;

            return false;
        }
    }
}
