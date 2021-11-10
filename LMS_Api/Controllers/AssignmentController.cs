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
using Newtonsoft.Json;

namespace LMS_Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AssignmentController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAssignmentService _assignmentService;
        private readonly IAssignmentAppUserService _assignmentAppUserService;
        private readonly IAssignmentMaterialService _assignmentMaterialService;
        private readonly IAssignmentAppUserMaterialService _assignmentAppUserMaterialService;
        private readonly ILessonService _lessonService;
        private readonly UserManager<AppUser> _userManager;

        public AssignmentController(IAssignmentService assignmentService,
            IAssignmentAppUserService assignmentAppUserService,
            IAssignmentMaterialService assignmentMaterialService,
            UserManager<AppUser> userManager,
            IMapper mapper,
            IAssignmentAppUserMaterialService assignmentAppUserMaterialService,
            ILessonService lessonService)
        {
            _userManager = userManager;
            _assignmentAppUserMaterialService = assignmentAppUserMaterialService;
            _lessonService = lessonService;
            _mapper = mapper;
            _assignmentService = assignmentService;
            _assignmentAppUserService = assignmentAppUserService;
            _assignmentMaterialService = assignmentMaterialService;
        }

        [HttpPost]
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

            foreach (var appUserGroup in lessonDb.Group.AppUserGroups)
            {
                var user = await _userManager.FindByIdAsync(appUserGroup.AppUserId);

                var roles = await _userManager.GetRolesAsync(user);

                var isStudent = roles.Any(x => x.ToLower() == nameof(Roles.Student).ToLower());

                if (isStudent is true)
                    students.Add(appUserGroup);
            }

            lessonDb.Group.AppUserGroups = students;

            await _assignmentAppUserService.InitializeAssignmentAsync(lessonDb, assignmentDb.Id);

            return Ok();
        }

        [HttpPut]
        [Route("{id}")]
        //[Authorize(Roles = nameof(Roles.Teacher))]
        public async Task<ActionResult> GradeAssignment(int id, [FromBody] AssignmentAppUserDto assignmentAppUserDto)
        {
            var assignmentAppUserDb = await _assignmentAppUserService.GetAssignmentAppUserByIdAsync(id);

            if (assignmentAppUserDb is null)
                return NotFound();

            assignmentAppUserDb.Grade = assignmentAppUserDto.Grade;

            await _assignmentAppUserService.EditAssignmentAppUserAsync(assignmentAppUserDb);

            return Ok();
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
        public async Task<ActionResult> GetAllAssignmentsByLessonId(int id)
        {
            var lessonDb = await _lessonService.GetLessonByIdAsync(id);

            if (lessonDb is null)
                return NotFound();

            var assignmentsdDb = await _assignmentService.GetAssignmentsByLessonIdAsync(id);

            var assignmentsDto = _mapper.Map<List<AssignmentDTO>>(assignmentsdDb);

            return Ok(assignmentsDto);
        }

        [HttpPost]
        [Route("{id}")]
        [Authorize(Roles = nameof(Roles.Student))]
        public async Task<ActionResult> SubmitAssingment(int id,[FromForm] SubmissionAttachmentDTO submissionAttachmentDto)
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
                return BadRequest();

            appUserAssignmentDb.IsSubmitted = true;
            appUserAssignmentDb.SubmissionDate = DateTime.UtcNow;

            if (appUserAssignmentDb.SubmissionDate > assignmentDb.Deadline)
                appUserAssignmentDb.isLate = true;

            await _assignmentAppUserService.EditAssignmentAppUserAsync(appUserAssignmentDb);

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

            return Ok();
        }

        [HttpGet]
        [Route("{id}")]
        //[Authorize(Roles = nameof(Roles.Teacher))]
        public async Task<ActionResult> GetSubmissionsByLessonId(int id)
        {
            var lessonDb = await _lessonService.GetLessonByIdAsync(id);

            if (lessonDb is null)
                return NotFound();

            var assignmentAppUsersDb = await _assignmentAppUserService.GetAssignmentAppUsersByLessonIdAsync(lessonDb.Id);

            var assignmentAppUsersDto = _mapper.Map<List<AssignmentAppUserDto>>(assignmentAppUsersDb);

            return Ok(assignmentAppUsersDto);
        }

        [HttpGet]
        [Route("{id}")]
        //[Authorize(Roles = nameof(Roles.Teacher))]
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

            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteAssignment(int id)
        {
            var assignmentDb = await _assignmentService.GetAssignmentByIdAsync(id);

            if (assignmentDb is null)
                return NotFound();

            List<AssignmentMaterial> assignmentMaterials = new();

            foreach (var assignmentMaterial in assignmentDb.AssignmentMaterials)
            {
                assignmentMaterials.Add(assignmentMaterial);
            }

            await _assignmentMaterialService.DeleteAssignmentMaterialsAsync(assignmentMaterials);

            await _assignmentService.DeleteAssignmentAsync(id);

            return Ok();
        }

    }
}