using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Entities.Configuration;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("Student");
        builder.Property(s => s.Age);
        builder.Property(s => s.IsRegularStudent)
            .HasDefaultValue(true);

        builder.HasMany(s => s.Evaluations)
            .WithOne(s => s.Student)
            .HasForeignKey(s => s.StudentId);

        builder.HasQueryFilter(s => !s.Deleted);

        builder.HasData
        (
            new Student
            {
                Id = Guid.NewGuid(),
                Name = "John Doe",
                Age = 30
            },
            new Student
            {
                Id = Guid.NewGuid(),
                Name = "Jane Doe",
                Age = 25
            },
            new Student
            {
                Id = Guid.NewGuid(),
                Name = "Mike Miles",
                Age = 28
            }
        );
    }
}
