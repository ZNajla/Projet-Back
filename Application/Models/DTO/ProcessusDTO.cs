using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.DTO
{
    public class ProcessusDTO
    {
        public string NomProcessus { get; set; }

        public string Description { get; set; }

        public DateTime Date_debut { get; set; }

        public DateTime Date_fin { get; set; }

        public string Types { get; set; }

        public ProcessusDTO(string nomProcessus, string description, DateTime date_debut, DateTime date_fin, string types)        {
            NomProcessus = nomProcessus;
            Description = description;
            Date_debut = date_debut;
            Date_fin = date_fin;
            Types = types;
        }   

    }
}
