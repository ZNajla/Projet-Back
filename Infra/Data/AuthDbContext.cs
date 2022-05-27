using Application.Models.Entitys;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data
{
    public class AuthDbContext : IdentityDbContext<User, IdentityRole, string>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Step>()
                .HasKey(c => new { c.UserId, c.DocumentId });
            modelBuilder.Entity<Detail_Processus>()
                .HasKey(c => new { c.UserId, c.ProcessusId });
        }

        public DbSet<Document> Documents { get; set; }
        public DbSet<Tache> Taches { get; set; }
        public DbSet<Types> Types { get; set; }
        public DbSet<Processus> Processus { get; set; }
        public DbSet<Detail_Processus> Detail_Processus { get; set; }
        public DbSet<Step> Steps { get; set; }

    }
}
