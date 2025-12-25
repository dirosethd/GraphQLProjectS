using System.Net.Http.Json;
using GraphQLClient.Services.Models;

namespace GraphQLClient.Services;

public class GraphQLService
{
    private readonly HttpClient _http;

    public GraphQLService(HttpClient http)
    {
        _http = http;
    }

    // Универсальный вызов GraphQL, возвращает объект data
    private async Task<TData> Send<TData>(string query)
    {
        var resp = await _http.PostAsJsonAsync("graphql", new { query });
        resp.EnsureSuccessStatusCode();

        var wrapper = await resp.Content.ReadFromJsonAsync<GraphQLResponse<TData>>();
        if (wrapper is null)
            throw new Exception("GraphQL response is null");

        if (wrapper.Errors is not null && wrapper.Errors.Length > 0)
            throw new Exception(wrapper.Errors[0].Message);

        return wrapper.Data;
    }

    // =======================
    // QUERIES
    // =======================

    public async Task<List<StudentDto>> GetStudents()
    {
        var data = await Send<GetStudentsData>(
            "query { students { id lastName firstName schoolClassId } }"
        );
        return data.Students ?? new List<StudentDto>();
    }

    public async Task<List<TeacherDto>> GetTeachers()
    {
        var data = await Send<GetTeachersData>(
            "query { teachers { id lastName firstName } }"
        );
        return data.Teachers ?? new List<TeacherDto>();
    }

    public async Task<ClassJournalDto> GetJournal(int classId, int subjectId)
    {
        var data = await Send<GetJournalData>(
            $"query {{ classJournal(classId:{classId}, subjectId:{subjectId}) {{ className subjectName rows {{ studentFullName grades average }} }} }}"
        );
        return data.ClassJournal!;
    }

    public async Task<StudentCardDto> GetStudentCard(int studentId)
    {
        var data = await Send<GetStudentCardData>(
            $"query {{ studentCard(studentId:{studentId}) {{ studentFullName className averagesBySubject {{ subjectName average }} }} }}"
        );
        return data.StudentCard!;
    }

    // =======================
    // MUTATIONS
    // =======================

    public async Task<StudentDto> AddStudent(string lastName, string firstName, string birthDate, int classId)
    {
        var q = $@"
mutation {{
  addStudent(input:{{
    lastName:""{Escape(lastName)}"",
    firstName:""{Escape(firstName)}"",
    birthDate:""{birthDate}"",
    schoolClassId:{classId}
  }}) {{
    id lastName firstName schoolClassId
  }}
}}";

        var data = await Send<AddStudentData>(q);
        return data.AddStudent!;
    }

    // =======================
    // helpers
    // =======================
    private static string Escape(string s) =>
        s.Replace(@"\", @"\\").Replace(@"""", @"\""");
}

// ===== GraphQL envelope =====
public class GraphQLResponse<T>
{
    public T Data { get; set; } = default!;
    public GraphQLError[]? Errors { get; set; }
}

public class GraphQLError
{
    public string Message { get; set; } = "";
}

public class GetStudentsData
{
    public List<StudentDto>? Students { get; set; }
}

public class GetTeachersData
{
    public List<TeacherDto>? Teachers { get; set; }
}

public class GetJournalData
{
    public ClassJournalDto? ClassJournal { get; set; }
}

public class GetStudentCardData
{
    public StudentCardDto? StudentCard { get; set; }
}

public class AddStudentData
{
    public StudentDto? AddStudent { get; set; }
}