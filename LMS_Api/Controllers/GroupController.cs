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

namespace LMS_Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public GroupController(IGroupService groupService, IMapper mapper,
            UserManager<AppUser> userManager, AppDbContext context)
        {
            _groupService = groupService;
            _mapper = mapper;
            _userManager = userManager;
            _context = context;
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
                var isTeacher = roles.Any(role => role.ToLower() == nameof(Roles.Teacher).ToLower());
                appUserDto.Roles = (List<string>)roles;

                if (isTeacher)
                {
                    groupDto.Teacher = appUserDto;
                    continue;
                }
                else
                {
                    appUsers.Add(appUserDto);
                }
            }

            groupDto.AppUsers = appUsers;
            groupDto.AppUsersCount = appUsers.Count; 

            return Ok(groupDto);
        }

        [HttpPost]
        public async Task<ActionResult> CreateGroup([FromBody] GroupDTO groupDto)
        {
            if (!ModelState.IsValid) return BadRequest();

           
            var groupDb = _mapper.Map<GroupDTO, Group>(groupDto);

            var subjectDb = await _context.Subjects.FirstOrDefaultAsync(s => s.Id == groupDto.Subject.Id);

            groupDb.Subject = subjectDb;

            if (subjectDb is null)
                return NotFound();

            if(groupDto.AppUsers is not null)
            {
                List<AppUserGroup> AppUserGroups = new();

                foreach (var appUserDto in groupDto.AppUsers)
                {
                    var appUserGroup = new AppUserGroup();
                    appUserGroup.AppUserId = appUserDto.Id;
                    appUserGroup.GroupId = groupDb.Id;

                    AppUserGroups.Add(appUserGroup);
                }

                groupDb.AppUserGroups = AppUserGroups;
            }

            await _groupService.AddGroupAsync(groupDb);

            return Ok();
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> EditGroup(int id, [FromBody] GroupDTO groupDto)
        {
            if (!ModelState.IsValid) return BadRequest();

            var groupDb = await _groupService.GetGroupByIdAsync(id);

            if (groupDb is null)
                return NotFound();

            var subjectDb = await _context.Subjects.FirstOrDefaultAsync(s => s.Id == groupDto.Subject.Id);

            if (subjectDb is null)
                return NotFound();

            groupDto.Id = groupDb.Id;
            groupDb.Subject = subjectDb;

            _mapper.Map(groupDto, groupDb);

            if(groupDto.AppUsers is not null)
            {
                List<AppUserGroup> AppUserGroups = new();

                foreach (var appUserDto in groupDto.AppUsers)
                {
                    var appUserGroup = new AppUserGroup();
                    appUserGroup.AppUserId = appUserDto.Id;
                    appUserGroup.GroupId = groupDb.Id;

                    AppUserGroups.Add(appUserGroup);
                }

                groupDb.AppUserGroups = AppUserGroups;
            }

            await _groupService.EditGroupAsync(groupDb);

            return Ok();
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