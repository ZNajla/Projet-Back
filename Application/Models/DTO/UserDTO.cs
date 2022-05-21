namespace Application.Models.DTO
{
    public class UserDTO
    {
        public UserDTO(string id, string fullName, string userName, string email, string phoneNumber, string adresse, string role )
        {
            Id = id;
            FullName = fullName;
            UserName = userName;
            Email = email;
            PhoneNumber = phoneNumber;
            Adresse = adresse;
            Role = role;
        }

        public string Id { get; set; }

        public string FullName { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Adresse { get; set; }

        public string Role { get; set; }

        public string Token { get; set; }
    }
}
