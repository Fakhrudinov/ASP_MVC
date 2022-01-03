using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MVC_Messenger.Models;

namespace MVC_Messenger.Services
{
    public class EmailSender : IEmailSender
    {
        private EmailSMTPOptions _options { get; set; }
        public EmailSender(IOptions<EmailSMTPOptions> options)
        {
            _options = options.Value;
        }

        public async Task SendEmailsToUsersAsync(InternetAddressList emailsList, string subject, string message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.Bcc.AddRange(emailsList);
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message };

            await SendEmail(emailMessage);
        }

        public async Task SendEmailsToAdminsAsync(InternetAddressList emailsList, string subject, string message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.Cc.AddRange(emailsList);
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message };

            await SendEmail(emailMessage);
        }

        public async Task SendEmailFromUserToUserAsync(MailboxAddress emailsAdress, string subject, string message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.To.Add(emailsAdress);
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message };

            await SendEmail(emailMessage);
        }

        private async Task SendEmail(MimeMessage emailMessage)
        {
            // здесь я пеерзаписываю актуального отправителя на прописанного в настройках SMTP сервера
            // - бесплатные смтп сервисы не дают отправлять с любого адреса, только от авторизованных.
            emailMessage.Sender = MailboxAddress.Parse(_options.Sender_EMail);
            if (!string.IsNullOrEmpty(_options.Sender_Name))
            {
                emailMessage.Sender.Name = _options.Sender_Name;
            }
            emailMessage.From.Add(emailMessage.Sender);

            await Task.Run(async () =>
            {
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_options.Host_Address, _options.Host_Port, _options.Host_SecureSocketOptions);
                    await client.AuthenticateAsync(_options.Host_Username, _options.Host_Password);

                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                }
            });
        }
    }
}
