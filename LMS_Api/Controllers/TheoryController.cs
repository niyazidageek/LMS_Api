using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Abstract;
using DataAccess.Identity;
using Entities.DTOs;
using Entities.Models;
using LMS_Api.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Utils;

namespace LMS_Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TheoryController : ControllerBase
    {
        private readonly ITheoryService _theoryService;
        private readonly ILessonService _lessonService;
        private readonly IMapper _mapper;
        private readonly IGroupMaxPointService _groupMaxPointService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITheoryAppUserService _theoryAppUserService;
        private readonly IAppUserGroupService _appUserGroupService;
        private readonly IAppUserGroupPointService _appUserGroupPointService;

        public TheoryController(ITheoryService theoryService,
            ILessonService lessonService,
            IAppUserGroupService appUserGroupService,
            IAppUserGroupPointService appUserGroupPointService,
            IMapper mapper,
            IGroupMaxPointService groupMaxPointService,
            ITheoryAppUserService theoryAppUserService,
            UserManager<AppUser> userManager)
        {
            _appUserGroupPointService = appUserGroupPointService;
            _appUserGroupService = appUserGroupService;
            _groupMaxPointService = groupMaxPointService;
            _theoryAppUserService = theoryAppUserService;
            _userManager = userManager;
            _mapper = mapper;
            _theoryService = theoryService;
            _lessonService = lessonService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetTheoriesByLessonId(int id)
        {
            var lessonDb = await _lessonService.GetLessonByIdAsync(id);

            if (lessonDb is null)
                return NotFound();

            var theoriesDb = await _theoryService.GetTheoriesByLessonIdAsync(id);

            var theoriesDto = _mapper.Map<List<TheoryDTO>>(theoriesDb);

            return Ok(theoriesDto);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetTheoryById(int id)
        {
            var theoryDb = await _theoryService.GetTheoryByIdAsync(id);

            if (theoryDb is null)
                return NotFound();

            var theoryDto = _mapper.Map<TheoryDTO>(theoryDb);

            return Ok(theoryDto);
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = nameof(Roles.Student))]
        public async Task<ActionResult> GetStudentsTheoryById(int id)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "uid").Value;

            if (userId is null)
                return Unauthorized();

            var theoryDb = await _theoryService.GetTheoryByIdAndUserId(id,userId);

            if (theoryDb is null)
                return NotFound();

            var theoryDto = _mapper.Map<TheoryDTO>(theoryDb);

            return Ok(theoryDto);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetStudentsTheoriesByLessonId(int id)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "uid").Value;
            if (userId is null)
                return Unauthorized();

            var lessonDb = await _lessonService.GetLessonByIdAsync(id);

            if (lessonDb is null)
                return NotFound();

            var theoriesDb = await _theoryService.GetTheoriesByLessonIdAndUserIdAsync(id, userId);

            var theoriesDto = _mapper.Map<List<TheoryDTO>>(theoriesDb);

            return Ok(theoriesDto);
        }

        [HttpPost]
        public async Task<ActionResult> CreateTheory([FromForm] TheoryAttachmentDTO theoryAttachmentDto)
        {
            var theoryDto = JsonConvert.DeserializeObject<TheoryDTO>(theoryAttachmentDto.Values);

            var theoryDb = _mapper.Map<Theory>(theoryDto);

            var lessonDb = await _lessonService.GetLessonByIdAsync(theoryDto.LessonId);

            if (lessonDb is null)
                return NotFound();

            var fileName = await FileHelper.AddJsonFile(theoryAttachmentDto.Content);

            theoryDb.FileName = fileName;

            await _theoryService.AddTheoryAsync(theoryDb);

            List<AppUserGroup> students = new();

            foreach (var appUserGroup in lessonDb.Group.AppUserGroups)
            {
                var user = await _userManager.FindByIdAsync(appUserGroup.AppUserId);

                var roles = await _userManager.GetRolesAsync(user);

                var isStudent = roles.Any(x => x.ToLower() == nameof(Roles.Student).ToLower());

                if (isStudent is true)
                    students.Add(appUserGroup);
            }

            lessonDb.Group.AppUserGroups = students;

            await _theoryAppUserService.InitializeTheoryAsync(students, theoryDb.Id);

            var groupId = lessonDb.GroupId;

            var groupMaxPoint = await _groupMaxPointService.GetGroupMaxPointByGroupId(groupId);

            groupMaxPoint.MaxPoint += theoryDb.Point;

            await _groupMaxPointService.EditGroupMaxPoint(groupMaxPoint);

            return Ok();
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> EditTheory(int id, [FromForm] TheoryAttachmentDTO theoryAttachmentDto)
        {
            var theoryDb = await _theoryService.GetTheoryByIdAsync(id);

            if (theoryDb is null)
                return NotFound();

            var lessonDb = await _lessonService.GetLessonByIdAsync(theoryDb.LessonId);

            var oldGrade = theoryDb.Point;

            var theoryDto = JsonConvert.DeserializeObject<TheoryDTO>(theoryAttachmentDto.Values);

            FileHelper.DeleteFile(theoryDb.FileName);

            var newFileName = await FileHelper.AddJsonFile(theoryAttachmentDto.Content);

            theoryDto.Id = theoryDb.Id;
            theoryDto.FileName = newFileName;

            _mapper.Map(theoryDto, theoryDb);

            await _theoryService.EditTheoryAsync(theoryDb);

            var groupId = lessonDb.GroupId;

            var groupMaxPoint = await _groupMaxPointService.GetGroupMaxPointByGroupId(groupId);

            groupMaxPoint.MaxPoint = groupMaxPoint.MaxPoint - oldGrade + theoryDb.Point;

            await _groupMaxPointService.EditGroupMaxPoint(groupMaxPoint);

            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteTheory(int id)
        {
            var theoryDb = await _theoryService.GetTheoryByIdAsync(id);

            if (theoryDb is null)
                return NotFound();

            var lessonDb = await _lessonService.GetLessonByIdAsync(theoryDb.LessonId);

            FileHelper.DeleteFile(theoryDb.FileName);

            var currentGrade = theoryDb.Point;

            var theoryAppUsers = await _theoryAppUserService.GetTheoryAppUsersByLessonIdAsync(lessonDb.Id);

            foreach (var theoryAppUser in theoryAppUsers)
            {
                var appUserGroup = await _appUserGroupService
                    .GetAppUserGroupByUserIdAndGroupIdAsync(theoryAppUser.AppUserId, lessonDb.GroupId);

                var appUserGroupPoint = await _appUserGroupPointService
                    .GetAppUserGroupPointByAppUserGroupIdAsync(appUserGroup.Id);

                appUserGroupPoint.Point -= currentGrade;

                await _appUserGroupPointService.EditAppUserGroupPointAsync(appUserGroupPoint);
            }

            await _theoryService.DeleteTheoryAsync(id);

            var groupId = lessonDb.GroupId;

            var groupMaxPoint = await _groupMaxPointService.GetGroupMaxPointByGroupId(groupId);

            groupMaxPoint.MaxPoint -= currentGrade;

            await _groupMaxPointService.EditGroupMaxPoint(groupMaxPoint);

            return Ok();
        }

        [HttpPost]
        [Route("{id}")]
        [Authorize(Roles = nameof(Roles.Student))]
        public async Task<ActionResult> MarkTheoryAsRead(int id)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "uid").Value;
            if (userId is null)
                return Unauthorized();

            var theoryDb = await _theoryService.GetTheoryByIdAsync(id);

            if (theoryDb is null)
                return NotFound();

            var theoryAppUserDb = await _theoryAppUserService
                .GetTheoryAppUserByTheoryIdAndUserIdAsync(id, userId);

            if (theoryAppUserDb.IsRead is true)
                return Conflict(new ResponseDTO
                {
                    Status=nameof(StatusTypes.TheoryError),
                    Message="You have already read the theory!"
                });

            theoryAppUserDb.IsRead = true;

            await _theoryAppUserService.EditTheoryAppUserAsync(theoryAppUserDb);

            var lessonDb = await _lessonService.GetLessonByIdAsync(theoryDb.LessonId);

            var groupId = lessonDb.GroupId;

            var appUserGroup = await _appUserGroupService
                .GetAppUserGroupByUserIdAndGroupIdAsync(theoryAppUserDb.AppUserId, groupId);

            var appUserGroupPointDb = await _appUserGroupPointService
                .GetAppUserGroupPointByAppUserGroupIdAsync(appUserGroup.Id);

            appUserGroupPointDb.Point += theoryDb.Point;

            await _appUserGroupPointService.EditAppUserGroupPointAsync(appUserGroupPointDb);


            return Ok(new ResponseDTO
            {
                Status=nameof(StatusTypes.Success),
                Message="You have read theory successfully!"
            });
        }
    }
}