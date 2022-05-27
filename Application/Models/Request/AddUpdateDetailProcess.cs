using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Request
{
    public class AddUpdateDetailProcess
    {
        public string Action { get; set; }

        public int Step { get; set; }

        public string Etat { get; set; }

        public string Commentaire { get; set; }

        public string UserId { get; set; }

        public string ProcessusId { get; set; }
    }
}
