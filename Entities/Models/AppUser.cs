using System;
using Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Entities.Models
{
    public class AppUser: IdentityUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
