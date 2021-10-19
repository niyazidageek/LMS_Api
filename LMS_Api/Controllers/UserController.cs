using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using System.Web;
using DataAccess.Identity;
using Entities.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Utils;

namespace LMS_Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult> Register([FromBody] RegisterDTO registerRequest)
        {
            var result = await _userService.RegisterAsync(registerRequest);

            if (result.Status == nameof(StatusTypes.EmailError) ||
                result.Status == nameof(StatusTypes.UsernameError))
                return Conflict(result);

            if (result.Status == nameof(StatusTypes.RegistrationError))
                return Unauthorized(result);

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginDTO loginRequest)
        {
            var result = await _userService.LoginAsync(loginRequest);

            if (result.Status == nameof(StatusTypes.LoginError))
                return Unauthorized(result);

            return Ok(result);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddRole([FromBody] AddRoleDTO addRoleRequest)
        {
            var result = await _userService.AddRoleAsync(addRoleRequest);

            if (result.Status == nameof(StatusTypes.RoleError))
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult> ConfirmEmail(string userId, string token)
        {
            var result = await _userService.ConfirmEmailAsync(userId, token);

            if (result.Status == nameof(StatusTypes.ConfirmationError))
                return BadRequest(result);

            if (result.Status == nameof(StatusTypes.UserError))
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordDTO forgetPasswordRequest)
        {
            var result = await _userService.ForgetPasswordAsync(forgetPasswordRequest);
            if (result.Status == nameof(StatusTypes.UserError))
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPasswordRequest)
        {
            var result = await _userService.ResetPasswordAsync(resetPasswordRequest);

            if (result.Status == nameof(StatusTypes.UserError))
                return NotFound(result);

            if (result.Status == nameof(StatusTypes.Success))
                return Ok(result);

            return BadRequest(result);
        }
    }
}