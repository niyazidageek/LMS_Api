using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Abstract;
using DataAccess.Concrete;
using Entities.DTOs;
using Entities.Models;
using LMS_Api.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace LMS_Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LessonController : ControllerBase
    {
        private readonly ILessonService _lessonService;
        private readonly IMapper _mapper;
        private readonly IGroupService _groupService;

        public LessonController(ILessonService lessonService,
            IMapper mapper,
            IGroupService groupService)
        {
            _lessonService = lessonService;
            _mapper = mapper;
            _groupService = groupService;
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

        [HttpPost]
        public async Task<ActionResult> CreateLesson([FromForm] LessonAttachmentDTO lessonAttachmentDto)
        {
            if (!ModelState.IsValid) return BadRequest();

            LessonDTO lessonDto = JsonConvert.DeserializeObject<LessonDTO>(lessonAttachmentDto.Values);

            var lessonDb = _mapper.Map<Lesson>(lessonDto);

            var groupDb = await _groupService.GetGroupByIdAsync(lessonDto.Group.Id);

            if (groupDb is null)
                return NotFound();

            lessonDb.Group = groupDb;

            if(lessonAttachmentDto.Files is not null)
            {
                lessonDb.Files = lessonAttachmentDto.Files;
                await _lessonService.AddLessonWithFilesAsync(lessonDb);

                return Ok();
            }

            await _lessonService.AddLessonAsync(lessonDb);

            return Ok();
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> EditLesson(int id, [FromForm] LessonAttachmentDTO lessonAttachmentDto)
        {
            if (!ModelState.IsValid) return BadRequest();

            LessonDTO lessonDto = JsonConvert.DeserializeObject<LessonDTO>(lessonAttachmentDto.Values);

            var lessonDb = await _lessonService.GetLessonByIdAsync(id);

            if (lessonDb is null)
                return NotFound();

            var groupDb = await _groupService.GetGroupByIdAsync(lessonDto.Group.Id);

            if (groupDb is null)
                return NotFound();

            lessonDto.Id = lessonDb.Id;

            _mapper.Map(lessonDto, lessonDb);

            lessonDb.Group = groupDb;

            if (lessonDto.Materials.Count is not 0)
            {
                var existingFileNames = new List<string>();

                foreach (var materialDto in lessonDto.Materials)
                {
                    existingFileNames.Add(materialDto.FileName); 
                }

                lessonDb.ExistingFileNames = existingFileNames;
            }

            if(lessonAttachmentDto.Files is not null)
            {
                lessonDb.Files = lessonAttachmentDto.Files;

                 await _lessonService.EditLessonWithFilesAsync(lessonDb);

                return Ok();
            }


            await _lessonService.EditLessonAsync(lessonDb);
            
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteLesson(int id)
        {
            await _lessonService.DeleteLessonAsync(id);

            return Ok();
        }
    }
}