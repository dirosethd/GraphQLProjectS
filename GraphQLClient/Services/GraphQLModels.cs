namespace GraphQLClient.Services.Models;

public class StudentDto
{
    public int Id { get; set; }
    public string LastName { get; set; } = "";
    public string FirstName { get; set; } = "";
    public int SchoolClassId { get; set; }
}

public class TeacherDto
{
    public int Id { get; set; }
    public string LastName { get; set; } = "";
    public string FirstName { get; set; } = "";
}

public class ClassJournalDto
{
    public string ClassName { get; set; } = "";
    public string SubjectName { get; set; } = "";
    public List<ClassJournalRowDto> Rows { get; set; } = new();
}

public class ClassJournalRowDto
{
    public string StudentFullName { get; set; } = "";
    public List<int> Grades { get; set; } = new();
    public double Average { get; set; }
}

public class StudentCardDto
{
    public string StudentFullName { get; set; } = "";
    public string ClassName { get; set; } = "";
    public List<StudentSubjectAverageDto> AveragesBySubject { get; set; } = new();
}

public class StudentSubjectAverageDto
{
    public string SubjectName { get; set; } = "";
    public double Average { get; set; }
}