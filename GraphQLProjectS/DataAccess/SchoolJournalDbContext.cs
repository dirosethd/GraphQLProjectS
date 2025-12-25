using System;
using System.Collections.Generic;
using GraphQLProjectS.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace GraphQLProjectS.DataAccess;

public partial class SchoolJournalDbContext : DbContext
{
    public SchoolJournalDbContext()
    {
    }

    public SchoolJournalDbContext(DbContextOptions<SchoolJournalDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Attendance> Attendances { get; set; }

    public virtual DbSet<Grade> Grades { get; set; }

    public virtual DbSet<SchoolClass> SchoolClasses { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    public virtual DbSet<TeachingAssignment> TeachingAssignments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=dirose;Initial Catalog=SchoolJournalDb;User ID=dirosethd;Password=1612;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attendance>(entity =>
        {
            entity.Property(e => e.Reason).HasMaxLength(250);

            entity.HasOne(d => d.Student).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK_Attendances_Students");
        });

        modelBuilder.Entity<Grade>(entity =>
        {
            entity.HasOne(d => d.Subject).WithMany(p => p.Grades)
                .HasForeignKey(d => d.SubjectId)
                .HasConstraintName("FK_Grades_Subjects");
        });

        modelBuilder.Entity<SchoolClass>(entity =>
        {
            entity.HasIndex(e => e.Name, "UX_SchoolClasses_Name").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasIndex(e => e.SchoolClassId, "IX_Students_SchoolClassId");

            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.MiddleName).HasMaxLength(100);
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasIndex(e => e.Name, "UX_Subjects_Name").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(150);
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.MiddleName).HasMaxLength(100);
        });

        modelBuilder.Entity<TeachingAssignment>(entity =>
        {
            entity.HasIndex(e => new { e.TeacherId, e.SubjectId, e.SchoolClassId }, "UX_TA_Unique").IsUnique();

            entity.HasOne(d => d.SchoolClass).WithMany(p => p.TeachingAssignments)
                .HasForeignKey(d => d.SchoolClassId)
                .HasConstraintName("FK_TA_SchoolClasses");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
