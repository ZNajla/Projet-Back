using Application.Models.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.DTO
{
    public class DocumentDTO
    {
        public string ID { get; set; }

        public string Url { get; set; }

        public string Reference { get; set; }

        public string Titre { get; set; }

        public int NbPage { get; set; }

        public string MotCle { get; set; }

        public string Version { get; set; }

        public DateTime Date { get; set; }

        public DateTime DateUpdate { get; set; }

        public UserDTO User { get; set; }

        public string Type { get; set; }

        //public ICollection<Step> CurrentState { get; set; }

        public DocumentDTO(string id , string url, string reference, string titre, int nbPage, string motCle, string version, DateTime date, UserDTO user, string type, DateTime dateUpdate)
        {
            ID = id;
            Url = url;
            Reference = reference;
            Titre = titre;
            NbPage = nbPage;
            MotCle = motCle;
            Version = version;
            Date = date;
            User = user;
            Type = type;
            DateUpdate = dateUpdate;
            // CurrentState = currentState;
        }
    }
}
