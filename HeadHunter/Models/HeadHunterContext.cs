using System.ComponentModel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HeadHunter.Models
{
    public class HeadHunterContext : IdentityDbContext<User>
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Resume> Resumes { get; set; }
        public DbSet<Vacancy> Vacancies { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Respond> Responds { get; set; }
        public DbSet<JobExperience> JobExperiences { get; set; }
        public DbSet<Qualification> Qualifications { get; set; }

        public HeadHunterContext(DbContextOptions options) : base(options)
        {
        }
        
    }
}