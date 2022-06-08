using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Models.Entitys
{
    public class Processus
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string NomProcessus { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Types> Types { get; set; }

        public virtual ICollection<Detail_Processus> Detail_Processus { get; set; }
    }
}
