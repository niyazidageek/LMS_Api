using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StudentHomeController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IGroupService _groupService;
        private readonly ILessonService _lessonService;
        private readonly IAppUserGroupPointService _appUserGroupPointService;
        private readonly IGroupMaxPointService _groupMaxPointService;
        private readonly IAppUserGroupService _appUserGroupService;
        private readonly ITheoryAppUserService _theoryAppUserService;
        private readonly IAssignmentAppUserService _assignmentAppUserService;
        private readonly IMapper _mapper;

        public StudentHomeController(UserManager<AppUser> userManager, IGroupService groupService,
            ILessonService lessonService, IAppUserGroupPointService appUserGroupPointService,
            IGroupMaxPointService groupMaxPointService, IAppUserGroupService appUserGroupService,
            ITheoryAppUserService theoryAppUserService, IAssignmentAppUserService assignmentAppUserService,
            IMapper mapper)
        {
            _groupMaxPointService = groupMaxPointService;
            _theoryAppUserService = theoryAppUserService;
            _assignmentAppUserService = assignmentAppUserService;
            _userManager = userManager;
            _groupService = groupService;
            _lessonService = lessonService;
            _appUserGroupPointService = appUserGroupPointService;
            _appUserGroupService = appUserGroupService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("{groupId?}")]
        [Authorize(Roles = nameof(Roles.Student))]
        public async Task<ActionResult> GetStudentHomeContent(int? groupId)
        {

            var userId = User.Claims.FirstOrDefault(c => c.Type == "uid").Value;
            if (userId is null)
                return Unauthorized();

            var groups = await _groupService.GetGroupsByUserIdAsync(userId);

            var group = groupId == null ? groups.First() : groups
                .FirstOrDefault(g=>g.Id == (int)groupId);

            var allLessons = await _lessonService
                .GetLessonsByGroupIdAsync(group.Id);

            var lessons = await _lessonService
                .GetLessonsByGroupIdAndUserIdAsync(group.Id,userId,futureDaysCount:2);

            int totalAssignments = 0;

            int totalTheories = 0;

            allLessons.ForEach(l => totalTheories += l.Theories.Count);
            allLessons.ForEach(l => totalAssignments += l.Assignments.Count);

            AppUser teacher=null;

            List<AppUser> students = new();

            foreach (var appUserGroup in group.AppUserGroups)
            {
                var roles = await _userManager.GetRolesAsync(appUserGroup.AppUser);

                var isTeacher = roles.Any(r => r.ToLower() == nameof(Roles.Teacher));

                if (isTeacher)
                {
                    teacher = appUserGroup.AppUser;
                }
                else
                {
                    students.Add(appUserGroup.AppUser);
                }
            }


            var groupMaxPointDb = await _groupMaxPointService.GetGroupMaxPointByGroupId(group.Id);
            var appUserGroupDb = await _appUserGroupService.GetAppUserGroupByUserIdAndGroupIdAsync(userId, group.Id);
            var appUserPointDb = await _appUserGroupPointService.GetAppUserGroupPointByAppUserGroupIdAsync(appUserGroupDb.Id);
            var theoryAppUsersDb = await _theoryAppUserService.GetTheoryAppUsersByAppUserIdAndGroupId(userId, group.Id);
            var assignmentAppUsersDb = await _assignmentAppUserService.GetAssignmentAppUsersByAppUserIdAndGroupIdAsync(userId, group.Id);
            var lessonsDbCount = await _lessonService.GetLessonsByGroupIdCountAsync(group.Id);

            decimal maxPoint = groupMaxPointDb.MaxPoint;

            decimal currentPoint = appUserPointDb.Point;

            int readTheoriesCount = theoryAppUsersDb.Count;

            int submittedAssignmentsCount = assignmentAppUsersDb.Count;

            int progressPercentage =
                maxPoint == 0 ? 0 : ((int)Math.Ceiling(currentPoint / maxPoint * 100));

            var teacherDto =  _mapper.Map<AppUserDTO>(teacher);

            var lessonsDto = _mapper.Map<List<LessonDTO>>(lessons);

            var groupsDto = _mapper.Map<List<GroupDTO>>(groups);

            StudentHomeDTO studentHomeDto = new()
            {
                Teacher = teacherDto,
                Lessons = lessonsDto,
                Groups = groupsDto,
                CurrentGroupId = group.Id,
                ProgressPercentage = progressPercentage,
                CurrentPoint = currentPoint,
                TotalTheories = totalTheories,
                TotalAssignments = totalAssignments,
                ReadTheoriesCount = readTheoriesCount,
                SubmittedAssignmentsCount = submittedAssignmentsCount

            };

            HttpContext.Response.Headers.Add("Count", lessonsDbCount.ToString());

            return Ok(studentHomeDto);
        }
    }
}