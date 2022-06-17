namespace Application.Models.Request
{
    public class AddUpdateTache
    {
        public string Action { get; set; }

        public DateTime DateCreation { get; set; }

        public string User { get; set; }

        public string Document { get; set; }
    }
}
