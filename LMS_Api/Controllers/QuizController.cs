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
    public class QuizController : ControllerBase
    {
        private readonly IQuizService _quizService;

        private readonly IMapper _mapper;

        public QuizController(IQuizService quizService, IMapper mapper)
        {
            _quizService = quizService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetQuizById(int id)
        {
            var quizDb = await _quizService.GetQuizByIdAsync(id);

            if (quizDb is null)
                return NotFound();

            var quizDto = _mapper.Map<QuizDTO>(quizDb);


            return Ok(quizDto);
        }

        [HttpGet]
        public async Task<ActionResult> GetQuizzes()
        {
            var quizzesDb = await _quizService.GetQuizzesAsync();

            if (quizzesDb is null)
                return NotFound();

            var quizzesDto = _mapper.Map<List<QuizDTO>>(quizzesDb);

            return Ok(quizzesDto);
        }

        [HttpPost]
        public async Task<ActionResult> CreateQuiz([FromBody] QuizDTO quizDto)
        {
            var quizDb = _mapper.Map<Quiz>(quizDto);

            await _quizService.AddQuizAsync(quizDb);

            return Ok();
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> EditQuiz([FromBody] QuizDTO quizDto,int id)
        {
            var quizDb = await _quizService.GetQuizByIdAsync(id);

            if (quizDb is null)
                return NotFound();

            quizDto.Id = quizDb.Id;

            _mapper.Map(quizDto, quizDb);

            await _quizService.EditQuizAsync(quizDb);

            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteQuiz(int id)
        {
            await _quizService.DeleteQuizAsync(id);

            return Ok();
        }
    }
}