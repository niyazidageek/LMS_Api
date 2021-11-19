﻿using System;
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
            CreateMap<Group, GroupDTO>().ReverseMap();

            CreateMap<Subject, SubjectDTO>().ReverseMap();

            CreateMap<AppUser, AppUserDTO>().ReverseMap();

            CreateMap<Lesson, LessonDTO>().ReverseMap();

            CreateMap<Question, QuestionDTO>().ReverseMap();

            CreateMap<Option, OptionDTO>().ReverseMap();

            CreateMap<Quiz, QuizDTO>().ReverseMap();

            CreateMap<Theory, TheoryDTO>().ReverseMap();

            CreateMap<LessonJoinLink, LessonJoinLinkDTO>();

            CreateMap<TheoryAppUser, TheoryAppUserDTO>();

            CreateMap<AssignmentMaterial, AssignmentMaterialDTO>().ReverseMap();

            CreateMap<Assignment, AssignmentDTO>().ReverseMap();

            CreateMap<AssignmentAppUser, AssignmentAppUserDto>().ReverseMap();

            CreateMap<AssignmentAppUserMaterial, AssignmentAppUserMaterialDTO>().ReverseMap();
        }
    }
}
