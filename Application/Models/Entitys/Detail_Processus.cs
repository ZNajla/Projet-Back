using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Models.Entitys
{
    public class Detail_Processus
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }

        public string Action { get; set; }

        public int Step { get; set; }

        public string Etat { get; set; }

        public string Commentaire { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public string ProcessusId { get; set; }
        public Processus Processus { get; set; }
    }
}
