using Application.Models.DTO;

namespace Application.Models.Request
{
    public class AddUpdateDetailProcess
    {
        public string Action { get; set; }

        public int Step { get; set; }

        public string Etat { get; set; }

        public string Commentaire { get; set; }

        public string User { get; set; }

        public string ProcessusId { get; set; }
    }
}
