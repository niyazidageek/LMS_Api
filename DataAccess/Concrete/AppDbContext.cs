using System;
using Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Concrete
{
    public class AppDbContext:IdentityDbContext<AppUser>
    {

        public AppDbContext()
        {

        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(@"Server=localhost;Database=LmsDb;User Id = sa;Password=MyPass@word;Initial Catalog = LmsDb;");

        }

        public DbSet<Subject> Subjects { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<AppUserGroup> AppUserGroups { get; set; }

        public DbSet<Lesson> Lessons { get; set; }

        public DbSet<AssignmentMaterial> AssignmentMaterials { get; set; }

        public DbSet<AssignmentAppUser> AssignmentAppUsers { get; set; }

        public DbSet<TheoryAppUser> TheoryAppUsers { get; set; }

        public DbSet<AssignmentAppUserMaterial> AssignmentAppUserMaterials { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<Assignment> Assignments { get; set; }

        public DbSet<Theory> Theories { get; set; }

        public DbSet<Quiz> Quizzes { get; set; }

        public DbSet<Option> Options { get; set; }

        public DbSet<AppUserQuiz> AppUserQuizzes { get; set; }

        public DbSet<GroupMaxPoint> GroupMaxPoints { get; set; }

        public DbSet<AppUserGroupPoint> AppUserGroupPoints { get; set; }

        public DbSet<GroupSubmission> GroupSubmissions { get; set; }

        public DbSet<LessonJoinLink> LessonJoinLinks { get; set; }
    }
}
