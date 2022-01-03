using System.Threading.Tasks;

namespace MVC_Messenger.Services
{
    public class SmsSender : ISmsSender
    {
        public async Task SendSmsFromUserToUserAsync(string PhoneNumber, string message)
        {
            throw new System.NotImplementedException();
        }
    }
}
