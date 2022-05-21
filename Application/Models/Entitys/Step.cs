using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Entitys
{
    public class Step
    { 
        public string Action { get; set; }

        public string NumStep { get; set; }

        public string Commentaire { get; set; }

        [Key]
        public string UserId { get; set; }
        public User User { get; set; }

        [Key]
        public string DocumentId { get; set; }
        public Document Document { get; set; }
    }
}
