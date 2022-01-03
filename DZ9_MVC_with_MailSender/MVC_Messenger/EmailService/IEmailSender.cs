using MimeKit;
using System.Threading.Tasks;

namespace MVC_Messenger.Services
{
    public interface IEmailSender
    {
        Task SendEmailsToUsersAsync(InternetAddressList emailsList, string subject, string message);
        Task SendEmailsToAdminsAsync(InternetAddressList emailsList, string subject, string message);
        Task SendEmailFromUserToUserAsync(MailboxAddress emailAdress, string subject, string message);
    }
}
