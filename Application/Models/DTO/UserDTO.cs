namespace Application.Models.DTO
{
    public class UserDTO
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Adresse { get; set; }

        public string Gender { get; set; }

        public string Position { get; set; }

        public string Function { get; set; }

        public DateTime BirthDate { get; set; }

        public string Facebook { get; set; }

        public string Google { get; set; } 

        public string Linkedin { get; set; } 

        public DateTime LastTimeLogedIn { get; set; }

        public string Role { get; set; }

        public string Token { get; set; }

        public UserDTO(string id, string fullName, string userName, string email, string phoneNumber,
            string adresse, string gender , string position , string function , DateTime birthDate , string facebook , string google , string linkedin ,
            DateTime lastTimeLogedIn , string role)
        {
            Id = id;
            FullName = fullName;
            UserName = userName;
            Email = email;
            PhoneNumber = phoneNumber;
            Adresse = adresse;
            Gender = gender;
            Position = position;
            Function = function;
            BirthDate = birthDate;
            Facebook = facebook;
            Google = google;
            Linkedin = linkedin;
            LastTimeLogedIn = lastTimeLogedIn;
            Role = role;
        }
    }
}
