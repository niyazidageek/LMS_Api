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
        private readonly ILessonService _lessonService;

        public AssignmentController(IAssignmentService assignmentService,
            IAssignmentAppUserService assignmentAppUserService,
            IAssignmentMaterialService assignmentMaterialService,
            IMapper mapper,
            ILessonService lessonService)
        {
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

            var assignmentsdDb = await _assignmentAppUserService.GetAssignmentsByLessonIdAndUserIdAsync(id, userId);

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

                await _appusermate.crep(assignmentAppUserMaterials);
            }

        }
    }
}