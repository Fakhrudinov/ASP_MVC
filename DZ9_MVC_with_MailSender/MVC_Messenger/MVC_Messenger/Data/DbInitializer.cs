using MVC_Messenger.Models;
using System.Linq;

namespace MVC_Messenger.Data
{
    public class DbInitializer
    {
        public static void Initialize(MessengerContext context)
        {
            context.Database.EnsureCreated();

            if (context.Users.Any())
            {
                return;
            }


            var roles = new Role[]
            {
                new Role{Id = 1, Name = "admin"},
                new Role{Id = 2, Name = "user"}
            };
            foreach (Role role in roles)
            {
                context.Roles.Add(role);
            }
            context.SaveChanges();


            var users = new User[]
            {
                new User{
                    Name="admin", 
                    Password="12345", 
                    Email="asbuka1975@yandex.ru", 
                    PhoneNumber="123456789", 
                    RoleId=1},
                new User{
                    Name="user1 56789",
                    Password="56789",
                    Email="asbuka@gmail.com",
                    PhoneNumber="798465132",
                    RoleId=2},
                new User{
                    Name="some work acc 11111",
                    Password="11111",
                    Email="Aleksandr.Fakhrudinov@iticapital.ru",
                    PhoneNumber="798465132",
                    RoleId=2},
                new User{
                    Name="some Fake User 22222",
                    Password="22222",
                    Email="fakeEmail2@fakeServerABCD.ru",
                    PhoneNumber="798465333",
                    RoleId=2},
            };            
            foreach (User user in users)
            {
                context.Users.Add(user);
            }
            context.SaveChanges();
        }
    }
}
