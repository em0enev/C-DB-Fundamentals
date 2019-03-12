using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace P01_StudentSystem.Data
{
    public class StudentSystemContext : DbContext
    {
        public DbSet<Student> Students { get; set; }

        public DbSet<StudentCourse> StudentCourses { get; set; }

        public DbSet<Resource> Resources { get; set; }

        public DbSet<Homework> HomeworkSubmissions { get; set; }

        public DbSet<Course> Courses { get; set; }

        public StudentSystemContext(DbContextOptions options) : base(options)
        {
        }

        public StudentSystemContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string Config = @"Server=DESKTOP-VNP1D7N\SQLEXPRESS;Database=StudentSystem;Integrated Security = true;";
            base.OnConfiguring(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Config);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureStudentEntity(modelBuilder);

            ConfigureCourseEntity(modelBuilder);

            ConfigureResourceEntity(modelBuilder);

            ConfigureHomeworkEntity(modelBuilder);

            ConfigureStudentCourseEntity(modelBuilder);
        }

        private void ConfigureStudentCourseEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<StudentCourse>()
                .HasKey(table => new
                {
                    table.StudentId,
                    table.CourseId
                });

            modelBuilder
                .Entity<StudentCourse>()
                .HasOne(s => s.Course)
                .WithMany(s => s.StudentsEnrolled)
                .HasForeignKey(s => s.CourseId);

            modelBuilder
                .Entity<StudentCourse>()
                .HasOne(s => s.Student)
                .WithMany(s => s.CourseEnrollments)
                .HasForeignKey(s => s.StudentId);
        }

        private void ConfigureHomeworkEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Homework>()
                .HasKey(h => h.HomeworkId);

            modelBuilder
                .Entity<Homework>()
                .HasOne(h => h.Student)
                .WithMany(h => h.HomeworkSubmissions)
                .HasForeignKey(h => h.StudentId);

            modelBuilder
                .Entity<Homework>()
                .HasOne(h => h.Course)
                .WithMany(h => h.HomeworkSubmissions)
                .HasForeignKey(h => h.CourseId);
        }

        private void ConfigureResourceEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Resource>()
                .HasKey(r => r.ResourceId);

            modelBuilder
                .Entity<Resource>()
                .Property(r => r.Name)
                .HasMaxLength(50)
                .IsUnicode();

            modelBuilder
                .Entity<Resource>()
                .HasOne(r => r.Course)
                .WithMany(c => c.Resources)
                .HasForeignKey(r => r.CourseId);
        }

        private void ConfigureCourseEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Course>()
                .HasKey(c => c.CourseId);

            modelBuilder
                .Entity<Course>()
                .Property(c => c.Name)
                .HasMaxLength(80)
                .IsUnicode()
                .IsRequired();

            modelBuilder
                .Entity<Course>()
                .Property(c => c.Description)
                .IsUnicode();

            modelBuilder
                .Entity<Course>()
                .Property(c => c.StartDate)
                .IsRequired();

            modelBuilder
                .Entity<Course>()
                .Property(c => c.EndDate)
                .IsRequired();

            modelBuilder
                .Entity<Course>()
                .Property(c => c.Price)
                .IsRequired();

        }

        private void ConfigureStudentEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Student>()
                .HasKey(s => s.StudentId);

            modelBuilder
                .Entity<Student>()
                .Property(s => s.Name)
                .HasMaxLength(100)
                .IsUnicode();

            modelBuilder
                .Entity<Student>()
                .Property(s => s.RegisteredOn)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder
                .Entity<Student>()
                .Property(s => s.PhoneNumber)
                .HasMaxLength(10)
                .IsFixedLength();

            modelBuilder
                .Entity<Student>()
                .HasData(new Student
                {
                    StudentId = 1,
                    Name = "pesho",
                    PhoneNumber = "123456789",
                    Birthday = DateTime.Now.AddDays(-50)
                });
        }
    }
}
