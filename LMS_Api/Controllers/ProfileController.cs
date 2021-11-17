using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Abstract;
using Entities.DTOs;
using Entities.Models;
using LMS_Api.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Utils;

namespace LMS_Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IGroupService _groupService;

        public ProfileController(UserManager<AppUser> userManager, IMapper mapper,
            IGroupService groupService)
        {
            _groupService = groupService;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetMyProfile()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "uid").Value;

            if (userId is null)
                return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return NotFound();

            var userDto = _mapper.Map<AppUserDTO>(user);

            var groupsDb = await _groupService.GetGroupsByUserIdAsync(userId);

            var groupsDto = _mapper.Map<List<GroupDTO>>(groupsDb);

            userDto.Groups = groupsDto;

            return Ok(userDto);

        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult> ChangeProfilePicture([FromForm] ProfilePictureAttachmentDTO profilePictureAttachmentDto)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "uid").Value;
            if (userId is null)
                return Unauthorized();

            if (profilePictureAttachmentDto.Picture is null)
                return BadRequest();

            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return NotFound();

            if(user.Filename is not null)
            {
                FileHelper.DeleteFile(user.Filename);
            }

            var fileName = await FileHelper.AddFile(profilePictureAttachmentDto.Picture);

            user.Filename = fileName;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok(new ResponseDTO
                {
                    Status = nameof(StatusTypes.Success),
                    Message = "Profile picture has been changed!"
                });
            }
            return BadRequest(new ResponseDTO
            {
                Status = nameof(StatusTypes.UserError),
                Message = "Something went wrong!"
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> RequestChangeEmail([FromBody] AppUserDTO appUserDto)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "uid").Value;

            if (userId is null)
                return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return NotFound();

            var confirmationToken = await _userManager.GenerateChangeEmailTokenAsync(user, appUserDto.Email);

            var encodedToken = Encoding.UTF8.GetBytes(confirmationToken);
            var validEmailToken = WebEncoders.Base64UrlEncode(encodedToken);
            string url = $"http://localhost:3000/ConfirmEmail/{user.Id}/{validEmailToken}/{appUserDto.Email}";

            var succeeded = EmailHelper.SendMailToOneUser(appUserDto.Email, "Confirm your new email", "", url);

            if (succeeded)
                return Ok(new ResponseDTO
                {
                    Status = nameof(StatusTypes.Success),
                    Message = "Message with confirmation has been sent to your new e-mail!"
                });

            return BadRequest(new ResponseDTO
            {
                Status = nameof(StatusTypes.EmailError),
                Message = "Email can't be sent!"
            });

        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> ConfirmNewEmail([FromBody] ConfirmEmailDTO confirmEmailDto)
        {
            var user = await _userManager.FindByIdAsync(confirmEmailDto.UserId);

            if (user is null)
                return NotFound(new ResponseDTO
                {
                    Status = nameof(StatusTypes.UsernameError),
                    Message = "There is no such user"
                });

            var decodedToken = WebEncoders.Base64UrlDecode(confirmEmailDto.Token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);

            var result = await _userManager.ChangeEmailAsync(user, confirmEmailDto.NewEmail,normalToken);

            if (result.Succeeded)
            {
                return Ok(new ResponseDTO
                {
                    Status = StatusTypes.Success.ToString(),
                    Message = "Email has been changed!"
                });
            }

            return BadRequest(new ResponseDTO
            {
                Status = StatusTypes.ConfirmationError.ToString(),
                Message = "Email has not been changed!"
            });
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult> ChangeProfileInfo([FromBody] AppUserDTO appUserDto)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "uid").Value;

            if (userId is null)
                return Unauthorized();

            var userDb = await _userManager.FindByIdAsync(userId);

            if (userDb is null)
                return NotFound();

            userDb.Name = appUserDto.Name;
            userDb.Surname = appUserDto.Surname;
            userDb.Bio = appUserDto.Bio;
            userDb.IsSubscribedToSender = appUserDto.IsSubscribedToSender;

            var result = await _userManager.UpdateAsync(userDb);

            if (result.Succeeded)
            {
                return Ok(new ResponseDTO
                {
                    Status = nameof(StatusTypes.Success),
                    Message = "User information has been changed!"
                });
            }
            return BadRequest(new ResponseDTO
            {
                Status = nameof(StatusTypes.UserError),
                Message = "Something went wrong!"
            });
        }
    }
}