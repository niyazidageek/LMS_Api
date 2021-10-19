using System;
using System.Net.Http;
using System.Net.Mail;
using Business.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.Web;
using System.IO;
using System.Reflection;

namespace Business.Concrete
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;


        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendMailToOneUser(string recipient, string subject, string token="",string url="")
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com");

            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            System.Net.NetworkCredential credentials =
                new System.Net.NetworkCredential(_configuration["SMTPConfig:Email"], _configuration["SMTPConfig:Password"]);
            client.EnableSsl = true;
            client.Credentials = credentials;

            try
            {
                var mail = new MailMessage(_configuration["SMTPConfig:Email"].Trim(), recipient.Trim());
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                string body = File.ReadAllText("/Users/niyazibabayev/Desktop/LMS_FrontEnd/templates/mail.html");
                body = body.Replace("#MailTopic#", subject);
                body = body.Replace("#Action#", subject);
                body = body.Replace("#Link#", url);
                body = body.Replace("#Token#", token);
                mail.Body = body;
                client.Send(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
