using GraphQLProjectS.DataAccess;
using GraphQLProjectS.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace GraphQLProjectS.GraphQL
{
    public class Mutation
    {
  
        public async Task<SchoolClass> AddClass(ClassInput input, [Service] SchoolJournalDbContext db)
        {
            var entity = new SchoolClass { Name = input.Name.Trim() };
            db.SchoolClasses.Add(entity);
            await db.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteClass(int id, [Service] SchoolJournalDbContext db)
        {
            var entity = await db.SchoolClasses.FirstAsync(x => x.Id == id);
            db.SchoolClasses.Remove(entity);
            await db.SaveChangesAsync();
            return true;
        }

    
        public async Task<Student> AddStudent(StudentInput input, [Service] SchoolJournalDbContext db)
        {
            if (!await db.SchoolClasses.AnyAsync(c => c.Id == input.SchoolClassId))
                throw new GraphQLException("SchoolClass not found.");

            var entity = new Student
            {
                LastName = input.LastName.Trim(),
                FirstName = input.FirstName.Trim(),
                MiddleName = input.MiddleName?.Trim(),
                BirthDate = input.BirthDate,             
                SchoolClassId = input.SchoolClassId
            };

            db.Students.Add(entity);
            await db.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteStudent(int id, [Service] SchoolJournalDbContext db)
        {
            var entity = await db.Students.FirstAsync(x => x.Id == id);
            db.Students.Remove(entity);
            await db.SaveChangesAsync();
            return true;
        }

    
        public async Task<Teacher> AddTeacher(TeacherInput input, [Service] SchoolJournalDbContext db)
        {
            var entity = new Teacher
            {
                LastName = input.LastName.Trim(),
                FirstName = input.FirstName.Trim(),
                MiddleName = input.MiddleName?.Trim(),
                BirthDate = input.BirthDate               
            };

            db.Teachers.Add(entity);
            await db.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteTeacher(int id, [Service] SchoolJournalDbContext db)
        {
            var entity = await db.Teachers.FirstAsync(x => x.Id == id);
            db.Teachers.Remove(entity);
            await db.SaveChangesAsync();
            return true;
        }

    
        public async Task<Subject> AddSubject(SubjectInput input, [Service] SchoolJournalDbContext db)
        {
            var entity = new Subject { Name = input.Name.Trim() };
            db.Subjects.Add(entity);
            await db.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteSubject(int id, [Service] SchoolJournalDbContext db)
        {
            var entity = await db.Subjects.FirstAsync(x => x.Id == id);
            db.Subjects.Remove(entity);
            await db.SaveChangesAsync();
            return true;
        }

  
        public async Task<TeachingAssignment> AddTeachingAssignment(TeachingAssignmentInput input, [Service] SchoolJournalDbContext db)
        {
            if (!await db.Teachers.AnyAsync(x => x.Id == input.TeacherId))
                throw new GraphQLException("Teacher not found.");

            if (!await db.Subjects.AnyAsync(x => x.Id == input.SubjectId))
                throw new GraphQLException("Subject not found.");

            if (!await db.SchoolClasses.AnyAsync(x => x.Id == input.SchoolClassId))
                throw new GraphQLException("SchoolClass not found.");

            var exists = await db.TeachingAssignments.AnyAsync(a =>
                a.TeacherId == input.TeacherId &&
                a.SubjectId == input.SubjectId &&
                a.SchoolClassId == input.SchoolClassId);

            if (exists)
                throw new GraphQLException("TeachingAssignment already exists.");

            var entity = new TeachingAssignment
            {
                TeacherId = input.TeacherId,
                SubjectId = input.SubjectId,
                SchoolClassId = input.SchoolClassId
            };

            db.TeachingAssignments.Add(entity);
            await db.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteTeachingAssignment(int id, [Service] SchoolJournalDbContext db)
        {
            var entity = await db.TeachingAssignments.FirstAsync(x => x.Id == id);
            db.TeachingAssignments.Remove(entity);
            await db.SaveChangesAsync();
            return true;
        }

    
        public async Task<Grade> AddGrade(GradeInput input, [Service] SchoolJournalDbContext db)
        {
            if (input.Value < 2 || input.Value > 5)
                throw new GraphQLException("Grade value must be between 2 and 5.");

            if (!await db.Students.AnyAsync(x => x.Id == input.StudentId))
                throw new GraphQLException("Student not found.");

            if (!await db.Subjects.AnyAsync(x => x.Id == input.SubjectId))
                throw new GraphQLException("Subject not found.");

            var entity = new Grade
            {
                StudentId = input.StudentId,
                SubjectId = input.SubjectId,
                Date = input.Date,           
                Value = input.Value
            };

            db.Grades.Add(entity);
            await db.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteGrade(int id, [Service] SchoolJournalDbContext db)
        {
            var entity = await db.Grades.FirstAsync(x => x.Id == id);
            db.Grades.Remove(entity);
            await db.SaveChangesAsync();
            return true;
        }

   
        public async Task<Attendance> AddAttendance(AttendanceInput input, [Service] SchoolJournalDbContext db)
        {
            if (!await db.Students.AnyAsync(x => x.Id == input.StudentId))
                throw new GraphQLException("Student not found.");

            var entity = new Attendance
            {
                StudentId = input.StudentId,
                Date = input.Date,           
                IsExcused = input.IsExcused,
                Reason = input.Reason?.Trim()
            };

            db.Attendances.Add(entity);
            await db.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAttendance(int id, [Service] SchoolJournalDbContext db)
        {
            var entity = await db.Attendances.FirstAsync(x => x.Id == id);
            db.Attendances.Remove(entity);
            await db.SaveChangesAsync();
            return true;
        }
    }
}

