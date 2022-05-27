using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Models.Entitys
{
    public class Tache
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public string Nom { get; set; }

        public string Description { get; set; }

        public DateTime DateDebut { get; set; }

        public DateTime DateFin { get; set; }

        public string Etat { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
