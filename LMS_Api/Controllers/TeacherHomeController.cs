using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Abstract;
using DataAccess.Identity;
using Entities.DTOs;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LMS_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherHomeController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILessonService _lessonService;
        private readonly IGroupService _groupService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IGroupMaxPointService _groupMaxPointService;

        public TeacherHomeController(IMapper mapper, IGroupService groupService,
            ILessonService lessonService, UserManager<AppUser> userManager,
            IGroupMaxPointService groupMaxPointService)
        {
            _mapper = mapper;
            _groupService = groupService;
            _lessonService = lessonService;
            _userManager = userManager;
            _groupMaxPointService = groupMaxPointService;
        }

        [HttpGet]
        [Route("{groupId?}")]
        [Authorize(Roles = nameof(Roles.Teacher))]
        public async Task<ActionResult> GetTeacherHomeContent(int? groupId)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "uid").Value;
            if (userId is null)
                return Unauthorized();

            var groups = await _groupService.GetGroupsByUserIdAsync(userId);

            var group = groupId == null ? groups.First() : groups
                .FirstOrDefault(g => g.Id == (int)groupId);

            var allLessons = await _lessonService
                .GetLessonsByGroupIdAsync(group.Id);

            var lessons = await _lessonService
               .GetLessonsByGroupIdAndUserIdAsync(group.Id, userId, futureDaysCount: 2);

            List<AppUser> students = new();

            foreach (var appUserGroup in group.AppUserGroups)
            {
                var roles = await _userManager.GetRolesAsync(appUserGroup.AppUser);

                var isStudent = roles.All(r => r.ToLower() == nameof(Roles.Student).ToLower());

                students.Add(appUserGroup.AppUser);
            }

            int totalAssignments = 0;

            int totalTheories = 0;

            allLessons.ForEach(l => totalTheories += l.Theories.Count);
            allLessons.ForEach(l => totalAssignments += l.Assignments.Count);

            var groupMaxPointDb = await _groupMaxPointService.GetGroupMaxPointByGroupId(group.Id);
            var lessonsDbCount = await _lessonService.GetLessonsByGroupIdCountAsync(group.Id);

            decimal maxPoint = groupMaxPointDb.MaxPoint;

            var groupsDto = _mapper.Map<List<GroupDTO>>(groups); 
            var studentsDto = _mapper.Map<List<AppUserDTO>>(students);
            var lessonsDto = _mapper.Map<List<LessonDTO>>(lessons);

            TeacherHomeDTO teacherHomeDto = new()
            {
                Groups = groupsDto,
                Students = studentsDto,
                Lessons = lessonsDto,
                CurrentGroupId = group.Id,
                MaxPoint = maxPoint
            };

            HttpContext.Response.Headers.Add("Count", lessonsDbCount.ToString());

            return Ok(teacherHomeDto);
        }
    }
}