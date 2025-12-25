using System;
using System.Collections.Generic;

namespace GraphQLProjectS.DataAccess.Entities;

public partial class Subject
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
}
