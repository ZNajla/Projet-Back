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

        public ProcessusDTO(string nomProcessus, string description)
        {
            NomProcessus = nomProcessus;
            Description = description;
        }   

    }
}
