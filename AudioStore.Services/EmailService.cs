using AudioStore.Models;
using AudioStore.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;


namespace AudioStore.Services
{
    public class EmailService : IEmailSender
    {
        private readonly SmtpClient _smtpClient;
        private readonly string senderEmail = "nkola.icic@gmail.com";
        private readonly string senderPassword = ("pcij ikvb fiyk omfq");
        private readonly string smtpServer = "smtp.gmail.com";
        private readonly int smtpPort = 587;
        public EmailService()
        {

            _smtpClient = new SmtpClient(smtpServer)
            {
                Port = smtpPort,
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = true,
            };
        }
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var mailMessage = new MailMessage(senderEmail, to, subject, body);
            await _smtpClient.SendMailAsync(mailMessage);
        }
    }
}
