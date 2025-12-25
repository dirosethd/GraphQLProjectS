using GraphQLProjectS.DataAccess;
using GraphQLProjectS.DataAccess.Entities;
using GraphQLProjectS.Models;
using Microsoft.EntityFrameworkCore;

namespace GraphQLProjectS.GraphQL
{
    public class Query
    {
        public IQueryable<Student> GetStudents([Service] SchoolJournalDbContext db) =>
            db.Students.AsNoTracking();

        public IQueryable<Teacher> GetTeachers([Service] SchoolJournalDbContext db) =>
            db.Teachers.AsNoTracking();

        public async Task<List<Subject>> GetSubjectsForClass(
            int classId,
            [Service] SchoolJournalDbContext db)
        {
            return await db.TeachingAssignments
                .Where(t => t.SchoolClassId == classId)
                .Join(db.Subjects,
                    ta => ta.SubjectId,
                    s => s.Id,
                    (ta, s) => s)
                .Distinct()
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<List<Teacher>> GetTeachersForClass(
            int classId,
            [Service] SchoolJournalDbContext db)
        {
            return await db.TeachingAssignments
                .Where(t => t.SchoolClassId == classId)
                .Join(db.Teachers,
                    ta => ta.TeacherId,
                    t => t.Id,
                    (ta, t) => t)
                .Distinct()
                .OrderBy(t => t.LastName)
                .ToListAsync();
        }

        public async Task<List<Teacher>> GetTeachersForSubject(
            int subjectId,
            [Service] SchoolJournalDbContext db)
        {
            return await db.TeachingAssignments
                .Where(t => t.SubjectId == subjectId)
                .Join(db.Teachers,
                    ta => ta.TeacherId,
                    t => t.Id,
                    (ta, t) => t)
                .Distinct()
                .OrderBy(t => t.LastName)
                .ToListAsync();
        }

        public async Task<int> GetAbsencesCountForClass(
            int classId,
            DateOnly from,
            DateOnly to,
            [Service] SchoolJournalDbContext db)
        {
            return await db.Attendances
                .Where(a => a.Date >= from && a.Date <= to)
                .Join(db.Students.Where(s => s.SchoolClassId == classId),
                    a => a.StudentId,
                    s => s.Id,
                    (a, s) => a)
                .CountAsync();
        }

        public async Task<double> GetAverageGradeForClass(
            int classId,
            [Service] SchoolJournalDbContext db)
        {
            var values = await db.Grades
                .Join(db.Students.Where(s => s.SchoolClassId == classId),
                    g => g.StudentId,
                    s => s.Id,
                    (g, s) => g.Value)
                .ToListAsync();

            return values.Count == 0 ? 0 : values.Average();
        }

        public async Task<double> GetAverageGradeForSubject(
            int subjectId,
            [Service] SchoolJournalDbContext db)
        {
            var values = await db.Grades
                .Where(g => g.SubjectId == subjectId)
                .Select(g => g.Value)
                .ToListAsync();

            return values.Count == 0 ? 0 : values.Average();
        }

        public async Task<ClassJournal> GetClassJournal(
            int classId,
            int subjectId,
            [Service] SchoolJournalDbContext db)
        {
            var cls = await db.SchoolClasses.FirstAsync(c => c.Id == classId);
            var subject = await db.Subjects.FirstAsync(s => s.Id == subjectId);

            var students = await db.Students
                .Where(s => s.SchoolClassId == classId)
                .OrderBy(s => s.LastName)
                .ToListAsync();

            var grades = await db.Grades
                .Where(g => g.SubjectId == subjectId)
                .Join(db.Students.Where(s => s.SchoolClassId == classId),
                    g => g.StudentId,
                    s => s.Id,
                    (g, s) => g)
                .ToListAsync();

            var rows = students.Select(st =>
            {
                var g = grades.Where(x => x.StudentId == st.Id).Select(x => x.Value).ToList();
                return new ClassJournalRow
                {
                    StudentId = st.Id,
                    StudentFullName = $"{st.LastName} {st.FirstName} {st.MiddleName}".Trim(),
                    Grades = g,
                    Average = g.Count == 0 ? 0 : g.Average()
                };
            }).ToList();

            return new ClassJournal
            {
                ClassName = cls.Name,
                SubjectName = subject.Name,
                Rows = rows
            };
        }

        public async Task<StudentCard> GetStudentCard(
            int studentId,
            [Service] SchoolJournalDbContext db)
        {
            var st = await db.Students.FirstAsync(s => s.Id == studentId);
            var className = await db.SchoolClasses
                .Where(c => c.Id == st.SchoolClassId)
                .Select(c => c.Name)
                .FirstAsync();

            var grades = await db.Grades
                .Where(g => g.StudentId == studentId)
                .ToListAsync();

            var subjectMap = await db.Subjects
                .ToDictionaryAsync(s => s.Id, s => s.Name);

            var grouped = grades
                .GroupBy(g => g.SubjectId)
                .Select(grp => new StudentSubjectAverage
                {
                    SubjectName = subjectMap[grp.Key],
                    Average = grp.Average(x => x.Value)
                })
                .ToList();

            return new StudentCard
            {
                StudentId = st.Id,
                StudentFullName = $"{st.LastName} {st.FirstName} {st.MiddleName}".Trim(),
                ClassName = className,
                AveragesBySubject = grouped
            };
        }
    }
}
