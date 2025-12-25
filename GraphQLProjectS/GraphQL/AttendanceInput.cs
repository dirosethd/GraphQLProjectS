namespace GraphQLProjectS.GraphQL
{
    public record AttendanceInput(int StudentId, DateOnly Date, bool IsExcused, string? Reason);
}
