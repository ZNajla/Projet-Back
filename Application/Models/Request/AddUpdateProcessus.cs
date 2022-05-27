namespace Application.Models.Request
{
    public class AddUpdateProcessus
    {
        public string NomProcessus { get; set; }

        public string Description { get; set; }

        public List<AddUpdateDetailProcess> Detail_Processus { get; set; }
    }
}
