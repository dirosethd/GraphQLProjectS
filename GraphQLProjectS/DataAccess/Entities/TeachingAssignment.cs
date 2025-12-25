using System;
using System.Collections.Generic;

namespace GraphQLProjectS.DataAccess.Entities;

public partial class TeachingAssignment
{
    public int Id { get; set; }

    public int TeacherId { get; set; }

    public int SubjectId { get; set; }

    public int SchoolClassId { get; set; }

    public virtual SchoolClass SchoolClass { get; set; } = null!;
}
