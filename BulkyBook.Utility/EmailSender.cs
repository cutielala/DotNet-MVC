using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var emailToSend = new MimeMessage();
            emailToSend.From.Add(MailboxAddress.Parse("cutielala@hotmail.com"));
            emailToSend.To.Add(MailboxAddress.Parse(email));
            emailToSend.Subject = subject;
            emailToSend.Body = new TextPart(htmlMessage);

            //send email
            using(var emailClient = new SmtpClient())
            {
                emailClient.Connect("@smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                //emailClient.Authenticate("dotnetmastery@gmail.com","DotNet213$"); 
                emailClient.Authenticate("hikvisionwebservice@gmail.com", "Hikvison91748!");
                emailClient.Send(emailToSend);
                emailClient.Disconnect(true);
            }

            return Task.CompletedTask;

        }
    }
}
