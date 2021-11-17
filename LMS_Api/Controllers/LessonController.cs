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
using LMS_Api.Utils;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet]
        [Route("{id}/{page}/{size}")]
        [Authorize(Roles = nameof(Roles.Student))]
        public async Task<ActionResult> GetLessonsByGroupIdAndUserId(int id, int page, int size)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "uid").Value;

            if (userId is null)
                return Unauthorized();

            var lessonsDb = await _lessonService.GetLessonsByGroupIdAndUserIdAsync(id, userId, page, size);

            var lessonsDbCount = await _lessonService.GetLessonsByGroupIdCountAsync(id);

            if (lessonsDb is null)
                return NotFound();

            var lessonsDto = _mapper.Map<List<LessonDTO>>(lessonsDb);

            HttpContext.Response.Headers.Add("Count", lessonsDbCount.ToString());

            return Ok(lessonsDto);
        }

        [HttpGet]
        [Route("{groupId}/{skip}/{take}")]
        public async Task<ActionResult> GetLessonsByGroupId(int groupId, int skip, int take)
        {
            var lessonsDb = await _lessonService.GetLessonsByGroupIdAsync(groupId, skip, take);

            if (lessonsDb is null)
                return NotFound();

            var lessonsDto = _mapper.Map<LessonDTO>(lessonsDb);

            return Ok(lessonsDto);
        }


        [HttpPost]
        public async Task<ActionResult> CreateLesson([FromBody] LessonDTO lessonDto)
        {

            //if (!ModelState.IsValid) return BadRequest();

            var lessonDb = _mapper.Map<Lesson>(lessonDto);

            await _lessonService.AddLessonAsync(lessonDb);

            return Ok();
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> EditLesson(int id, [FromBody] LessonDTO lessonDto)
        {
            //if (!ModelState.IsValid) return BadRequest();

            var lessonDb = await _lessonService.GetLessonByIdAsync(id);

            if (lessonDb is null)
                return NotFound();

            lessonDto.Id = lessonDb.Id;

            _mapper.Map(lessonDto, lessonDb);

            await _lessonService.EditLessonAsync(lessonDb);

            return Ok();

        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteLesson(int id)
        {
            var lessonDb = await _lessonService.GetLessonByIdAsync(id);

            if (lessonDb is null)
                return NotFound();

            await _lessonService.DeleteLessonAsync(id);

            return Ok();
        }
    }
}