using System;
using DataAccess.Concrete;
using DataAccess.Identity;
using Entities.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Business.Concrete
{
    public class DatabaseManagementService
    {
        public static void MigrationInitialization(IApplicationBuilder app)
        {
            using(var serviceScope = app.ApplicationServices.CreateScope())
            {
                serviceScope.ServiceProvider.GetService<AppDbContext>().Database.Migrate();
            }
        }

        public static async void RoleInitialization(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                var dataInitializer = new DataInitializer(userManager, roleManager);

                await dataInitializer.SeedAsync();
            }
        }
    }
}
