using System;
using Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Entities.Models
{
    public class AppUser: IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
