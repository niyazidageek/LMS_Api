using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Business.Abstract;
using DataAccess.Concrete;
using DataAccess.Identity;
using Entities.DTOs;
using Entities.Models;
using LMS_Api.Attributes;
using LMS_Api.Hubs;
using LMS_Api.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Utils;

namespace LMS_Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LessonController : ControllerBase
    {
        private readonly IHubContext<BroadcastHub> _hub;
        private readonly ILessonService _lessonService;
        private readonly IMapper _mapper;
        private readonly IGroupService _groupService;
        private readonly IAppUserGroupService _appUserGroupService;
        private readonly ILessonJoinLinkService _lessonJoinLinkService;

        public LessonController(ILessonService lessonService,
            IMapper mapper,
            IAppUserGroupService appUserGroupService,
            IHubContext<BroadcastHub> hub,
            IGroupService groupService,
            ILessonJoinLinkService lessonJoinLinkService)
        {
            _appUserGroupService = appUserGroupService;
            _hub = hub;
            _lessonService = lessonService;
            _mapper = mapper;
            _groupService = groupService;
            _lessonJoinLinkService = lessonJoinLinkService;
        }

        [HttpPost]
        [Route("{id}")]
        [Authorize(Roles=nameof(Roles.Teacher))]
        public async Task<ActionResult> StartLesson(int id, [FromBody] LessonJoinLinkDTO lessonJoinLinkDto)
        {
            var lessonDb = await _lessonService.GetLessonByIdAsync(id);

            if (lessonDb is null)
                return NotFound();

            if (!lessonDb.IsOnline)
                return Conflict(new ResponseDTO
                {
                    Status = nameof(StatusTypes.LessonJoinLinkError),
                    Message = "Since lesson format is offline, you can't start it"
                });

            var hasStarted = await _lessonJoinLinkService.HasLessonStartedByLessonIdAsync(id);

            if (hasStarted)
                return Conflict(new ResponseDTO
                {
                    Status=nameof(StatusTypes.LessonJoinLinkError),
                    Message="Lesson has already been started or ended!"
                });

            var appUserGroups = await _appUserGroupService.GetAppUserGroupsByGroupIdAsync(lessonDb.GroupId);

            var userIds = appUserGroups.Select(ag => ag.AppUserId).ToList();

            lessonJoinLinkDto.LessonId = id;
            var lessonJoinLinkJson = JsonConvert.SerializeObject(lessonJoinLinkDto,
                new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

            await _hub.Clients.Users(userIds).SendAsync("ReceiveLink", lessonJoinLinkJson);

            await _lessonJoinLinkService.AddLessonJoinLinkAsync(new LessonJoinLink
            {
                LessonId = lessonDb.Id,
                JoinLink = lessonJoinLinkDto.JoinLink
            });

            return Ok(new ResponseDTO
            {
                Status=nameof(StatusTypes.Success),
                Message="Lesson has been started!"
            });
        }

        [HttpGet]
        public async Task<ActionResult> GetLessons()
        {
            var lessonsDb = await _lessonService.GetLessonsAsync();

            if (lessonsDb is null)
                return NotFound();

            var lessonsDto = _mapper.Map<List<LessonDTO>>(lessonsDb);

            return Ok(lessonsDto);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetLessonById(int id)
        {
            var lessonDb = await _lessonService.GetLessonByIdAsync(id);

            if (lessonDb is null)
                return NotFound();

            var lessonDto = _mapper.Map<LessonDTO>(lessonDb);

            return Ok(lessonDto);
        }

        [HttpGet]
        [Route("{id}/{page}/{size}/{futureDaysCount?}")]
        [Authorize(Roles = nameof(Roles.Student))]
        public async Task<ActionResult> GetLessonsByGroupIdAndUserId(int id, int page, int size, int? futureDaysCount)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "uid").Value;

            if (userId is null)
                return Unauthorized();

            if (page < 0)
                return BadRequest(new ResponseDTO
                {
                    Status = nameof(StatusTypes.OffsetError),
                    Message = "Can't load information!"
                });

            var lessonsDb = futureDaysCount is null ?
                await _lessonService.GetLessonsByGroupIdAndUserIdAsync(id, userId, page, size) :
                await _lessonService.GetLessonsByGroupIdAndUserIdAsync(id, userId, page, size, (int)futureDaysCount);

            var lessonsDbCount = await _lessonService.GetLessonsByGroupIdCountAsync(id);

            if (lessonsDb is null)
                return NotFound();

            var lessonsDto = _mapper.Map<List<LessonDTO>>(lessonsDb);

            HttpContext.Response.Headers.Add("Count", lessonsDbCount.ToString());

            return Ok(lessonsDto);
        }

        [HttpGet]
        [Route("{groupId}/{page}/{size}/{futureDaysCount?}")]
        public async Task<ActionResult> GetLessonsByGroupId(int groupId, int page, int size, int? futureDaysCount)
        {
            if (page < 0)
                return BadRequest(new ResponseDTO
                {
                    Status = nameof(StatusTypes.OffsetError),
                    Message = "Can't load information!"
                });

            var lessonsDb = futureDaysCount is null ?
                await _lessonService.GetLessonsByGroupIdAsync(groupId, page, size) :
                await _lessonService.GetLessonsByGroupIdAsync(groupId, page, size, (int)futureDaysCount);

            if (lessonsDb is null)
                return NotFound();

            var lessonsDbCount = await _lessonService.GetLessonsByGroupIdCountAsync(groupId);


            var lessonsDto = _mapper.Map<List<LessonDTO>>(lessonsDb);

            HttpContext.Response.Headers.Add("Count", lessonsDbCount.ToString());

            return Ok(lessonsDto);
        }

        [HttpGet]
        [Route("{groupId}/{page}/{size}")]
        [Roles(nameof(Roles.Teacher), nameof(Roles.Admin), nameof(Roles.SuperAdmin))]
        public async Task<ActionResult> GetLessonsWithSubmissionsByGroupId(int groupId, int page, int size)
        {
            if (page < 0)
                return BadRequest(new ResponseDTO
                {
                    Status = nameof(StatusTypes.OffsetError),
                    Message = "Can't load information!"
                });

            var lessonsDb = await _lessonService.GetLessonsByGroupIdWithSubmissionsAsync(groupId, page, size);

            if (lessonsDb is null)
                return NotFound();

            var lessonsDbCount = await _lessonService.GetLessonsByGroupIdWithSubmissionsCountAsync(groupId);

            var lessonsDto = _mapper.Map<List<LessonDTO>>(lessonsDb);

            HttpContext.Response.Headers.Add("Count", lessonsDbCount.ToString());

            return Ok(lessonsDto);
        }

        [HttpPost]
        [Roles(nameof(Roles.Teacher), nameof(Roles.Admin), nameof(Roles.SuperAdmin))]
        public async Task<ActionResult> CreateLesson([FromBody] LessonDTO lessonDto)
        {

            //if (!ModelState.IsValid) return BadRequest();

            var lessonDb = _mapper.Map<Lesson>(lessonDto);

            await _lessonService.AddLessonAsync(lessonDb);

            return Ok(new ResponseDTO
            {
                Status = nameof(StatusTypes.Success),
                Message="Lesson has been successfully created!"
            });
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = nameof(Roles.Teacher))]
        public async Task<IActionResult> EditLesson(int id, [FromBody] LessonDTO lessonDto)
        {
            //if (!ModelState.IsValid) return BadRequest();

            var lessonDb = await _lessonService.GetLessonByIdAsync(id);

            if (lessonDb is null)
                return NotFound();

            lessonDto.Id = lessonDb.Id;

            _mapper.Map(lessonDto, lessonDb);

            await _lessonService.EditLessonAsync(lessonDb);

            return Ok(new ResponseDTO
            {
                Status=nameof(StatusTypes.Success),
                Message="Lesson has been successfully edited!"
            });

        }

        [HttpGet]
        [Route("{groupId}/{input}")]
        public async Task<IActionResult> SearchLessonsByGroupId(int groupId,string input)
        {
            var groupDb = await _groupService.GetGroupByIdAsync(groupId);

            if (groupDb is null)
                return NotFound();

            input = input.Trim();

            var lessonsDb = await _lessonService.GetLessonsByMatchAndGroupIdAsync(groupId,input);

            var lessonsDto = _mapper.Map<List<LessonDTO>>(lessonsDb);

            return Ok(lessonsDto);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteLesson(int id)
        {
            var lessonDb = await _lessonService.GetLessonByIdAsync(id);

            if (lessonDb is null)
                return NotFound();

            await _lessonService.DeleteLessonAsync(id);

            return Ok(new ResponseDTO
            {
                Status=nameof(StatusTypes.Success),
                Message="Lesson has been successfully deleted!"
            });
        }
    }
}