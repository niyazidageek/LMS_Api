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
        public async Task<ActionResult> CreateLesson([FromForm] LessonDTO lessonDto)
        {
            if (!ModelState.IsValid) return BadRequest();

            var lessonDb = _mapper.Map<Lesson>(lessonDto);

            var groupDb = await _context.Groups.FirstOrDefaultAsync(g => g.Id == lessonDto.GroupId);

            if (groupDb is null)
                return NotFound();

            lessonDb.Group = groupDb;

            List<string> fileNames = new();

            if(lessonDto.Files is not null)
            {
                try
                {
                    foreach (var file in lessonDto.Files)
                    {
                        var fileName = FileHelper.AddFile(file);
                        if (fileName is null)
                            return BadRequest();
                        fileNames.Add(fileName);
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

                await _lessonService.AddLessonAsync(lessonDb, fileNames);

                return Ok();
            }

            await _lessonService.AddLessonAsync(lessonDb);

            return Ok();
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> EditLesson(int id, [FromBody] LessonDTO lessonDto)
        {
            //if (!ModelState.IsValid) return BadRequest();

            //var subjectDb = await _subjectService.GetSubjectByIdAsync(id);

            //if (subjectDb is null)
            //    return NotFound();

            //subjectDto.Id = subjectDb.Id;

            //_mapper.Map(subjectDto, subjectDb);

            //await _subjectService.EditSubjectAsync(subjectDb);

            //return Ok();

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