using System.Collections.Generic;

namespace MVC_Messenger.Models
{
    public class PaginationViewUsers
    {
        public IEnumerable<User> Users { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
