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
    }
}
