using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Models.Entitys
{
    public class Types
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }

        public string Nom { get; set; }

        public virtual ICollection<Document> Documents { get; set; }

        public virtual ICollection<Processus> Processus { get; set; }









        public Types() { }

        public Types (string nom)
        {
            Nom = nom;
        }
    }
}
