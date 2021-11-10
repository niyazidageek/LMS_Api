using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Abstract;
using Entities.DTOs;
using Entities.Models;
using Microsoft.AspNetCore.Http;
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

        public AssignmentController(IAssignmentService assignmentService,
            IAssignmentAppUserService assignmentAppUserService,
            IAssignmentMaterialService assignmentMaterialService,
            IMapper mapper,
            IAssignmentAppUserMaterialService assignmentAppUserMaterialService,
            ILessonService lessonService)
        {
            _assignmentAppUserMaterialService = assignmentAppUserMaterialService;
            _lessonService = lessonService;
            _mapper = mapper;
            _assignmentService = assignmentService;
            _assignmentAppUserService = assignmentAppUserService;
            _assignmentMaterialService = assignmentMaterialService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateAssingmentAsync([FromForm] AssignmentAttachmentDTO assignmentAttachmentDto)
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

            await _assignmentAppUserService.InitializeAssignmentAsync(lessonDb, assignmentDb.Id);

            return Ok();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetAssignmentsByLessonId(int id)
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

        [HttpPost]
        [Route("{id}")]
        public async Task<ActionResult> SubmitAssingment(int id,[FromForm] SubmissionAttachmentDTO submissionAttachmentDto)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "uid").Value;
            if (userId is null)
                return Unauthorized();

            var assignmentDb = _assignmentService.GetAssignmentByIdAsync(id);

            if (assignmentDb is null)
                return NotFound();

            var appUserAssignmentDb = await _assignmentAppUserService.GetAssignmentAppUserByIdAsync(id);

            if (appUserAssignmentDb is null)
                return NotFound();

            appUserAssignmentDb.IsSubmitted = true;
            appUserAssignmentDb.SubmissionDate = DateTime.UtcNow;

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
        public async Task<ActionResult> GetSubmissionsByLessonId(int id)
        {
            var lessonDb = await _lessonService.GetLessonByIdAsync(id);

            if (lessonDb is null)
                return NotFound();

            var assignmentAppUsersDb = await _assignmentAppUserService.GetAssignmentAppUsersByLessonIdAsync(lessonDb.Id);

            var assignmentAppUserDto = _mapper.Map<List<AssignmentAppUserDto>>(assignmentAppUsersDb);

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
    }
}