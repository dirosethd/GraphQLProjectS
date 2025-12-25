namespace GraphQLProjectS.GraphQL
{
    public record StudentInput(string LastName, string FirstName, string? MiddleName, DateOnly BirthDate, int SchoolClassId);
}
