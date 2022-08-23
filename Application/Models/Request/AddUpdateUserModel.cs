namespace Application.Models.Request
{
    public class AddUpdateUserModel
    {
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

        public string Password { get; set; }
    }
}
