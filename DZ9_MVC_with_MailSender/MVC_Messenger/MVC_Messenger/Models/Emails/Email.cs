using System;
using System.ComponentModel.DataAnnotations;

namespace MVC_Messenger.Models
{
    public class Email
    {
        public int EmailID { get; set; }

        public string Sender { get; set; }
        public string Receiver { get; set; }

        public DateTime SendDateTime { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
