using System;
namespace Business.Abstract
{
    public interface IEmailService
    {
        void SendMailToOneUser(string recipient, string subject, string url = "", string token = "");
    }
}
