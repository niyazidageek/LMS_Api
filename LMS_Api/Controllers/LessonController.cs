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
        private readonly AppDbContext _context;

        public LessonController(ILessonService lessonService, IMapper mapper,
            AppDbContext context)
        {
            _lessonService = lessonService;
            _mapper = mapper;
            _context = context;
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
            //if (!ModelState.IsValid) return BadRequest();

            LessonDTO lessonDto = JsonConvert.DeserializeObject<LessonDTO>(lessonAttachmentDto.Values);

            var lessonDb = _mapper.Map<Lesson>(lessonDto);

            var groupDb = await _context.Groups.FirstOrDefaultAsync(g => g.Id == lessonDto.Group.Id);

            if (groupDb is null)
                return NotFound();

            lessonDb.Group = groupDb;

            if(lessonAttachmentDto.Files is not null)
            {           
                await _lessonService.AddLessonAsync(lessonDb, lessonAttachmentDto.Files);

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

            var groupDb = await _context.Groups.FirstOrDefaultAsync(g => g.Id == lessonDto.Group.Id);

            if (groupDb is null)
                return NotFound();

            lessonDto.Id = lessonDb.Id;

            _mapper.Map(lessonDto, lessonDb);

            lessonDb.Group = groupDb;

            //if (lessonDto.Files is not null)
            //{


            //    return Ok();
            //}

            List<string> fileNames = new();

            foreach (var materialDto in lessonDto.Materials)
            {
                fileNames.Add(materialDto.FileName);
            }
            
            await _lessonService.EditLessonAsync(lessonDb, lessonAttachmentDto.Files, fileNames);
            

            //await _lessonService.EditLessonAsync(lessonDb);

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