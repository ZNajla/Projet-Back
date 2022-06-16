using Application.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Models.Entitys
{
    public class DocumentState
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public State StateDocument { get; set; }

        public int NumeroState { get; set; }

        public DateTime Date { get; set; }

        public string Comment { get; set; }

        public Guid DocumentId { get; set; }
        public virtual Document Document { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

    }
}