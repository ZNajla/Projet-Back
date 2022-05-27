using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Models.Entitys
{
    public class Types
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public string Nom { get; set; }

        public virtual Processus Processus { get; set; }

        public virtual ICollection<Document> Documents { get; set; }

    }
}
