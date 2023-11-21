using Castle.Core.Configuration;
using Ecom.Jarad.Core.DTOS;
using Ecom.Jarad.Core.Interfaces;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Jarad.Infrastructure.Repositries
{
    public class EmailSendReposirty : IEmailSender
    {
      
        public void sendEmail(EmailModelDTO email)
        {
            MimeMessage Email = new MimeMessage();
            string from = "ahmad222jarad@gmail.com";
            Email.From.Add(new MailboxAddress("ahmad222jarad", from));
            Email.To.Add(new MailboxAddress(email.To, email.To));
            Email.Subject = email.Subject;
            Email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = string.Format(email.Content),
            };
            using (var Client = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    Client.Connect("smtp.gmail.com", 465, true);
                    Client.Authenticate("ahmad222jarad@gmail.com", "ttcobueavyosyyqk");
                    Client.Send(Email);
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    Client.Disconnect(true);
                    Client.Dispose();
                }
            }
        }
    }
}
