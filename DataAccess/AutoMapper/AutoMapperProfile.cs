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
            CreateMap<Group, GroupDTO>().ReverseMap();

            CreateMap<Subject, SubjectDTO>().ReverseMap();

            CreateMap<AppUser, AppUserDTO>().ReverseMap();

            CreateMap<Lesson, LessonDTO>().ReverseMap();

            CreateMap<Question, QuestionDTO>().ReverseMap();

            CreateMap<Notification, NotificationDTO>().ReverseMap();

            CreateMap<AppUserNotification, AppUserNotificationDTO>().ReverseMap();

            CreateMap<Option, OptionDTO>().ReverseMap();

            CreateMap<Quiz, QuizDTO>().ReverseMap();

            CreateMap<AppUserQuiz, AppUserQuizDTO>().ReverseMap();

            CreateMap<Theory, TheoryDTO>().ReverseMap();

            CreateMap<Option, OptionQuizDTO>().ReverseMap();

            CreateMap<Question, QuestionQuizDTO>().ReverseMap();

            CreateMap<LessonJoinLink, LessonJoinLinkDTO>().ReverseMap();

            CreateMap<TheoryAppUser, TheoryAppUserDTO>().ReverseMap();

            CreateMap<QuizMaxPoint, QuizMaxPointDTO>().ReverseMap();

            CreateMap<AppUserOption, AppUserOptionDTO>().ReverseMap();

            CreateMap<AssignmentMaterial, AssignmentMaterialDTO>().ReverseMap();

            CreateMap<Assignment, AssignmentDTO>().ReverseMap();

            CreateMap<Application, ApplicationDTO>().ReverseMap();

            CreateMap<AssignmentAppUser, AssignmentAppUserDto>().ReverseMap();

            CreateMap<AssignmentAppUserMaterial, AssignmentAppUserMaterialDTO>().ReverseMap();
        }
    }
}
