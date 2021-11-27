using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Abstract;
using DataAccess.Identity;
using Entities.DTOs;
using Entities.Models;
using LMS_Api.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Utils;

namespace LMS_Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IQuestionService _questionService;
        private readonly IQuizService _quizService;
        private readonly IGroupMaxPointService _groupMaxPointService;
        private readonly IQuizMaxPointService _quizMaxPointService;

        public QuestionController(IMapper mapper, IQuestionService questionService,
            IQuizService quizService, IGroupMaxPointService groupMaxPointService,
            IQuizMaxPointService quizMaxPointService)
        {
            _quizMaxPointService = quizMaxPointService;
            _mapper = mapper;
            _questionService = questionService;
            _quizService = quizService;
            _groupMaxPointService = groupMaxPointService;
        }


        [HttpGet]
        [Route("{quizId}")]
        public async Task<ActionResult> GetQuestionsByQuizId(int quizId)
        {
            var quizDb = await _quizService.GetQuizByIdAsync(quizId);

            if (quizDb is null)
                return NotFound();
            

            var questionsDb = await _questionService.GetQuestionsByQuizId(quizId);

            if (questionsDb is null)
                return NotFound();

            var questionsDto = _mapper.Map<List<QuestionDTO>>(questionsDb);

            return Ok(questionsDto);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetQuestionById(int id)
        {
            var questionDb = await _questionService.GetQuestionByIdAsync(id);

            if (questionDb is null)
                return NotFound();

            var questionDto = _mapper.Map<QuestionDTO>(questionDb);

            return Ok(questionDto);
        }

        [HttpPost]
        public async Task<ActionResult> CreateQuestion([FromForm] QuestionAttachmentDTO questionAttachmentDto)
        {
            QuestionDTO questionDto = JsonConvert.DeserializeObject<QuestionDTO>(questionAttachmentDto.Values);

            var quizDb = await _quizService.GetQuizByIdAsync(questionDto.QuizId);

            if (quizDb is null)
                return NotFound();

            var questionDb = _mapper.Map<Question>(questionDto);

            questionDb.File = questionAttachmentDto.QuestionFile;


            var groupMaxPointDb = await _groupMaxPointService.GetGroupMaxPointByGroupId(quizDb.GroupId);
            var quizMaxPointDb = await _quizMaxPointService.GetQuizMaxPointByQuizId(quizDb.Id);

            groupMaxPointDb.MaxPoint += questionDb.Point;
            quizMaxPointDb.MaxPoint += questionDb.Point;

            await _groupMaxPointService.EditGroupMaxPoint(groupMaxPointDb);
            await _quizMaxPointService.EditQuizMaxPoint(quizMaxPointDb);

            if (questionAttachmentDto.QuestionFile is not null)
            {
                await _questionService.AddQuestionWithFileAsync(questionDb);

                return Ok(new ResponseDTO
                {
                    Status=nameof(StatusTypes.Success),
                    Message="Question has been successfully created!"
                });
            }

            await _questionService.AddQuestionAsync(questionDb);

            return Ok(new ResponseDTO
            {
                Status = nameof(StatusTypes.Success),
                Message = "Question has been successfully created!"
            });
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> EditQuestion([FromForm] QuestionAttachmentDTO questionAttachmentDto, int id)
        {
            QuestionDTO questionDto = JsonConvert.DeserializeObject<QuestionDTO>(questionAttachmentDto.Values);

            var questionDb = await _questionService.GetQuestionByIdAsync(id);

            if (questionDb is null)
                return NotFound();

            var oldGrade = questionDb.Point;
            var groupMaxPoint = await _groupMaxPointService.GetGroupMaxPointByGroupId(questionDb.Quiz.GroupId);
            var quizMaxPoint = await _quizMaxPointService.GetQuizMaxPointByQuizId(questionDb.Quiz.Id);

            questionDto.Id = questionDb.Id;

            _mapper.Map(questionDto, questionDb);
  

            groupMaxPoint.MaxPoint = groupMaxPoint.MaxPoint - oldGrade + questionDb.Point;
            quizMaxPoint.MaxPoint = quizMaxPoint.MaxPoint - oldGrade + questionDb.Point;

            await _groupMaxPointService.EditGroupMaxPoint(groupMaxPoint);
            await _quizMaxPointService.EditQuizMaxPoint(quizMaxPoint);

            if (questionAttachmentDto.QuestionFile is not null)
            {
                questionDb.File = questionAttachmentDto.QuestionFile;

                await _questionService.EditQuestionWithFileAsync(questionDb);

                return Ok(new ResponseDTO
                {
                    Status=nameof(StatusTypes.Success),
                    Message="Question has been successfully updated!"
                });
            }
            else 
            {
                await _questionService.EditQuestionWithoutFileAsync(questionDb);

                return Ok(new ResponseDTO
                {
                    Status = nameof(StatusTypes.Success),
                    Message = "Question has been successfully updated!"
                });
            }
           
        }

        [HttpGet]
        [Route("{page}/{quizId}")]
        [Roles(nameof(Roles.Teacher), nameof(Roles.Student), nameof(Roles.Admin), nameof(Roles.SuperAdmin))]
        public async Task<ActionResult> GetQuestionByPageAndQuizId(int page, int quizId)
        {
            if (page < 0)
                return BadRequest(new ResponseDTO
                {
                    Status = nameof(StatusTypes.OffsetError),
                    Message = "Can't load information!"
                });

            var quiz = await _quizService.GetQuizByIdAsync(quizId);

            if(quiz is null)
                return NotFound(new ResponseDTO
                {
                    Status = nameof(StatusTypes.NotFoundError),
                    Message = "Quiz is not found!"
                });

            var questionDb = await _questionService.GetQuestionByPageAndQuizId(page, quizId);

            if (questionDb is null)
                return NotFound(new ResponseDTO
                {
                    Status=nameof(StatusTypes.NotFoundError),
                    Message="Question is not found!"
                });

            var questionsDbCount = await _questionService.GetQuestionsCountByQuizIdAsync(quizId);

            var questionDto = _mapper.Map<QuestionQuizDTO>(questionDb);

            HttpContext.Response.Headers.Add("Count", questionsDbCount.ToString());

            return Ok(questionDto);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteQuestion(int id)
        {
            var questionDb = await _questionService.GetQuestionByIdAsync(id);

            if (questionDb is null)
                return NotFound();

            var currentGrade = questionDb.Point;

            await _questionService.DeleteQuestionAsync(questionDb);

            var groupMaxPoint = await _groupMaxPointService.GetGroupMaxPointByGroupId(questionDb.Quiz.GroupId);
            var quizMaxPoint = await _quizMaxPointService.GetQuizMaxPointByQuizId(questionDb.Quiz.Id);

            quizMaxPoint.MaxPoint -= currentGrade;
            groupMaxPoint.MaxPoint -= currentGrade;

            await _groupMaxPointService.EditGroupMaxPoint(groupMaxPoint);
            await _quizMaxPointService.EditQuizMaxPoint(quizMaxPoint);

            return Ok(new ResponseDTO
            {
                Status=nameof(StatusTypes.Success),
                Message="Question has been successfully deleted!"
            });
        }
    }
}