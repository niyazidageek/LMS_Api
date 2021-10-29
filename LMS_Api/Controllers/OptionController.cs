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
    public class OptionController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IOptionService _optionService;
        private readonly IQuestionService _questionService;

        public OptionController(IMapper mapper, IOptionService optionService,
            IQuestionService questionService)
        {
            _mapper = mapper;
            _optionService = optionService;
            _questionService = questionService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateOption([FromForm] OptionAttachmentDTO optionAttachmentDto)
        {
            OptionDTO optionDto = JsonConvert.DeserializeObject<OptionDTO>(optionAttachmentDto.Values);

            var questionDb = await _questionService.GetQuestionByIdAsync(optionDto.Question.Id);

            if (questionDb is null)
                return NotFound();

            var optionDb = _mapper.Map<Option>(optionDto);

            optionDb.Question = questionDb;

            if(optionAttachmentDto.OptionFile is not null)
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
    }
}