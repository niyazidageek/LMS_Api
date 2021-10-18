using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Entities.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

            if (result.Status == StatusTypes.EmailError.ToString() ||
                result.Status == StatusTypes.UsernameError.ToString())
                return Conflict(result);

            if (result.Status == StatusTypes.RegistrationError.ToString())
                return Unauthorized(result);

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginDTO loginRequest)
        {
            var result = await _userService.LoginAsync(loginRequest);

            if (result.Status == StatusTypes.LoginError.ToString())
                return Unauthorized(result);

            return Ok(result);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        public async Task<ActionResult> AddRole([FromBody] AddRoleDTO addRoleRequest)
        {
            var result = await _userService.AddRoleAsync(addRoleRequest);

            if (result.Status == StatusTypes.InvalidToken.ToString())
                return StatusCode(403,result);

            if (result.Status == StatusTypes.RoleError.ToString())
                return NotFound(result);

            return Ok(result);
        }
    }
}