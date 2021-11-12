using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
        private readonly ILessonMaterialService _lessonMaterialService;

        public LessonController(ILessonService lessonService,
            IMapper mapper,
            IGroupService groupService,
            ILessonMaterialService lessonMaterialService)
        {
            _lessonMaterialService = lessonMaterialService;
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
        public async Task<ActionResult> CreateLesson([FromForm] LessonAttachmentDTO lessonAttachmentDto)
        {
            var theorycontent = JsonConvert.DeserializeObject(lessonAttachmentDto.Values);

            var b = lessonAttachmentDto.Values.Length;

            await FileHelper.AddJsonFile(lessonAttachmentDto.Values);


            //if (!ModelState.IsValid) return BadRequest();

            //LessonDTO lessonDto = JsonConvert.DeserializeObject<LessonDTO>(lessonAttachmentDto.Values);

            //var lessonDb = _mapper.Map<Lesson>(lessonDto);

            //await _lessonService.AddLessonAsync(lessonDb);

            //if (lessonAttachmentDto.Materials is not null)
            //{
            //    List<LessonMaterial> lessonMaterials = new();

            //    foreach (var file in lessonAttachmentDto.Materials)
            //    {
            //        LessonMaterial lessonMaterial = new();
            //        lessonMaterial.LessonId = lessonDb.Id;
            //        lessonMaterial.File = file;
            //        lessonMaterials.Add(lessonMaterial);
            //    }

            //    await _lessonMaterialService.CreateLessonMaterialsAsync(lessonMaterials);
            //}

            return Ok();
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> EditLesson(int id, [FromForm] LessonAttachmentDTO lessonAttachmentDto)
        {
            //if (!ModelState.IsValid) return BadRequest();

            //LessonDTO lessonDto = JsonConvert.DeserializeObject<LessonDTO>(lessonAttachmentDto.Values);

            //var lessonDb = await _lessonService.GetLessonByIdAsync(id);

            //if (lessonDb is null)
            //    return NotFound();

            //var existingMaterialFiles = lessonDb.LessonMaterials.Select(lm => lm.FileName).ToList();

            //var deleteableMaterialFiles = existingMaterialFiles
            //        .Where(ef => !lessonDto.LessonMaterials.Any(lm => lm.FileName == ef))
            //        .ToList();

            //if (deleteableMaterialFiles is not null || deleteableMaterialFiles.Count is not 0)
            //{
            //    List<LessonMaterial> lessonMaterials = new();

            //    foreach (var file in deleteableMaterialFiles)
            //    {
            //        LessonMaterial lessonMaterial = new();
            //        lessonMaterial.FileName = file;
            //        lessonMaterial.LessonId = lessonDb.Id;
            //        lessonMaterials.Add(lessonMaterial);
            //    }

            //    await _lessonMaterialService.DeleteLessonMaterialsAsync(lessonMaterials);
            //}

            //if(lessonAttachmentDto.Materials is not null)
            //{
            //    List<LessonMaterial> lessonMaterials = new();

            //    foreach (var file in lessonAttachmentDto.Materials)
            //    {
            //        LessonMaterial lessonMaterial = new();
            //        lessonMaterial.LessonId = lessonDb.Id;
            //        lessonMaterial.File = file;
            //        lessonMaterials.Add(lessonMaterial);
            //    }

            //    await _lessonMaterialService.CreateLessonMaterialsAsync(lessonMaterials);
            //}

            //lessonDto.Id = lessonDb.Id;
            //foreach (var lessonMaterialDto in lessonDto.LessonMaterials)
            //{
            //    lessonMaterialDto.Id = lessonDb.LessonMaterials
            //        .FirstOrDefault(lm => lm.FileName == lessonMaterialDto.FileName).Id;
            //}

            //_mapper.Map(lessonDto, lessonDb);

            //await _lessonService.EditLessonAsync(lessonDb);

            return Ok();

        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteLesson(int id)
        {
            //var lessonDb = await _lessonService.GetLessonByIdAsync(id);

            //List<LessonMaterial> lessonMaterials = new();

            //foreach (var lessonMaterial in lessonDb.LessonMaterials)
            //{
            //    lessonMaterials.Add(lessonMaterial);
            //}

            //await _lessonMaterialService.DeleteLessonMaterialsAsync(lessonMaterials);

            //await _lessonService.DeleteLessonAsync(id);

            return Ok();
        }
    }
}