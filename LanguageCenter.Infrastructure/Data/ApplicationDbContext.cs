using LanguageCenter.Infrastructure.Data.Models;
using LanguageCenter.Infrastructure.InitialSeed;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LanguageCenter.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Certificate> Certificates { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Language> Languages { get; set; }

        public DbSet<Teacher> Teachers { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new InitialDbConfiguration<Language>(@"C:\Users\Alex95\Desktop\LanguageCenter\LanguageCenter\LanguageCenter.Infrastructure\Seed\languages.json"));
            builder.ApplyConfiguration(new InitialDbConfiguration<Course>(@"C:\Users\Alex95\Desktop\LanguageCenter\LanguageCenter\LanguageCenter.Infrastructure\Seed\courses.json"));

            base.OnModelCreating(builder);
        }
    }
}