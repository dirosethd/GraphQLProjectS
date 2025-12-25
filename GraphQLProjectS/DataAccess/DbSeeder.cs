using GraphQLProjectS.DataAccess.Entities;

namespace GraphQLProjectS.DataAccess
{
    public static class DbSeeder
    {
        public static void Seed(SchoolJournalDbContext db)
        {
            if (db.Students.Any())
                return;

            var class1 = new SchoolClass { Name = "7A" };
            var class2 = new SchoolClass { Name = "7B" };
            db.SchoolClasses.AddRange(class1, class2);
            db.SaveChanges();

            var math = new Subject { Name = "Математика" };
            var physics = new Subject { Name = "Физика" };
            db.Subjects.AddRange(math, physics);
            db.SaveChanges();

            var teacher1 = new Teacher
            {
                LastName = "Иванов",
                FirstName = "Пётр",
                MiddleName = "Сергеевич",
                BirthDate = new DateOnly(1980, 5, 10)
            };

            var teacher2 = new Teacher
            {
                LastName = "Сидорова",
                FirstName = "Анна",
                MiddleName = "Игоревна",
                BirthDate = new DateOnly(1985, 8, 20)
            };

            db.Teachers.AddRange(teacher1, teacher2);
            db.SaveChanges();

            var student1 = new Student
            {
                LastName = "Петров",
                FirstName = "Илья",
                BirthDate = new DateOnly(2011, 3, 15),
                SchoolClassId = class1.Id
            };

            var student2 = new Student
            {
                LastName = "Смирнова",
                FirstName = "Мария",
                BirthDate = new DateOnly(2011, 6, 25),
                SchoolClassId = class1.Id
            };

            db.Students.AddRange(student1, student2);
            db.SaveChanges();

            db.TeachingAssignments.AddRange(
                new TeachingAssignment
                {
                    TeacherId = teacher1.Id,
                    SubjectId = math.Id,
                    SchoolClassId = class1.Id
                },
                new TeachingAssignment
                {
                    TeacherId = teacher2.Id,
                    SubjectId = physics.Id,
                    SchoolClassId = class1.Id
                }
            );
            db.SaveChanges();

            db.Grades.AddRange(
                new Grade
                {
                    StudentId = student1.Id,
                    SubjectId = math.Id,
                    Date = new DateOnly(2024, 9, 10),
                    Value = 5
                },
                new Grade
                {
                    StudentId = student2.Id,
                    SubjectId = math.Id,
                    Date = new DateOnly(2024, 9, 10),
                    Value = 4
                }
            );
            db.SaveChanges();

            db.Attendances.Add(
                new Attendance
                {
                    StudentId = student1.Id,
                    Date = new DateOnly(2024, 9, 5),
                    IsExcused = false
                }
            );
            db.SaveChanges();
        }
    }
}
