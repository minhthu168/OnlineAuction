using eProject.Models;
using eProject.Repository;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eProject.Service
{
    public class MessageServices : Repository.IMessageServices
    {
        private readonly MailSettings _mailSettings;
        private readonly Data.DatabaseContext context;
        public MessageServices(IOptions<MailSettings> mailSettings, Data.DatabaseContext context)
        {
            _mailSettings = mailSettings.Value;
            this.context = context;
        }

        public void AddMail(Message message)
        {
            context.Messages.Add(message);
            context.SaveChanges();
        }

        public void SendMail(Message message, string ToEmail)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(ToEmail));
            email.Subject = message.Title;
            var builder = new BodyBuilder();
            builder.HtmlBody = message.Body;
            email.Body = builder.ToMessageBody();
            SmtpClient smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            smtp.Send(email);
            smtp.Disconnect(true);
        }

       
    }
}
