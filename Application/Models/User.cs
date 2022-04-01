using Microsoft.AspNetCore.Identity;

namespace Application.Models
{
    public class User:IdentityUser
    {
        public string FullName { get; set; } = String.Empty;
        public string Adresse { get; set; } = String.Empty;
    }
}
