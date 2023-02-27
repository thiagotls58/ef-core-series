using Entities.Configuration;
using Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace Entities;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Student> Students { get; set; }
    public DbSet<StudentDetails> StudentDetails { get; set; }
    public DbSet<Evaluation> Evaluations { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<StudentConfiguration> StudentConfigurations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new StudentConfiguration());
        modelBuilder.ApplyConfiguration(new StudentSubjectConfiguration());
    }
}
