using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace Application.Models.Entitys
{
    public class User:IdentityUser
    {
        public string FullName { get; set; } = String.Empty;
        public string Adresse { get; set; } = String.Empty;
        public string Gender { get; set; } = String.Empty;
        public string Position { get; set; } = String.Empty;
        public string Function { get; set; } = String.Empty;
        public DateTime BirthDate { get; set; }
        public string Facebook { get; set; } = String.Empty;
        public string Google { get; set; } = String.Empty;
        public string Linkedin { get; set; } = String.Empty;
        public DateTime LastTimeLogedIn { get; set; }

        public virtual ICollection<Document> Documents { get; set; }
        public ICollection<Tache> Taches{ get; set; }
        public ICollection<Detail_Processus> Detail_Processus { get; set; }
    }
}
