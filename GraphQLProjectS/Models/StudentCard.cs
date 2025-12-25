namespace GraphQLProjectS.Models
{
    public class StudentCard
    {
        public int StudentId { get; set; }
        public string StudentFullName { get; set; } = default!;
        public string ClassName { get; set; } = default!;
        public List<StudentSubjectAverage> AveragesBySubject { get; set; } = new();
    }
}
