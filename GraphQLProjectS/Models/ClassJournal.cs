namespace GraphQLProjectS.Models
{
    public class ClassJournal
    {
        public string ClassName { get; set; } = default!;
        public string SubjectName { get; set; } = default!;
        public List<ClassJournalRow> Rows { get; set; } = new();
    }
}
