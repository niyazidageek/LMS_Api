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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Utils;

namespace LMS_Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationService _applicationService;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public ApplicationController(IApplicationService applicationService,
            IMapper mapper, UserManager<AppUser> userManager)
        {
            _applicationService = applicationService;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpPost]
        [Authorize(Roles = nameof(Roles.Student))]
        public async Task<ActionResult> PostApplication([FromBody] ApplicationDTO applicationDto)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "uid").Value;

            if (userId is null)
                return Unauthorized();

            var applicationDb = _mapper.Map<Application>(applicationDto);

            applicationDb.CreationDate = DateTime.UtcNow;

            await _applicationService.AddApplicationAsync(applicationDb);

            return Ok(new ResponseDTO
            {
                Status = nameof(StatusTypes.Success),
                Message = "Your application have been successfully submitted!"
            });
        }

        [HttpGet]
        [Route("{page}/{size}")]
        [Roles(nameof(Roles.Admin), nameof(Roles.SuperAdmin))]
        public async Task<ActionResult> GetApplicationsByPageAndSize(int page, int size)
        {
            var applicationsDb = await _applicationService.GetApplicationsByPageAndSizeAsync(page, size);

            if (applicationsDb is null)
                return NotFound();

            var applicationsDto = _mapper.Map<List<ApplicationDTO>>(applicationsDb);

            var applicationsCountDb = await _applicationService.GetApplicationsCountAsync();

            HttpContext.Response.Headers.Add("Count", applicationsCountDb.ToString());

            return Ok(applicationsDto);
        }

        [HttpGet]
        [Route("{id}")]
        [Roles(nameof(Roles.Admin), nameof(Roles.SuperAdmin))]
        public async Task<ActionResult> GetApplicationById(int id)
        {
            var application = await _applicationService.GetApplicationByIdAsync(id);

            if (application is null)
                return NotFound();

            var applicationDto = _mapper.Map<ApplicationDTO>(application);

            return Ok(applicationDto);
        }

        [HttpDelete]
        [Route("{id}")]
        [Roles(nameof(Roles.Admin), nameof(Roles.SuperAdmin))]
        public async Task<ActionResult> DeleteApplicationById(int id)
        {
            var application = await _applicationService.GetApplicationByIdAsync(id);

            if (application is null)
                return NotFound();

            await _applicationService.DeleteApplicationAsync(id);

            return Ok(new ResponseDTO
            {
                Status = nameof(StatusTypes.Success),
                Message = "Application has been successfully deleted!"
            });
        }
    }
}