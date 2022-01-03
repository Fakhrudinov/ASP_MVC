using MimeKit;
using MVC_Messenger.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVC_Messenger.Data.Repository
{
    public interface IDataBaseRepository
    {
        Task<List<string>> GetAllAdminsEmail();
        Task<List<string>> GetAllUsersEmail();
        Task<string> GetUserEmailAsync(int id);
        Task SetNewEmailList(Email newEmail, InternetAddressList emailsList);
    }
}
