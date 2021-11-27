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
using LMS_Api.Hubs;
using LMS_Api.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Utils;

namespace LMS_Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly IQuizService _quizService;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly IAppUserQuizService _appUserQuizService;
        private readonly IGroupMaxPointService _groupMaxPointService;
        private readonly IAppUserGroupService _appUserGroupService;
        private readonly IAppUserGroupPointService _appUserGroupPointService;
        private readonly IQuestionService _questionService;
        private readonly IOptionService _optionService;
        private readonly IAppUserOptionService _appUserOptionService;
        private readonly INotificationService _notificationService;
        private readonly IAppUserNotificationService _appUserNotificationService;
        private readonly IHubContext<BroadcastHub> _hub;

        public QuizController(IQuizService quizService, IMapper mapper,
            UserManager<AppUser> userManager, IAppUserQuizService appUserQuizService,
            IGroupMaxPointService groupMaxPointService, IQuestionService questionService,
            IOptionService optionService, IAppUserGroupService appUserGroupService,
            IAppUserGroupPointService appUserGroupPointService, IAppUserOptionService appUserOptionService,
            INotificationService notificationService, IAppUserNotificationService appUserNotificationService,
            IHubContext<BroadcastHub> hub)
        {
            _notificationService = notificationService;
            _appUserGroupPointService = appUserGroupPointService;
            _appUserGroupService = appUserGroupService;
            _groupMaxPointService = groupMaxPointService;
            _appUserOptionService = appUserOptionService;
            _quizService = quizService;
            _mapper = mapper;
            _userManager = userManager;
            _appUserQuizService = appUserQuizService;
            _questionService = questionService;
            _optionService = optionService;
            _appUserNotificationService = appUserNotificationService;
            _hub = hub;
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


        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = nameof(Roles.Student))]
        public async Task<ActionResult> SubmitQuizById(int id, [FromBody] QuizSubmissionDTO quizSubmissionDto)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "uid").Value;

            if (userId is null)
                return Unauthorized();

            var quizDb = await _quizService.GetQuizByIdAsync(id);

            if (quizDb is null)
                return NotFound();

            var appUserQuizDb = await _appUserQuizService.GetAppUserQuizByUserIdAndQuizIdAsync(userId, quizDb.Id);

            if (appUserQuizDb is null)
                return NotFound();

            if (appUserQuizDb.IsSubmitted)
                return Conflict(new ResponseDTO
                {
                    Status=nameof(StatusTypes.QuizError),
                    Message="You have already submitted the quiz!"
                });

            appUserQuizDb.SubmissionDate = DateTime.UtcNow;
            appUserQuizDb.isLate = appUserQuizDb.SubmissionDate > quizDb.Deadline ? true : false;
            appUserQuizDb.IsSubmitted = true;

            decimal result = 0;
            int correctCount = 0;


            foreach (var quizAnswer in quizSubmissionDto.QuizAnswers)
            {
                var questionDb = await _questionService.GetQuestionByIdAsync(quizAnswer.QuestionId);

                if (questionDb is null)
                    return NotFound();

                AppUserOption appUserOption = new();

                if (quizAnswer.OptionId is null)
                {
                    appUserOption.AppUserId = userId;
                    appUserOption.OptionId = null;
                    appUserOption.QuestionId = questionDb.Id;
                }
                else
                {
                    var optionDb = await _optionService.GetOptionByIdAsync((int)quizAnswer.OptionId);

                    if (optionDb is null)
                        return NotFound();

                    if (optionDb.IsCorrect)
                    {
                        result += questionDb.Point;
                        correctCount++;
                    }    

                    appUserOption.AppUserId = userId;
                    appUserOption.OptionId = optionDb.Id;
                    appUserOption.QuestionId = questionDb.Id;
                }

                await _appUserOptionService.AddAppUserOptionAsync(appUserOption);
            }

            appUserQuizDb.Result = result;
            appUserQuizDb.CorrectAnswers = correctCount;

            await _appUserQuizService.EditAppUserQuizAsync(appUserQuizDb);

            var appUserGroup = await _appUserGroupService
                .GetAppUserGroupByUserIdAndGroupIdAsync(userId, quizDb.GroupId);

            var appUserGroupPointDb = await _appUserGroupPointService
                .GetAppUserGroupPointByAppUserGroupIdAsync(appUserGroup.Id);

            appUserGroupPointDb.Point += appUserQuizDb.Result;

            await _appUserGroupPointService.EditAppUserGroupPointAsync(appUserGroupPointDb);

            return Ok(new ResponseDTO
            {
                Status=nameof(StatusTypes.Success),
                Message="You have successfully submitted the quiz!"
            });

        }


        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> MakeQuizAvailableById(int id, [FromBody] QuizDTO quizDto)
        {
            var quizDb = await _quizService.GetQuizByIdAsync(id);

            if (quizDb is null)
                return NotFound();

            if (quizDb.IsAvailable)
                return Conflict(new ResponseDTO
                {
                    Status=nameof(StatusTypes.QuizError),
                    Message="Quiz is available already!"
                });

            quizDb.IsAvailable = true;

            await _quizService.EditQuizAsync(quizDb);

            Notification notification = new()
            {
                Content = $"Quiz '{quizDb.Name}' is available!",
                CreationDate = DateTime.UtcNow
            };

            await _notificationService.AddNotificationAsync(notification);

            List<AppUserGroup> students = new();

            List<string> receivers = new();

            foreach (var appUserGroup in quizDb.Group.AppUserGroups)
            {
                var user = await _userManager.FindByIdAsync(appUserGroup.AppUserId);

                var roles = await _userManager.GetRolesAsync(user);

                var isStudent = roles.Any(x => x.ToLower() == nameof(Roles.Student).ToLower());

                if (isStudent is true)
                {
                    students.Add(appUserGroup);

                    AppUserNotification appUserNotification = new()
                    {
                        AppUserId = appUserGroup.AppUserId,
                        NotificationId = notification.Id,
                        Notification = notification,
                        IsRead = false
                    };
                        
                    await _appUserNotificationService
                        .AddAppUserNotificationAsync(appUserNotification);

                    var appUserNotificationDto = _mapper.Map<AppUserNotificationDTO>(appUserNotification);

                    var appUserNotificationJson = JsonConvert.SerializeObject(appUserNotificationDto,
                        new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

                    await _hub.Clients.User(appUserGroup.AppUserId).SendAsync("ReceiveNotification", appUserNotificationJson);

                    if (user.IsSubscribedToSender is true)
                        receivers.Add(user.Email);
                }
            }

            await _appUserQuizService.InitializeQuizAsync(students, quizDb.Id);

            if (quizDto.NotifyAll)
            {
                var isSent = EmailHelper.SendMailToManyUsers(receivers, "New quiz is available!");

                if (isSent is false)
                    return BadRequest(new ResponseDTO
                    {
                        Status = nameof(StatusTypes.EmailError),
                        Message = "Email can't be sent!"
                    });
            } 

            return Ok(new ResponseDTO
            {
                Status = nameof(StatusTypes.Success),
                Message = "Quiz has been successfully posted!"
            });

        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = nameof(Roles.Student))]

        public async Task<ActionResult> GetStudentsQuizById(int id)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "uid").Value;

            if (userId is null)
                return Unauthorized();

            var quizDb = await _quizService.GetQuizByUserIdAsync(id, userId);

            if (quizDb is null)
                return NotFound();

            var quizDto = _mapper.Map<QuizDTO>(quizDb);

            return Ok(quizDto);
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = nameof(Roles.Student))]

        public async Task<ActionResult> GetStudentsQuizInfoById(int id)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "uid").Value;

            if (userId is null)
                return Unauthorized();

            var quizDb = await _quizService.GetQuizInfoByUserIdAsync(id, userId);

            if (quizDb is null)
                return NotFound();

            var quizDto = _mapper.Map<QuizDTO>(quizDb);

            var questionsCount = await _questionService.GetQuestionsCountByQuizIdAsync(quizDb.Id);

            quizDto.QuestionCount = questionsCount;

            if (quizDb.AppUserQuizzes.First().IsSubmitted == true)
            {
                var appUserOptions = await _appUserOptionService
                    .GetAppUserOptionsByQuizIdAndUserIdAsync(quizDb.Id, userId);

                var appUserOptionsDto = _mapper.Map<List<AppUserOptionDTO>>(appUserOptions);

                quizDto.AppUserOptions = appUserOptionsDto;
            }

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

        [HttpGet]
        [Route("{groupId}/{page}/{size}")]
        [Authorize(Roles = nameof(Roles.Student))]
        public async Task<ActionResult> GetStudentsQuizzesByGroupId(int groupId, int page, int size)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "uid").Value;

            if (userId is null)
                return Unauthorized();

            if (page < 0)
                return BadRequest(new ResponseDTO
                {
                    Status = nameof(StatusTypes.OffsetError),
                    Message = "Can't load information!"
                });

            var quizzesDb = await _quizService.GetQuizzesByGroupIdAndUserIdAsync(userId,groupId, page, size);

            if (quizzesDb is null)
                return NotFound();

            var quizzesDbCount = await _quizService.GetQuizzesCountByGroupIdAndUserIdAsync(userId,groupId);

            var quizzesDto = _mapper.Map<List<QuizDTO>>(quizzesDb);

            HttpContext.Response.Headers.Add("Count", quizzesDbCount.ToString());

            return Ok(quizzesDto);
        }

        [HttpGet]
        [Route("{groupId}/{page}/{size}")]
        [Roles(nameof(Roles.Teacher), nameof(Roles.Admin), nameof(Roles.SuperAdmin))]
        public async Task<ActionResult> GetQuizzesByGroupId(int groupId, int page, int size)
        {
            if (page < 0)
                return BadRequest(new ResponseDTO
                {
                    Status = nameof(StatusTypes.OffsetError),
                    Message = "Can't load information!"
                });

            var quizzesDb = await _quizService.GetQuizzesByGroupIdAsync(groupId, page, size);

            if (quizzesDb is null)
                return NotFound();

            var quizzesDbCount = await _quizService.GetQuizzesCountByGroupIdAsync(groupId);

            var quizzesDto = _mapper.Map<List<QuizDTO>>(quizzesDb);

            HttpContext.Response.Headers.Add("Count", quizzesDbCount.ToString());

            return Ok(quizzesDto);
        }


        [HttpPost]
        public async Task<ActionResult> CreateQuiz([FromBody] QuizDTO quizDto)
        {
            var quizDb = _mapper.Map<Quiz>(quizDto);

            quizDb.QuizMaxPoint = new QuizMaxPoint { QuizId = quizDb.Id };

            await _quizService.AddQuizAsync(quizDb);

            return Ok(new ResponseDTO
            {
                Status=nameof(StatusTypes.Success),
                Message="Quiz has been successfully created!"
            });
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

            return Ok(new ResponseDTO
            {
                Status=nameof(StatusTypes.Success),
                Message="Quiz has been successfully edited!"
            });
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteQuiz(int id)
        {
            await _quizService.DeleteQuizAsync(id);

            return Ok(new ResponseDTO
            {
                Status=nameof(StatusTypes.Success),
                Message="Quiz has been successfully deleted!"
            });
        }
    }
}