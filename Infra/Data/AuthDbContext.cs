using Application.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data
{
    public class AuthDbContext : IdentityDbContext<User, IdentityRole, string>
    {
        public AuthDbContext(DbContextOptions options) : base(options)
        {
        }

    }
}
