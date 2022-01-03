using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MVC_Messenger.Models;

namespace MVC_Messenger.Services
{
    public class EmailSender : IEmailSender
    {
        public EmailSMTPOptions Options { get; set; }
        public EmailSender(IOptions<EmailSMTPOptions> options)
        {
            this.Options = options.Value;
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
            emailMessage.Sender = MailboxAddress.Parse(Options.Sender_EMail);
            if (!string.IsNullOrEmpty(Options.Sender_Name))
            {
                emailMessage.Sender.Name = Options.Sender_Name;
            }
            emailMessage.From.Add(emailMessage.Sender);

            await Task.Run(async () =>
            {
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(Options.Host_Address, Options.Host_Port, Options.Host_SecureSocketOptions);
                    await client.AuthenticateAsync(Options.Host_Username, Options.Host_Password);

                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                }
            });
        }
    }
}
