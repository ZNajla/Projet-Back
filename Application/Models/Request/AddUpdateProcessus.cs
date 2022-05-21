using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Request
{
    public class AddUpdateProcessus
    {
        public string NomProcessus { get; set; }

        public string Description { get; set; }

        public DateTime Date_debut { get; set; }

        public DateTime Date_fin { get; set; }

        public string Types { get; set; }
    }
}
