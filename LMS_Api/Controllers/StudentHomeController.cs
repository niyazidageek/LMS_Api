using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Business.Abstract;
using DataAccess.Identity;
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

        public StudentHomeController(UserManager<AppUser> userManager, IGroupService groupService,
            ILessonService lessonService)
        {
            _userManager = userManager;
            _groupService = groupService;
            _lessonService = lessonService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetStudentHomeContent()
        {

            var userId = User.Claims.FirstOrDefault(c => c.Type == "uid").Value;
            if (userId is null)
                return Unauthorized();

            var groups = await _groupService.GetGroupsByUserIdAsync(userId);

            var lessons = await _lessonService.GetLessonsByGroupIdAsync(groups.First().Id);

            int totalAssignments = 0;

            int totalMaterials = 0;

            //lessons.ForEach(l => totalMaterials += l.LessonMaterials.Count);
            lessons.ForEach(l => totalAssignments += l.Assignments.Count);

            AppUser teacher = new();

            foreach (var appUserGroup in groups.First().AppUserGroups)
            {
                var roles = await _userManager.GetRolesAsync(appUserGroup.AppUser);
                
                var isTeacher = roles.Any(r => r.ToLower() == nameof(Roles.Teacher));
                if (isTeacher)
                {
                    teacher = appUserGroup.AppUser;
                }
            }

            

        

            
            return Ok();
        }
    }
}