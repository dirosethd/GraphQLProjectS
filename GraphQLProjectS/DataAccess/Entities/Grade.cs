using System;
using System.Collections.Generic;

namespace GraphQLProjectS.DataAccess.Entities;

public partial class Grade
{
    public int Id { get; set; }

    public int StudentId { get; set; }

    public int SubjectId { get; set; }

    public DateOnly Date { get; set; }

    public int Value { get; set; }

    public virtual Subject Subject { get; set; } = null!;
}
