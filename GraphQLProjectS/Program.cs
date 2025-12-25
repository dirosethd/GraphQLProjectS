using GraphQLProjectS.DataAccess;
using GraphQLProjectS.GraphQL;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<SchoolJournalDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("SchoolDb")));


builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>();


builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(p =>
        p.AllowAnyOrigin()
         .AllowAnyHeader()
         .AllowAnyMethod());
});

var app = builder.Build();


app.UseCors();

app.MapGet("/", () => "API is running");


app.MapGraphQL("/graphql");


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SchoolJournalDbContext>();
    await db.Database.EnsureCreatedAsync();
    DbSeeder.Seed(db);
}

app.Run();
