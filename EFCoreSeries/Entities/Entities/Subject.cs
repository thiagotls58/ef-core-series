﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Entities;

public class Subject
{
    [Column("SubjectId")]
    public Guid Id { get; set; }
    public string SubjectName { get; set; }

    public ICollection<StudentSubject> StudentSubjects { get; set; }
}
