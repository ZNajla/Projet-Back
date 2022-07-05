using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Request
{
    public class MailRequest
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public MailRequest()
        {
        }

        public MailRequest(string email , string subject , string body )
        {
            this.ToEmail = email;
            this.Subject = subject;
            this.Body = body;
        }
    }
}
