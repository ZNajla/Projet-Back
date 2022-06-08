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

        public string Etat { get; set; }

        public string Commentaire { get; set; }

        public string username { get; set; }

        public string userEmail { get; set; }

        public DetailProcessDTO(string action, int step, string etat, string commentaire, string username, string userEmail)
        {
            Action = action;
            Step = step;
            Etat = etat;
            Commentaire = commentaire;
            this.username = username;
            this.userEmail = userEmail;
        }
    }
}
