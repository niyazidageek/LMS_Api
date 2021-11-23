using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Abstract;
using DataAccess.Concrete;
using DataAccess.Identity;
using Entities.DTOs;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Utils;

namespace LMS_Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;
        private readonly IMapper _mapper;
        private readonly ISubjectService _subjectService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IAppUserGroupService _appUserGroupService;
        private readonly IAssignmentService _assignmentService;
        private readonly IAssignmentAppUserService _assignmentAppUserService;
        private readonly ITheoryService _theoryService;
        private readonly ITheoryAppUserService _theoryAppUserService;

        public GroupController(IGroupService groupService, IMapper mapper,
            UserManager<AppUser> userManager, ISubjectService subjectService,
            IAppUserGroupService appUserGroupService, IAssignmentService assignmentService,
            IAssignmentAppUserService assignmentAppUserService, ITheoryService theoryService,
            ITheoryAppUserService theoryAppUserService)
        {
            _theoryService = theoryService;
            _theoryAppUserService = theoryAppUserService;
            _groupService = groupService;
            _mapper = mapper;
            _subjectService = subjectService;
            _userManager = userManager;
            _appUserGroupService = appUserGroupService;
            _assignmentService = assignmentService;
            _assignmentAppUserService = assignmentAppUserService;
        }

        [HttpGet]
        public async Task<ActionResult> GetGroups()
        {
            var groupsDb = await _groupService.GetGroupsAsync();

            if (groupsDb is null)
                return NotFound();

            var groupsDto = _mapper.Map<List<GroupDTO>>(groupsDb);

            for (int i = 0; i < groupsDto.Count; i++)
            {
                groupsDto[i].AppUsersCount = groupsDb[i].AppUserGroups.Count;
            }

            return Ok(groupsDto);
        }

        [HttpGet]
        [Route("{skip}/{take}")]
        public async Task<ActionResult> GetGroupsByCountAsync(int skip, int take)
        {
            var groupsDb = await _groupService.GetGroupsByCountAsync(skip, take);

            if (groupsDb is null)
                return NotFound();

            var groupsDto = _mapper.Map<List<GroupDTO>>(groupsDb);

            for (int i = 0; i < groupsDto.Count; i++)
            {
                groupsDto[i].AppUsersCount = groupsDb[i].AppUserGroups.Count;
            }

            return Ok(groupsDto);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetGroupById(int id)
        {
            var groupDb = await _groupService.GetGroupDetailsByIdAsync(id);

            if (groupDb is null)
                return NotFound();

            var groupDto = _mapper.Map<GroupDTO>(groupDb);

            List<AppUserDTO> appUsers = new();

            foreach (var appUserGroup in groupDb.AppUserGroups)
            {
                AppUserDTO appUserDto = new();
                var user = await _userManager.FindByIdAsync(appUserGroup.AppUser.Id);
                var roles = await _userManager.GetRolesAsync(user);
                _mapper.Map(appUserGroup.AppUser, appUserDto);
                //var isTeacher = roles.Any(role => role.ToLower() == nameof(Roles.Teacher).ToLower());
                appUserDto.Roles = (List<string>)roles;

                appUsers.Add(appUserDto);
            }

            groupDto.AppUsers = appUsers;
            groupDto.AppUsersCount = appUsers.Count; 

            return Ok(groupDto);
        }

        [HttpPost]
        public async Task<ActionResult> CreateGroup([FromBody] GroupDTO groupDto)
        {
            //if (!ModelState.IsValid) return BadRequest();

            var groupDb = _mapper.Map<GroupDTO, Group>(groupDto);

            groupDb.SubjectId = groupDto.SubjectId;

            groupDb.GroupMaxPoint = new GroupMaxPoint { GroupId = groupDb.Id };

            await _groupService.AddGroupAsync(groupDb);

            if (groupDto.AppUserIds is not null)
            {
                List<AppUserGroup> appUserGroups = new();

                foreach (var appUserId in groupDto.AppUserIds)
                {
                    var appUserGroup = new AppUserGroup();
                    appUserGroup.AppUserId = appUserId;
                    appUserGroup.GroupId = groupDb.Id;
                    appUserGroup.AppUserGroupPoint = new AppUserGroupPoint { AppUserGroupId = appUserGroup.Id };

                    appUserGroups.Add(appUserGroup);
                }

                await _appUserGroupService.CreateAppUserGroupsAsync(appUserGroups);
            }

            return Ok(new ResponseDTO
            {
                Status = nameof(StatusTypes.Success),
                Message = "Group has been successfully created!"
            });
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> EditGroup(int id, [FromBody] GroupDTO groupDto)
        {
            //if (!ModelState.IsValid) return BadRequest();

            var groupDb = await _groupService.GetGroupByIdAsync(id);

            if (groupDb is null)
                return NotFound();

            groupDto.Id = groupDb.Id;
            _mapper.Map(groupDto, groupDb);

            await _groupService.EditGroupAsync(groupDb);

            var existingAppUserGroups = groupDb.AppUserGroups;

            var deleteableAppUserGroups = existingAppUserGroups
                    .Where(eag => !groupDto.AppUserIds.Any(ai => ai == eag.AppUserId))
                    .ToList();

            var newAppUserGroupIds = groupDto.AppUserIds
                .Except(existingAppUserGroups.Select(eaug =>eaug.AppUserId))
                .ToList();

            if (deleteableAppUserGroups is not null || deleteableAppUserGroups.Count is not 0)
            {
                List<AppUserGroup> appUserGroups = new();

                foreach (var appUserGroup in deleteableAppUserGroups)
                {
                    appUserGroups.Add(appUserGroup);
                }

                await _appUserGroupService.DeleteAppUserGroupsAsync(appUserGroups);
            }

            if (newAppUserGroupIds is not null)
            {
                List<AppUserGroup> appUserGroupsWithTeacher = new();
                List<AppUserGroup> appUserGroupsWithoutTeacher = new();

                foreach (var appUserId in newAppUserGroupIds)
                {
                    var appUserGroup = new AppUserGroup();
                    appUserGroup.AppUserId = appUserId;
                    appUserGroup.GroupId = groupDb.Id;
                    appUserGroup.AppUserGroupPoint = new AppUserGroupPoint { AppUserGroupId = appUserGroup.Id };

                    var user = await _userManager.FindByIdAsync(appUserId);
                    var roles = await _userManager.GetRolesAsync(user);
                    var isTeacher = roles.Any(role => role.ToLower() == nameof(Roles.Teacher).ToLower());

                    if (isTeacher)
                    {
                        appUserGroupsWithTeacher.Add(appUserGroup);
                    }
                    else
                    {
                        appUserGroupsWithTeacher.Add(appUserGroup);
                        appUserGroupsWithoutTeacher.Add(appUserGroup);
                    }
                    
                }

                await _appUserGroupService.CreateAppUserGroupsAsync(appUserGroupsWithTeacher);

                var assignments = await _assignmentService.GetAssignmentsByGroupIdAsync(groupDb.Id);

                var theories = await _theoryService.GetTheoriesByGroupIdAsync(groupDb.Id);

                await _assignmentAppUserService.ReinitializeAssignmentsAsync(appUserGroupsWithoutTeacher, assignments);

                await _theoryAppUserService.ReinitializeTheoriesAsync(appUserGroupsWithoutTeacher, theories);
            }

            return Ok(new ResponseDTO
            {
                Status=nameof(StatusTypes.Success),
                Message = "Group has been successfully edited!"
            });
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            await _groupService.DeleteGroupAsync(id);

            return Ok();
        }
    }
}