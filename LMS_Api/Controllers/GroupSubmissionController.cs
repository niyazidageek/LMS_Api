using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS_Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GroupSubmissionController : ControllerBase
    {
        private readonly IGroupSubmissionService _groupSubmissionService;

        public GroupSubmissionController(IGroupSubmissionService groupSubmissionService)
        {
            _groupSubmissionService = groupSubmissionService;
        }

        [HttpGet]
        [Route("{groupId}/{year?}")]
        public async Task<ActionResult> GetSubmissionsCountByGroupIdAndYear(int groupId, int? year)
        {
            int _year = year ?? DateTime.UtcNow.Year;

            var groupSubmissionsDb = await _groupSubmissionService.GetAllGroupSubmissionsByGroupIdAndYear(groupId, _year);

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

            return Ok(submissionsCounts);
        }
    }
}