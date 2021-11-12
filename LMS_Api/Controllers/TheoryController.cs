using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Abstract;
using Entities.DTOs;
using Entities.Models;
using LMS_Api.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LMS_Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TheoryController : ControllerBase
    {
        private readonly ITheoryService _theoryService;
        private readonly ILessonService _lessonService;
        private readonly IMapper _mapper;

        public TheoryController(ITheoryService theoryService,
            ILessonService lessonService,
            IMapper mapper)
        {
            _mapper = mapper;
            _theoryService = theoryService;
            _lessonService = lessonService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetTheoriesByLessonId(int id)
        {
            var lessonDb = await _lessonService.GetLessonByIdAsync(id);

            if (lessonDb is null)
                return NotFound();

            var theoriesDb = await _theoryService.GetTheoriesByLessonIdAsync(id);

            var theoriesDto =  _mapper.Map<List<TheoryDTO>>(theoriesDb);

            return Ok(theoriesDto);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetTheoryById(int id)
        {
            var theoryDb = await _theoryService.GetTheoryByIdAsync(id);

            if (theoryDb is null)
                return NotFound();

            var theoryDto = _mapper.Map<TheoryDTO>(theoryDb);

            return Ok(theoryDto);
        }

        [HttpPost]
        public async Task<ActionResult> CreateTheory([FromForm] TheoryAttachmentDTO theoryAttachmentDto)
        {
            var theoryDto = JsonConvert.DeserializeObject<TheoryDTO>(theoryAttachmentDto.Values);

            var theoryDb = _mapper.Map<Theory>(theoryDto);

            var fileName = await FileHelper.AddJsonFile(theoryAttachmentDto.Content);

            theoryDb.FileName = fileName;

            await _theoryService.AddTheoryAsync(theoryDb);

            return Ok();
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> EditTheory(int id, [FromForm] TheoryAttachmentDTO theoryAttachmentDto)
        {
            var theoryDb = await _theoryService.GetTheoryByIdAsync(id);

            if (theoryDb is null)
                return NotFound();

            var theoryDto = JsonConvert.DeserializeObject<TheoryDTO>(theoryAttachmentDto.Values);

            FileHelper.DeleteFile(theoryDb.FileName);

            var newFileName = await FileHelper.AddJsonFile(theoryAttachmentDto.Content);

            theoryDto.Id = theoryDb.Id;
            theoryDto.FileName = newFileName;

            _mapper.Map(theoryDto, theoryDb);

            await _theoryService.EditTheoryAsync(theoryDb);

            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteTheory(int id)
        {
            var theoryDb = await _theoryService.GetTheoryByIdAsync(id);

            if (theoryDb is null)
                return NotFound();

            FileHelper.DeleteFile(theoryDb.FileName);

            await _theoryService.DeleteTheoryAsync(id);

            return Ok();
        }
    }
}