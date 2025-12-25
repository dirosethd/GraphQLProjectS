namespace GraphQLProjectS.Models
{
    public class ClassJournalRow
    {
        public int StudentId { get; set; }
        public string StudentFullName { get; set; } = default!;
        public List<int> Grades { get; set; } = new();
        public double Average { get; set; }
    }
}
