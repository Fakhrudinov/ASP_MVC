using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using MimeKit;
using MVC_Messenger.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace MVC_Messenger.Data.Repository
{
    public class DataBaseRepository : IDataBaseRepository
    {
        // не возможно получить зависимость transient для singleton, поэтому контекст сюда внедрить не удается
        //private MessengerContext _context;
        //public UserRepository(MessengerContext context)
        //{
        //    _context = context;
        //}

        private string connectionString;

        public DataBaseRepository(IOptions<DataBaseConnection> settings)
        {
            connectionString = settings.Value.DefaultConnection;
        }

        public async Task<List<string>> GetAllUsersEmail()
        {
            string sqlExpression = "SELECT Email FROM Users u where u.RoleId = 2";
            return await GetEmailsFromDataBase(sqlExpression);
        }

        public async Task<string> GetUserEmailAsync(int id)
        {
            string sqlExpression = "SELECT Email FROM Users u where u.UserID = " + id;
            return await GetSingleEmailFromDataBase(sqlExpression);
        }

        private async Task<string> GetSingleEmailFromDataBase(string sqlExpression)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand(sqlExpression, connection);
                var result = await command.ExecuteScalarAsync();
                return result.ToString();
            }            
        }

        async Task<List<string>> IDataBaseRepository.GetAllAdminsEmail()
        {
            string sqlExpression = "SELECT Email FROM Users u where u.RoleId = 1";
            return await GetEmailsFromDataBase(sqlExpression);
        }

        private async Task<List<string>> GetEmailsFromDataBase(string sqlExpression)
        {
            List<string> result = new List<string>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand(sqlExpression, connection);
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        result.Add(reader.GetString(0));
                    }
                }
            }

            return result;
        }

        public async Task SetNewEmailList(Email newEmail, InternetAddressList emailsList)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();                

                foreach (MailboxAddress email in emailsList)
                {
                    string sqlExpression = "INSERT INTO Emails (Sender, Receiver, SendDateTime, Subject, Message) VALUES (@Sender, @Receiver, @SendDateTime, @Subject, @Message)";

                    SqlCommand command = new SqlCommand(sqlExpression, connection);

                    SqlParameter sender = new SqlParameter("@Sender", newEmail.Sender);
                    command.Parameters.Add(sender);
                    SqlParameter receiver = new SqlParameter("@Receiver", email.Address);
                    command.Parameters.Add(receiver);                    
                    SqlParameter sendDateTime = new SqlParameter("@SendDateTime", newEmail.SendDateTime);
                    command.Parameters.Add(sendDateTime);
                    SqlParameter subject = new SqlParameter("@Subject", newEmail.Subject);
                    command.Parameters.Add(subject);
                    SqlParameter message = new SqlParameter("@Message", newEmail.Message);
                    command.Parameters.Add(message);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
