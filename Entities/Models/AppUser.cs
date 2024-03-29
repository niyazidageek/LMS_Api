﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Entities.Models
{
    public class AppUser: IdentityUser
    {

        [Required, StringLength(255)]
        public string Name { get; set; }

        [Required, StringLength(255)]
        public string Surname { get; set; }

        public string Filename { get; set; }

        public string Bio { get; set; }

        public bool IsSubscribedToSender { get; set; } = true;

        public List<AppUserGroup> AppUserGroups { get; set; }

        public List<AppUserQuiz> AppUserQuizzes { get; set; }

        public List<AppUserNotification> AppUserNotifications { get; set; }

        public List<Application> Applications { get; set; }
    }
}
