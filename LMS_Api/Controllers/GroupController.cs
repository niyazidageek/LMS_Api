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
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;
        private readonly IMapper _mapper;

        public GroupController(IGroupService groupService, IMapper mapper)
        {
            _groupService = groupService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> GetGroups()
        {
            var groupsDb = await _groupService.GetGroupsAsync();

            if (groupsDb is null)
                return NotFound();

            var groupsDto = _mapper.Map<List<GroupDTO>>(groupsDb);

            return Ok(groupsDto);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetGroupsById(int id)
        {
            var groupDb = await _groupService.GetGroupByIdAsync(id);

            if (groupDb is null)
                return NotFound();

            var groupDto = _mapper.Map<GroupDTO>(groupDb);

            return Ok(groupDto);
        }

        [HttpPost]
        public async Task<ActionResult> CreateGroup([FromBody] GroupDTO groupDto)
        {
            if (!ModelState.IsValid) return BadRequest();

            var groupDb = _mapper.Map<Group>(groupDto);

            await _groupService.AddGroupAsync(groupDb);

            return Ok();
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> EditGroup(int id, [FromBody] GroupDTO groupDto)
        {
            if (!ModelState.IsValid) return BadRequest();

            var groupDb = await _groupService.GetGroupByIdAsync(id);

            if (groupDb is null)
                return NotFound();

            groupDto.Id = groupDb.Id;

            _mapper.Map(groupDto, groupDb);

            await _groupService.EditGroupAsync(groupDb);

            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            await _groupService.DeleteGroupAsync(id);

            return Ok();
        }
    }
}