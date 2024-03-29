﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.DTOs
{
    public class AppUserDTO
    {

        //[Required]
        public string Id { get; set; }

        //[Required, StringLength(255)]
        public string Name { get; set; }

        //[Required, StringLength(255)]
        public string Surname { get; set; }

        //[Required, StringLength(255)]
        public string Username { get; set; }

        public string Filename { get; set; }

        //[Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string Bio { get; set; }

        public bool IsSubscribedToSender { get; set; } = true;

        public List<GroupDTO> Groups { get; set; }

        public List<string> Roles { get; set; }
    }
}
