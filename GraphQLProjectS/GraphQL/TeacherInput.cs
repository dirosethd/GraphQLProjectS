namespace GraphQLProjectS.GraphQL
{
    public record TeacherInput(string LastName, string FirstName, string? MiddleName, DateOnly BirthDate);
}
