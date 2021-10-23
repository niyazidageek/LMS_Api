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

namespace LMS_Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;
        private readonly IMapper _mapper;

        public SubjectController(ISubjectService subjectService, IMapper mapper)
        {
            _subjectService = subjectService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> GetSubjects()
        {
            var subjectsDb = await _subjectService.GetSubjectsAsync();

            if (subjectsDb is null)
                return NotFound();

            var subjectsDto = _mapper.Map<List<SubjectDTO>>(subjectsDb);

            return Ok(subjectsDto);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetSubjectById(int id)
        {
            var subjectDb = await _subjectService.GetSubjectByIdAsync(id);

            if (subjectDb is null)
                return NotFound();

            var subjectDto = _mapper.Map<SubjectDTO>(subjectDb);


            return Ok(subjectDto);
        }

        [HttpPost]
        public async Task<ActionResult> CreateSubject([FromBody] SubjectDTO subjectDto)
        {
            if (!ModelState.IsValid) return BadRequest();

            var subjectDb = _mapper.Map<Subject>(subjectDto);

            await _subjectService.AddSubjectAsync(subjectDb);

            return Ok();
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> EditSubject(int id, [FromBody] SubjectDTO subjectDto)
        {
            if (!ModelState.IsValid) return BadRequest();

            var subjectDb = await _subjectService.GetSubjectByIdAsync(id);

            if (subjectDb is null)
                return NotFound();

            subjectDto.Id = subjectDb.Id;

            _mapper.Map(subjectDto, subjectDb);

            await _subjectService.EditSubjectAsync(subjectDb);

            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteSubject(int id)
        {
            await _subjectService.DeleteSubjectAsync(id);

            return Ok();
        }
    }
}