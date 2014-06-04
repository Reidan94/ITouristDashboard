using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace ITourist.Models.LogicModels.Services
{
    public class ConfirmationMailSender : ISender
    {
        private readonly string siteMail;
        private readonly string passWord;
        private readonly string host;

        public ConfirmationMailSender()
        {
            siteMail = ConfigurationManager.AppSettings["SiteMail"];
            passWord = ConfigurationManager.AppSettings["EmailPassword"];
            host = ConfigurationManager.AppSettings["host"];
        }

        public void Send(string subject, string text, string userMail)
        {
            SmtpClient smtpClient = null;

            MailMessage message = null;
            try
            {
                message = new MailMessage(new MailAddress(siteMail), new MailAddress(userMail))
                {
                    Subject = subject,
                    Body = text
                };
                smtpClient = new SmtpClient(host, Convert.ToInt32(ConfigurationManager.AppSettings["port"]))
                {
                    Credentials = new NetworkCredential(siteMail, passWord)
                };
                smtpClient.Send(message);
            }
            catch (SmtpException)
            {
                if (message != null)
                    message.Dispose();
                if (smtpClient != null)
                    smtpClient.Dispose();
                throw;
            }
        }
    }
}