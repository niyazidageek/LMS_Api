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
    public class AssignmentController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IGroupService _groupService;
        private readonly IGroupSubmissionService _groupSubmissionService;
        private readonly IGroupMaxPointService _groupMaxPointService;
        private readonly IAssignmentService _assignmentService;
        private readonly IAssignmentAppUserService _assignmentAppUserService;
        private readonly IAssignmentMaterialService _assignmentMaterialService;
        private readonly IAssignmentAppUserMaterialService _assignmentAppUserMaterialService;
        private readonly ILessonService _lessonService;
        private readonly IAppUserGroupPointService _appUserGroupPointService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IAppUserGroupService _appUserGroupService;

        public AssignmentController(IAssignmentService assignmentService,
            IAssignmentAppUserService assignmentAppUserService,
            IGroupSubmissionService groupSubmissionService,
            IAssignmentMaterialService assignmentMaterialService,
            UserManager<AppUser> userManager,
            IMapper mapper,
            IGroupService groupService,
            IAppUserGroupPointService appUserGroupPointService,
            IGroupMaxPointService groupMaxPointService,
            IAssignmentAppUserMaterialService assignmentAppUserMaterialService,
            ILessonService lessonService,
            IAppUserGroupService appUserGroupService)
        {
            _groupService = groupService;
            _groupSubmissionService = groupSubmissionService;
            _appUserGroupService = appUserGroupService;
            _appUserGroupPointService = appUserGroupPointService;
            _groupMaxPointService = groupMaxPointService;
            _userManager = userManager;
            _assignmentAppUserMaterialService = assignmentAppUserMaterialService;
            _lessonService = lessonService;
            _mapper = mapper;
            _assignmentService = assignmentService;
            _assignmentAppUserService = assignmentAppUserService;
            _assignmentMaterialService = assignmentMaterialService;
        }

        [HttpPost]
        [Authorize(Roles = nameof(Roles.Teacher))]
        public async Task<ActionResult> CreateAssignment([FromForm] AssignmentAttachmentDTO assignmentAttachmentDto)
        {
            AssignmentDTO assignmentDto = JsonConvert.DeserializeObject<AssignmentDTO>(assignmentAttachmentDto.Values);

            var assignmentDb = _mapper.Map<Assignment>(assignmentDto);

            await _assignmentService.AddAssignmentAsync(assignmentDb);

            if (assignmentAttachmentDto.Materials is not null)
            {
                List<AssignmentMaterial> assignmentMaterials = new();

                foreach (var file in assignmentAttachmentDto.Materials)
                {
                    AssignmentMaterial assignmentMaterial = new();
                    assignmentMaterial.AssignmentId = assignmentDb.Id;
                    assignmentMaterial.File = file;
                    assignmentMaterials.Add(assignmentMaterial);
                }

                await _assignmentMaterialService.CreateAssignmentMaterialsAsync(assignmentMaterials);
            }

            var lessonDb = await _lessonService.GetLessonByIdAsync(assignmentDb.LessonId);

            if (lessonDb is null)
                return NotFound();

            List<AppUserGroup> students = new();

            List<string> receivers = new();

            foreach (var appUserGroup in lessonDb.Group.AppUserGroups)
            {
                var user = await _userManager.FindByIdAsync(appUserGroup.AppUserId);

                var roles = await _userManager.GetRolesAsync(user);

                var isStudent = roles.Any(x => x.ToLower() == nameof(Roles.Student).ToLower());

                if (isStudent is true)
                {
                    students.Add(appUserGroup);

                    if (user.IsSubscribedToSender is true)
                        receivers.Add(user.Email);
                }
            }

            lessonDb.Group.AppUserGroups = students;

            await _assignmentAppUserService.InitializeAssignmentAsync(students, assignmentDb.Id);

            if (assignmentDto.NotifyAll)
            {
                var isSent = EmailHelper.SendMailToManyUsers(receivers, "New homework is available!");

                if (isSent is false)
                    return BadRequest(new ResponseDTO
                    {
                        Status = nameof(StatusTypes.EmailError),
                        Message = "Email can't be sent!"
                    });
            }

            var groupId = lessonDb.GroupId;

            var groupMaxPoint = await _groupMaxPointService.GetGroupMaxPointByGroupId(groupId);

            groupMaxPoint.MaxPoint += assignmentDb.MaxGrade;

            await _groupMaxPointService.EditGroupMaxPoint(groupMaxPoint);

            return Ok(new ResponseDTO
            {
                Status = nameof(StatusTypes.Success),
                Message = "Assignment has been successfully created!"
            });
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = nameof(Roles.Teacher))]
        public async Task<ActionResult> GradeAssignment(int id, [FromBody] AssignmentAppUserDto assignmentAppUserDto)
        {
            var assignmentAppUserDb = await _assignmentAppUserService.GetAssignmentAppUserByIdAsync(id);

            if (assignmentAppUserDb is null)
                return NotFound();

            var oldGrade = assignmentAppUserDb.Grade;

            assignmentAppUserDb.Grade = assignmentAppUserDto.Grade;

            if (!assignmentAppUserDb.Graded)
            {
                assignmentAppUserDb.Graded = true;
            }

            await _assignmentAppUserService.EditAssignmentAppUserAsync(assignmentAppUserDb);

            var lessonDb = await _lessonService.GetLessonByIdAsync(assignmentAppUserDb.Assignment.LessonId);

            var groupId = lessonDb.GroupId;

            var appUserGroup = await _appUserGroupService
                .GetAppUserGroupByUserIdAndGroupIdAsync(assignmentAppUserDb.AppUserId, groupId);

            var appUserGroupPointDb = await _appUserGroupPointService
                .GetAppUserGroupPointByAppUserGroupIdAsync(appUserGroup.Id);

            appUserGroupPointDb.Point -= oldGrade;

            appUserGroupPointDb.Point += assignmentAppUserDto.Grade;

            await _appUserGroupPointService.EditAppUserGroupPointAsync(appUserGroupPointDb);

            return Ok(new ResponseDTO
            {
                Status=nameof(StatusTypes.Success),
                Message="Assignment has been succesfully graded!"
            });
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles=nameof(Roles.Student))]
        public async Task<ActionResult> GetUndoneAssignmentsByLessonId(int id)
        {
            var lessonDb = await _lessonService.GetLessonByIdAsync(id);

            if (lessonDb is null)
                return NotFound();

            var userId = User.Claims.FirstOrDefault(c => c.Type == "uid").Value;
            if (userId is null)
                return Unauthorized();

            var assignmentsdDb = await _assignmentService.GetAssignmentsByLessonIdAndUserIdAsync(id, userId);

            var assignmentsDto = _mapper.Map<List<AssignmentDTO>>(assignmentsdDb);

            return Ok(assignmentsDto);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetAssignmentById(int id)
        {
            var assignmentDb = await _assignmentService.GetAssignmentByIdAsync(id);

            if (assignmentDb is null)
                return NotFound();

            var assignmentDto = _mapper.Map<AssignmentDTO>(assignmentDb);

            return Ok(assignmentDto);
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = nameof(Roles.Student))]
        public async Task<ActionResult> GetStudentsAssignmentById(int id)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "uid").Value;
            if (userId is null)
                return Unauthorized();

            var assignmentDb = await _assignmentService.GetAssignmentByIdAndUserIdAsync(id,userId);

            if (assignmentDb is null)
                return NotFound();

            var assignmentDto = _mapper.Map<AssignmentDTO>(assignmentDb);

            return Ok(assignmentDto);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetAllAssignmentsByLessonId(int id)
        {
            var lessonDb = await _lessonService.GetLessonByIdAsync(id);

            if (lessonDb is null)
                return NotFound();

            var assignmentsdDb = await _assignmentService.GetAssignmentsByLessonIdAsync(id);

            var assignmentsDto = _mapper.Map<List<AssignmentDTO>>(assignmentsdDb);

            return Ok(assignmentsDto);
        }

        [HttpGet]
        [Route("{groupId}/{page:int?}/{size:int?}")]
        public async Task<ActionResult> GetAllAssignemntsByGroupId(int groupId, int? page, int? size)
        {
            var groupDb = await _groupService.GetGroupByIdAsync(groupId);

            if (groupDb is null)
                return NotFound();

            var assignmentsdDb = page is not null && size is not null
                ? await _assignmentService.GetAssignmentsByGroupIdAsync(groupId, (int)page, (int)size)
                : await _assignmentService.GetAssignmentsByGroupIdAsync(groupId);

            var assignmentDbCount = await _assignmentService.GetAssignmentsByGroupIdCountAsync(groupId);

            var assignmentsDto = _mapper.Map<List<AssignmentDTO>>(assignmentsdDb);

            HttpContext.Response.Headers.Add("Count", assignmentDbCount.ToString());

            return Ok(assignmentsDto);
        }

        [HttpPost]
        [Route("{id}")]
        [Authorize(Roles = nameof(Roles.Student))]
        public async Task<ActionResult> SubmitAssignment(int id,[FromForm] SubmissionAttachmentDTO submissionAttachmentDto)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "uid").Value;
            if (userId is null)
                return Unauthorized();

            var assignmentDb = await _assignmentService.GetAssignmentByIdAsync(id);

            if (assignmentDb is null)
                return NotFound();

            var appUserAssignmentDb = await
                _assignmentAppUserService.GetAssignmentAppUserByAssignmentIdAndUserIdAsync(id, userId);

            if (appUserAssignmentDb is null)
                return NotFound();

            if (appUserAssignmentDb.IsSubmitted is true)
                return Conflict(new ResponseDTO
                {
                    Status=nameof(StatusTypes.AssignmentError),
                    Message="You have already submitted your assignment!"
                });

            appUserAssignmentDb.IsSubmitted = true;
            appUserAssignmentDb.SubmissionDate = DateTime.UtcNow;

            if (appUserAssignmentDb.SubmissionDate > assignmentDb.Deadline)
                appUserAssignmentDb.isLate = true;

            await _assignmentAppUserService.EditAssignmentAppUserAsync(appUserAssignmentDb);

            GroupSubmission groupSubmission = new();
            groupSubmission.GroupId = assignmentDb.Lesson.GroupId;
            groupSubmission.Date = DateTime.UtcNow;

            await _groupSubmissionService.AddGroupSubmissionAsync(groupSubmission);

            if(submissionAttachmentDto.Files is not null)
            {
                List<AssignmentAppUserMaterial> assignmentAppUserMaterials = new();

                foreach (var file in submissionAttachmentDto.Files)
                {
                    AssignmentAppUserMaterial assignmentAppUserMaterial = new();
                    assignmentAppUserMaterial.AssignmentAppUserId = appUserAssignmentDb.Id;
                    assignmentAppUserMaterial.File = file;
                    assignmentAppUserMaterials.Add(assignmentAppUserMaterial);
                }

                await _assignmentAppUserMaterialService.CreateAssignmentAppUserMaterialAsync(assignmentAppUserMaterials);
            }

            return Ok(new ResponseDTO
            {
                Status=nameof(StatusTypes.Success),
                Message="Submitted successfully!"
            });
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = nameof(Roles.Teacher))]
        public async Task<ActionResult> GetSubmissionsByLessonId(int id)
        {
            var lessonDb = await _lessonService.GetLessonByIdAsync(id);

            if (lessonDb is null)
                return NotFound();

            var assignmentAppUsersDb = await _assignmentAppUserService.GetAssignmentAppUsersByLessonIdAsync(lessonDb.Id);

            var assignmentAppUsersDto = _mapper.Map<List<AssignmentAppUserDto>>(assignmentAppUsersDb);

            var assignmentAppUsersDbCount = await _assignmentAppUserService
                .GetAssignmentAppUsersByLessonIdCountAsync(lessonDb.Id);

            HttpContext.Response.Headers.Add("Count", assignmentAppUsersDbCount.ToString());

            return Ok(assignmentAppUsersDto);
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = nameof(Roles.Teacher))]
        public async Task<ActionResult> GetSubmissionById(int id)
        {
            var assignmentAppUserDb = await _assignmentAppUserService.GetAssignmentAppUserByIdAsync(id);

            if (assignmentAppUserDb is null)
                return NotFound();

            var assignmentAppUserDto = _mapper.Map<AssignmentAppUserDto>(assignmentAppUserDb);

            return Ok(assignmentAppUserDto);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> EditAssignment(int id, [FromForm] AssignmentAttachmentDTO assignmentAttachmentDto)
        {
            var assignmentDb = await _assignmentService.GetAssignmentByIdAsync(id);

            if (assignmentDb is null)
                return NotFound();

            var lessonDb = await _lessonService.GetLessonByIdAsync(assignmentDb.LessonId);

            var oldGrade = assignmentDb.MaxGrade;

            AssignmentDTO assignmentDto = JsonConvert.DeserializeObject<AssignmentDTO>(assignmentAttachmentDto.Values);

            var existingAppUserMaterials = assignmentDb.AssignmentMaterials.Select(am => am.FileName).ToList();

            var deleteableAppUserMaterials = existingAppUserMaterials
                    .Where(eam => !assignmentDto.AssignmentMaterials.Any(am => am.FileName == eam))
                    .ToList();

            if (deleteableAppUserMaterials is not null || deleteableAppUserMaterials.Count is not 0)
            {
                List<AssignmentMaterial> assignmentMaterials = new();

                foreach (var file in deleteableAppUserMaterials)
                {
                    AssignmentMaterial assignmentMaterial = new();
                    assignmentMaterial.FileName = file;
                    assignmentMaterial.AssignmentId = assignmentDb.Id;
                    assignmentMaterials.Add(assignmentMaterial);
                }

                await _assignmentMaterialService.DeleteAssignmentMaterialsAsync(assignmentMaterials);
            }

            if (assignmentAttachmentDto.Materials is not null)
            {
                List<AssignmentMaterial> assignmentMaterials = new();

                foreach (var file in assignmentAttachmentDto.Materials)
                {
                    AssignmentMaterial assignmentMaterial = new();
                    assignmentMaterial.AssignmentId = assignmentDb.Id;
                    assignmentMaterial.File = file;
                    assignmentMaterials.Add(assignmentMaterial);
                }

                await _assignmentMaterialService.CreateAssignmentMaterialsAsync(assignmentMaterials);
            }

            assignmentDto.Id = assignmentDb.Id;

            foreach (var assignmentMaterialDto in assignmentDto.AssignmentMaterials)
            {
                assignmentMaterialDto.Id = assignmentDb.AssignmentMaterials
                    .FirstOrDefault(am => am.FileName == assignmentMaterialDto.FileName).Id;
            }

            _mapper.Map(assignmentDto, assignmentDb);

            await _assignmentService.EditAssignmentAsync(assignmentDb);

            var groupId = lessonDb.GroupId;

            var groupMaxPoint = await _groupMaxPointService.GetGroupMaxPointByGroupId(groupId);

            groupMaxPoint.MaxPoint = groupMaxPoint.MaxPoint - oldGrade + assignmentDb.MaxGrade;

            await _groupMaxPointService.EditGroupMaxPoint(groupMaxPoint);

            return Ok(new ResponseDTO
            {
                Status=nameof(StatusTypes.Success),
                Message="Assingment has been successfully edited!"
            });
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteAssignment(int id)
        {
            var assignmentDb = await _assignmentService.GetAssignmentByIdAsync(id);

            var lessonDb = await _lessonService.GetLessonByIdAsync(assignmentDb.LessonId);

            if (assignmentDb is null)
                return NotFound();

            List<AssignmentMaterial> assignmentMaterials = new();

            foreach (var assignmentMaterial in assignmentDb.AssignmentMaterials)
            {
                assignmentMaterials.Add(assignmentMaterial);
            }

            await _assignmentMaterialService.DeleteAssignmentMaterialsAsync(assignmentMaterials);

            var currentGrade = assignmentDb.MaxGrade;

            var assignmentAppUsers = await _assignmentAppUserService.GetAssignmentAppUsersByLessonIdAsync(lessonDb.Id);

            foreach (var assignmentAppUser in assignmentAppUsers)
            {
                var appUserGroup = await _appUserGroupService
                    .GetAppUserGroupByUserIdAndGroupIdAsync(assignmentAppUser.AppUserId, lessonDb.GroupId);

                var appUserGroupPoint = await _appUserGroupPointService
                    .GetAppUserGroupPointByAppUserGroupIdAsync(appUserGroup.Id);

                appUserGroupPoint.Point -= assignmentAppUser.Grade;

                await _appUserGroupPointService.EditAppUserGroupPointAsync(appUserGroupPoint);
            }

            await _assignmentService.DeleteAssignmentAsync(id);

            var groupId = lessonDb.GroupId;

            var groupMaxPoint = await _groupMaxPointService.GetGroupMaxPointByGroupId(groupId);

            groupMaxPoint.MaxPoint -= currentGrade;

            await _groupMaxPointService.EditGroupMaxPoint(groupMaxPoint);

            return Ok();
        }

    }
}