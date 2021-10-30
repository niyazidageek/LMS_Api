using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Abstract;
using DataAccess.Concrete;
using Entities.DTOs;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace LMS_Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OptionController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IOptionService _optionService;
        private readonly IQuestionService _questionService;
        private readonly AppDbContext _context;

        public OptionController(IMapper mapper, IOptionService optionService,
            IQuestionService questionService, AppDbContext context)
        {
            _mapper = mapper;
            _optionService = optionService;
            _questionService = questionService;
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult> CreateOption([FromForm] OptionAttachmentDTO optionAttachmentDto)
        {
            OptionDTO optionDto = JsonConvert.DeserializeObject<OptionDTO>(optionAttachmentDto.Values);

            var optionDb = _mapper.Map<Option>(optionDto);

            //var questionDb = await _questionService.GetQuestionWithOptionsByIdAsync(optionDto.Question.Id);

            var questionDb = await _context.Questions.FirstOrDefaultAsync(q => q.Id == optionDto.Question.Id);

            if (questionDb is null)
                return NotFound();

            optionDb.Question = questionDb;

            if (optionAttachmentDto.OptionFile is not null)
            {
                optionDb.File = optionAttachmentDto.OptionFile;

                await _optionService.AddOptionWithFileAsync(optionDb);

                return Ok();
            }
            else
            {
                await _optionService.AddOptionAsync(optionDb);

                return Ok();
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> EditOption([FromForm] OptionAttachmentDTO optionAttachmentDto, int id)
        {
            OptionDTO optionDto = JsonConvert.DeserializeObject<OptionDTO>(optionAttachmentDto.Values);

            var questionDb = await _questionService.GetQuestionByIdAsync(optionDto.Question.Id);

            if (questionDb is null)
                return NotFound();

            var optionDb = await _optionService.GetOptionByIdAsync(id);

            if (optionDb is null)
                return NotFound();

            optionDto.Id = optionDb.Id;

            var fileName = optionDb.FileName;

            _mapper.Map(optionDto, optionDb);

            optionDb.FileName = fileName;
            optionDb.Question = questionDb;

            if (optionAttachmentDto.OptionFile is not null)
            {
                optionDb.File = optionAttachmentDto.OptionFile;

                await _optionService.EditQuestionWithFileAsync(optionDb);

                return Ok();
            }
            else if (optionDto.FileName is not null)
            {
                await _optionService.EditOptionAsync(optionDb);

                return Ok();
            }
            else
            {
                if (optionDb.FileName is not null)
                {
                    await _optionService.EditQuestionWithoutFileAsync(optionDb);

                    return Ok();
                }
                else
                {
                    await _optionService.EditOptionAsync(optionDb);

                    return Ok();
                }

            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteOption(int id)
        {
            var optionDb = await _optionService.GetOptionByIdAsync(id);

            if (optionDb is null)
                return NotFound();

            await _optionService.DeleteQuestionWithFileAsync(optionDb);

            return Ok();
        }
    }
}