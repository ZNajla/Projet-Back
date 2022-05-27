using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Models.Entitys
{
    public class Document
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public string Url { get; set; }

        public string Reference { get; set; }

        public string Titre { get; set; }

        public int NbPage { get; set; }

        public string MotCle { get; set; }

        public string Version { get; set; }

        public DateTime Date { get; set; }

        public DateTime DateUpdate { get; set; }

        public virtual User User { get; set; }

        public virtual Types Types { get; set; }

        public ICollection<Step> Steps { get; set; }

    }
}
