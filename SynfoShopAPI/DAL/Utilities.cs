using Microsoft.Extensions.Configuration;
using MimeKit;
using System;

namespace SynfoShopAPI.DAL
{
    public class Utilities
    {
        private readonly IConfiguration _config;
        public Utilities(IConfiguration config)
        {
            _config = config;
        }
        public void SendMailInfo(string Subject, string Message, string ToEmail)
        {
            string fromMail = _config["MailSetting:mailUserId"].ToString();
            string mailPassword = _config["MailSetting:mailPassword"].ToString();
            string mailServer = _config["MailSetting:mailServer"].ToString();
            int mailPort = Convert.ToInt32(_config["MailSetting:mailPort"].ToString());


            var email = new MimeMessage();

            email.From.Add(new MailboxAddress("SynfoShop", fromMail));
            email.To.Add(new MailboxAddress("Receiver Name", ToEmail));

            email.Subject = Subject;
            var bodyBuilder = new BodyBuilder();

            bodyBuilder.HtmlBody = Message;
            email.Body = bodyBuilder.ToMessageBody();

            using (var smtp = new MailKit.Net.Smtp.SmtpClient())
            {
                smtp.Connect(mailServer, mailPort, false);
                smtp.Authenticate(fromMail, mailPassword);
                smtp.Send(email);
                smtp.Disconnect(true);
            }

            //MailMessage message = new MailMessage();
            //message.From = new MailAddress(fromMail);
            //message.To.Add(ToEmail);
            //message.Subject = Subject;
            //message.IsBodyHtml = true;
            //message.Body = Message;
            //System.Net.Mail.SmtpClient ss = new System.Net.Mail.SmtpClient(mailServer);
            //ss.Port = mailPort;
            //ss.UseDefaultCredentials = true;
            //ss.Send(message);
        }
        public static int validateInt(object value)
        {
            int amount;
            try
            {
                amount = Convert.ToInt32(value);
            }
            catch
            {
                amount = 0;
            }
            return amount;
        }
    }
}