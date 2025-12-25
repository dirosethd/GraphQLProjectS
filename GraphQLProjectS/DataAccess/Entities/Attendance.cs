using System;
using System.Collections.Generic;

namespace GraphQLProjectS.DataAccess.Entities;

public partial class Attendance
{
    public int Id { get; set; }

    public int StudentId { get; set; }

    public DateOnly Date { get; set; }

    public bool IsExcused { get; set; }

    public string? Reason { get; set; }

    public virtual Student Student { get; set; } = null!;
}
