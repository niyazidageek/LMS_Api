 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Abstract;
using DataAccess.Concrete;
using Entities.DTOs;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Utils;

namespace LMS_Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OptionController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IOptionService _optionService;

        public OptionController(IMapper mapper, IOptionService optionService)
        {
            _mapper = mapper;
            _optionService = optionService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetOptionById(int id)
        {
            var optionDb = await _optionService.GetOptionByIdAsync(id);

            if (optionDb is null)
                return NotFound();

            var optionDto = _mapper.Map<OptionDTO>(optionDb);

            return Ok(optionDto);
        }

        [HttpGet]
        public async Task<ActionResult> GetOptions()
        {
            var optionsDb = await _optionService.GetOptionsAsync();

            if (optionsDb is null)
                return NotFound();

            var optionsDto = _mapper.Map<List<OptionDTO>>(optionsDb);

            return Ok(optionsDto);
        }

        [HttpPost]
        public async Task<ActionResult> CreateOption([FromForm] OptionAttachmentDTO optionAttachmentDto)
        {
            OptionDTO optionDto = JsonConvert.DeserializeObject<OptionDTO>(optionAttachmentDto.Values);

            var optionDb = _mapper.Map<Option>(optionDto);

            if (optionAttachmentDto.OptionFile is not null)
            {
                optionDb.File = optionAttachmentDto.OptionFile;

                await _optionService.AddOptionWithFileAsync(optionDb);

                return Ok();
            }
            else
            {
                await _optionService.AddOptionAsync(optionDb);

                return Ok(new ResponseDTO
                {
                    Status=nameof(StatusTypes.Success),
                    Message="Option has been successfully created!"
                });
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> EditOption([FromForm] OptionAttachmentDTO optionAttachmentDto, int id)
        {
            OptionDTO optionDto = JsonConvert.DeserializeObject<OptionDTO>(optionAttachmentDto.Values);


            var optionDb = await _optionService.GetOptionByIdAsync(id);

            if (optionDb is null)
                return NotFound();

            optionDto.Id = optionDb.Id;
           
            _mapper.Map(optionDto, optionDb);

            if (optionAttachmentDto.OptionFile is not null)
            {
                optionDb.File = optionAttachmentDto.OptionFile;

                await _optionService.EditQuestionWithFileAsync(optionDb);

                return Ok(new ResponseDTO
                {
                    Status = nameof(StatusTypes.Success),
                    Message = "Option has been successfully edited!"
                });
            }
            else
            {
                await _optionService.EditQuestionWithoutFileAsync(optionDb);

                return Ok(new ResponseDTO
                {
                    Status = nameof(StatusTypes.Success),
                    Message = "Option has been successfully edited!"
                });
            }
            
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteOption(int id)
        {
            var optionDb = await _optionService.GetOptionByIdAsync(id);

            if (optionDb is null)
                return NotFound();

            if (optionDb.FileName is not null)
            {
                await _optionService.DeleteQuestionWithFileAsync(optionDb);
            }
            else
            {
                await _optionService.DeleteOptionAsync(optionDb);
            }
           
            return Ok(new ResponseDTO
            {
                Status=nameof(StatusTypes.Success),
                Message="Option has been successfully deleted!"
            });
        }
    }
}