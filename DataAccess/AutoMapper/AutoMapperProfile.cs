using System;
using AutoMapper;
using Entities.DTOs;
using Entities.Models;

namespace DataAccess.AutoMapper
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Group, GroupDTO>().ReverseMap();

            CreateMap<Subject, SubjectDTO>().ReverseMap();
        }
    }
}
