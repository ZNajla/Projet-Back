using System.ComponentModel.DataAnnotations;

namespace Application.Models.Entitys
{
    public class Step
    {
        public DateTime date_debut { get; set; }

        [Key]
        public string UserId { get; set; }
        public User User { get; set; }

        [Key]
        public Guid DocumentId { get; set; }
        public Document Document { get; set; }
    }
}
