using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Models.Entitys
{
    public class Tache
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public string Action { get; set; }

        public DateTime DateCreation { get; set; }

        public virtual User User { get; set; }

        public virtual Document Document { get; set; }

    }
}
