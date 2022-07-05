using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.DTO
{
    public class DetailProcessDTO
    {
        public string Action { get; set; }

        public int Step { get; set; }

        public string username { get; set; }

        public string userEmail { get; set; }

        public DetailProcessDTO(string action, int step,  string username, string userEmail)
        {
            Action = action;
            Step = step;
            this.username = username;
            this.userEmail = userEmail;
        }
    }
}
