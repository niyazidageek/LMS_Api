using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Entities.DTOs;
using Entities.Models;

namespace DataAccess.AutoMapper
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {

            CreateMap<GroupDTO, Group>().ReverseMap();

            CreateMap<Subject, SubjectDTO>().ReverseMap();

            CreateMap<AppUser, AppUserDTO>().ReverseMap();

            CreateMap<Lesson, LessonDTO>().ReverseMap();
        }
    }
}
