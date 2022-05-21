using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Models.Entitys
{
    public class Processus
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string NomProcessus { get; set; }

        public string Description { get; set; }

        public DateTime Date_debut { get; set; }

        public DateTime Date_fin { get; set; }

        public virtual Types Types { get; set; }

        public ICollection<Detail_Processus> Detail_Processus { get; set; }
    }
}
