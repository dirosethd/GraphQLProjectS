using System;
using System.Collections.Generic;

namespace GraphQLProjectS.DataAccess.Entities;

public partial class SchoolClass
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<TeachingAssignment> TeachingAssignments { get; set; } = new List<TeachingAssignment>();
}
