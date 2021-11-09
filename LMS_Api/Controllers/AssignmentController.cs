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
    public class AssignmentController : ControllerBase
    {
        private readonly IAssignmentService _assignmentService;
        private readonly IAssignmentAppUserService _assignmentAppUserService;
        private readonly IAssignmentMaterialService _assignmentMaterialService;

        public AssignmentController(IAssignmentService assignmentService,
            IAssignmentAppUserService assignmentAppUserService,
            IAssignmentMaterialService assignmentMaterialService)
        {
            _assignmentService = assignmentService;
            _assignmentAppUserService = assignmentAppUserService;
            _assignmentMaterialService = assignmentMaterialService;
        }

        [HttpPost]
        public async Task
    }
}