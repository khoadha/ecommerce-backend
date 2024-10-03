﻿using DataAccessLayer.BusinessModels;
using MailKit.Net.Smtp;
using MimeKit;
using System.Text;

namespace EXE101_API.Services.EmailService
{
    public class EmailService : IEmailService {

        private readonly EmailConfiguration _emailConfig;
        public EmailService(EmailConfiguration emailConfig) {
            _emailConfig = emailConfig;
        }
        public void SendEmail(Message message) {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);
        }

        private void Send(MimeMessage mailMessage) {
            using (var client = new SmtpClient()) {
                try {
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfig.UserName, _emailConfig.Password);
                    client.Send(mailMessage);
                } catch {
                    throw;
                } finally {
                    client.Disconnect(true);
                    mailMessage.Dispose();
                    client.Dispose();
                }
            }
        }

        private MimeMessage CreateEmailMessage(Message message) {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(Encoding.UTF8, message.Subject, _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Content };
            return emailMessage;
        }

    }
}

