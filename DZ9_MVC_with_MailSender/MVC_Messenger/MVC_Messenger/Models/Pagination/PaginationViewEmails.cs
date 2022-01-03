using System.Collections.Generic;

namespace MVC_Messenger.Models
{
    public class PaginationViewEmails
    {
        public IEnumerable<Email> Emails { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
