using GraphQLProjectS.DataAccess;
using GraphQLProjectS.GraphQL;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ===== DB =====
builder.Services.AddDbContext<SchoolJournalDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("SchoolDb")));

// ===== GraphQL =====
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>();

// ===== CORS (для Blazor WASM) =====
builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(p =>
        p.AllowAnyOrigin()
         .AllowAnyHeader()
         .AllowAnyMethod());
});

var app = builder.Build();

// CORS обязательно ДО MapGraphQL
app.UseCors();

// просто чтобы проверить что сервер живой
app.MapGet("/", () => "API is running");

// GraphQL endpoint
app.MapGraphQL("/graphql");

// ===== optional seed =====
// ВНИМАНИЕ: если DbSeeder ломает старт — временно закомментируй эти 6 строк
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SchoolJournalDbContext>();
    await db.Database.EnsureCreatedAsync();
    DbSeeder.Seed(db);
}

app.Run();
