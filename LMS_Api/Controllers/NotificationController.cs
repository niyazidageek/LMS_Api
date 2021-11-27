using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Abstract;
using Entities.DTOs;
using LMS_Api.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Utils;

namespace LMS_Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly IAppUserNotificationService _appUserNotificationService;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;
        private readonly IHubContext<BroadcastHub> _hub;

        public NotificationController(IAppUserNotificationService appUserNotificationService,
            INotificationService notificationService, IMapper mapper,
             IHubContext<BroadcastHub> hub)
        {
            _appUserNotificationService = appUserNotificationService;
            _notificationService = notificationService;
            _mapper = mapper;
            _hub = hub;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetUnreadNotifications()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "uid").Value;

            if (userId is null)
                return Unauthorized();

            var notificationsDb = await _appUserNotificationService
                .GetUnreadAppUserNotificationsByUserIdAsync(userId);

            if (notificationsDb is null)
                return NotFound();

            var notificationsDto = _mapper.Map<List<AppUserNotificationDTO>>(notificationsDb);

            return Ok(notificationsDto);
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public async Task<ActionResult> MarkNotificationAsRead(int id)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "uid").Value;

            if (userId is null)
                return Unauthorized();

            var notificationAppUserDb = await _appUserNotificationService
                .GetAppUserNotificationById(id);

            if (notificationAppUserDb is null)
                return NotFound();

            if (notificationAppUserDb.IsRead)
                return Conflict(new ResponseDTO
                {
                    Status = nameof(StatusTypes.NotificationError),
                    Message="Notification has already been read!"
                });

            notificationAppUserDb.IsRead = true;

            await _appUserNotificationService.EditAppUserNotificationAsync(notificationAppUserDb);

            return Ok();
        }
    }
}