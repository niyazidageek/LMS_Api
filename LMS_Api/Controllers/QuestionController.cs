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

            for (int i = 0; i < questionsDto.Count; i++)
            {
                questionsDto[i].OptionsCount = questionsDb[i].Options.Count;
            }


            return Ok(questionsDto);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetQuestionById(int id)
        {
            var questionDb = await _questionService.GetQuestionWithOptionsByIdAsync(id);

            if (questionDb is null)
                return NotFound();

            var questionDto = _mapper.Map<QuestionDTO>(questionDb);

            return Ok(questionDto);
        }

        [HttpPost]
        public async Task<ActionResult> CreateQuestion([FromForm] QuestionAttachmentDTO questionAttachmentDto)
        {
            QuestionDTO questionDto = JsonConvert.DeserializeObject<QuestionDTO>(questionAttachmentDto.Values);

            var questionDb = _mapper.Map<Question>(questionDto);

            questionDb.File = questionAttachmentDto.QuestionFile;


            if (questionAttachmentDto.QuestionFile is not null)
            {
                await _questionService.AddQuestionWithFileAsync(questionDb);

                return Ok();
            }

            await _questionService.AddQuestionAsync(questionDb);

            return Ok();
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> EditQuestion([FromForm] QuestionAttachmentDTO questionAttachmentDto, int id)
        {
            QuestionDTO questionDto = JsonConvert.DeserializeObject<QuestionDTO>(questionAttachmentDto.Values);

            var questionDb = await _questionService.GetQuestionWithOptionsByIdAsync(id);

            if (questionDb is null)
                return NotFound();

            questionDto.Id = questionDb.Id;

            //var fileName = questionDb.FileName;

            _mapper.Map(questionDto, questionDb);

            //questionDb.FileName = fileName;

            if (questionAttachmentDto.QuestionFile is not null)
            {
                questionDb.File = questionAttachmentDto.QuestionFile;

                await _questionService.EditQuestionWithFileAsync(questionDb);

                return Ok();
            }
            else 
            {
                await _questionService.EditQuestionWithoutFileAsync(questionDb);

                return Ok();
            }
           
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteQuestion(int id)
        {
            var questionDb = await _questionService.GetQuestionWithOptionsByIdAsync(id);

            if (questionDb is null)
                return NotFound();

            await _questionService.DeleteQuestionAsync(questionDb);

            return Ok();
        }
    }
}