using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace TopLearn.Core.Senders
{
    public class EmailSender
    {
        
        public static Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            mail.From = new MailAddress("emailsender.amir@gmail.com");
            mail.To.Add(email);
            mail.Subject = subject;
            mail.Body = htmlMessage;
            mail.IsBodyHtml = true;

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("emailsender.amir@gmail.com", "mbzmsdiorbqshzgx");
            
            SmtpServer.EnableSsl = true; // only for port 465
            SmtpServer.Send(mail);
            return Task.CompletedTask;
        }
    }
}