using Entities.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities;

public class Student
{
    [Column("StudentId")]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public bool IsRegularStudent { get; set; }
    public bool Deleted { get; set; }

    public StudentDetails StudentDetails { get; set; }
    
    public ICollection<Evaluation> Evaluations { get; set; }

    public ICollection<StudentSubject> StudentSubjects { get; set; }
}
