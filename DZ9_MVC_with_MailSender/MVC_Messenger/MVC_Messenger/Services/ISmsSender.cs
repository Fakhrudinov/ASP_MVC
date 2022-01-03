using System.Threading.Tasks;

namespace MVC_Messenger.Services
{
    public interface ISmsSender
    {
        Task SendSmsFromUserToUserAsync(string PhoneNumber, string message);
    }
}
