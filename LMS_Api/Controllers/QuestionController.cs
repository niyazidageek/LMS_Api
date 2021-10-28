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
    public class QuestionController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IQuestionService _questionService;

        public QuestionController(IMapper mapper, IQuestionService questionService)
        {
            _mapper = mapper;
            _questionService = questionService;
        }

        [HttpGet]
        public async Task<ActionResult> GetQuestions()
        {
            var questionsDb = await _questionService.GetQuestionsAsync();

            if (questionsDb is null)
                return NotFound();

            var questionsDto = _mapper.Map<List<QuestionDTO>>(questionsDb);

            return Ok(questionsDto);
        }

        [HttpPost]
        public async Task<ActionResult> CreateQuestion([FromForm] QuestionAttachmentDTO questionAttachmentDto)
        {
            QuestionDTO questionDto = JsonConvert.DeserializeObject<QuestionDTO>(questionAttachmentDto.Values);

            var questionDb =  _mapper.Map<Question>(questionDto);

            if(questionAttachmentDto.QuestionFile is not null)
            {
                await _questionService.AddQuestionWithFileAsync(questionDb);

                return Ok();
            }

            await _questionService.AddQuestionAsync(questionDb);

            return Ok(questionDb);
        }
    }
}