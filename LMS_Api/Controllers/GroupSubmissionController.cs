using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS_Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GroupSubmissionController : ControllerBase
    {
        private readonly IGroupSubmissionService _groupSubmissionService;
        private readonly IAssignmentAppUserService _assignmentAppUserService;

        public GroupSubmissionController(IGroupSubmissionService groupSubmissionService,
            IAssignmentAppUserService assignmentAppUserService)
        {
            _groupSubmissionService = groupSubmissionService;
            _assignmentAppUserService = assignmentAppUserService;
        }

        [HttpGet]
        [Route("{groupId}/{year?}")]
        public async Task<ActionResult> GetSubmissionsCountByGroupIdAndYear(int groupId, int? year)
        {
            int _year = year ?? DateTime.UtcNow.Year;

            var groupSubmissionsDb = await _groupSubmissionService.GetAllGroupSubmissionsByGroupIdAndYearAsync(groupId, _year);

            List<int> submissionsCounts = new();

            if(DateTime.UtcNow.Year == _year)
            {
                for (int i = 1; i <= DateTime.UtcNow.Month; i++)
                {
                    List<int> submissionsCount = new();

                    var groupSubmissionsByMonth = groupSubmissionsDb
                        .Where(gs => gs.Date.Month == i)
                        .ToList();

                    submissionsCounts.Add(groupSubmissionsByMonth.Count);
                }
            }
            else
            {
                for (int i = 1; i <= 12; i++)
                {
                    List<int> submissionsCount = new();

                    var groupSubmissionsByMonth = groupSubmissionsDb
                        .Where(gs => gs.Date.Month == i)
                        .ToList();

                    submissionsCounts.Add(groupSubmissionsByMonth.Count);
                }
            }

            var possibleYears = await _groupSubmissionService.GetPossibleYearsAsync(groupId);

            var submissionCountDto = new SubmissionCountDTO
            {
                Data = submissionsCounts,
                Years = possibleYears,
                CurrentYear = _year
            };

            return Ok(submissionCountDto);
        }

        [HttpGet]
        [Route("{groupId}/{year?}")]
        public async Task<ActionResult> GetAssignmentProgressByGroupIdAndYear(int groupId, int? year)
        {
            int _year = year ?? DateTime.UtcNow.Year;

            List<int?> submissionPercentages = new();

            if (DateTime.UtcNow.Year == _year)
            {
                var possibleSubmissionsCount = await _assignmentAppUserService
                    .GetAllPossibleSubmissionsCountByGroupIdAndYearAsync(DateTime.UtcNow.Month, groupId, _year);

                var submissionsCount = await _assignmentAppUserService
                    .GetAllSubmissionsCountByGroupIdAndYearAsync(DateTime.UtcNow.Month, groupId, _year);

                for (int i = 0; i < possibleSubmissionsCount.Count; i++)
                {
                    int? possibleSubmissionCount = possibleSubmissionsCount[i];

                    if (possibleSubmissionCount is null)
                    {
                        submissionPercentages.Add(null);
                        continue;
                    }

                    int submissionCount = submissionsCount[i];

                    int resultPercentage = (int)Math.Round(submissionCount / (decimal)possibleSubmissionCount*100);

                    submissionPercentages.Add(resultPercentage);
                }
            }
            else
            {
                var possibleSubmissionsCount = await _assignmentAppUserService
                    .GetAllPossibleSubmissionsCountByGroupIdAndYearAsync(12, groupId, _year);

                var submissionsCount = await _assignmentAppUserService
                    .GetAllSubmissionsCountByGroupIdAndYearAsync(12, groupId, _year);

                for (int i = 0; i < possibleSubmissionsCount.Count; i++)
                {
                    int? possibleSubmissionCount = possibleSubmissionsCount[i];

                    if (possibleSubmissionCount is null)
                    {
                        submissionPercentages.Add(null);
                        continue;
                    }

                    int submissionCount = submissionsCount[i];

                    int resultPercentage = (int)Math.Round(submissionCount / (decimal)possibleSubmissionCount * 100);

                    submissionPercentages.Add(resultPercentage);
                }
            }

            var possibleYears = await _assignmentAppUserService.GetPossibleYearsAsync(groupId);

            var groupAssignmentPorgressDto = new GroupAssignmentProgressDTO
            {
                Data = submissionPercentages,
                Years = possibleYears,
                CurrentYear = _year
            };

            return Ok(groupAssignmentPorgressDto);
        }
    }
}