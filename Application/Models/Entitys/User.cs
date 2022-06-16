using Microsoft.AspNetCore.Identity;

namespace Application.Models.Entitys
{
    public class User:IdentityUser
    {
        public string FullName { get; set; } = String.Empty;
        public string Adresse { get; set; } = String.Empty;

        public virtual ICollection<Document> Documents { get; set; }
       // public ICollection<Tache> Taches{ get; set; }
        public ICollection<Detail_Processus> Detail_Processus { get; set; }
    }
}
