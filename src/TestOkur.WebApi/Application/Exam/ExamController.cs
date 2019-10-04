namespace TestOkur.WebApi.Application.Exam
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using MassTransit;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using TestOkur.Common;
    using TestOkur.Infrastructure.Cqrs;
    using TestOkur.WebApi.Application.Exam.Commands;
    using TestOkur.WebApi.Application.Exam.Queries;

    [Route("api/v1/exams")]
    public class ExamController : ControllerBase
    {
        private readonly IProcessor _processor;
        private readonly IPublishEndpoint _publishEndpoint;

        public ExamController(IProcessor processor, IPublishEndpoint publishEndpoint)
        {
            _processor = processor ?? throw new ArgumentNullException(nameof(processor));
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        [Authorize(AuthorizationPolicies.Customer)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync([FromBody, Required]CreateExamCommand command)
        {
            if (!User.IsInRole(Roles.Admin))
            {
                command.Shared = false;
            }

            await _processor.SendAsync(command);
            return Ok();
        }

        [HttpPost("re-evaluate")]
        [Authorize(AuthorizationPolicies.Private)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<IActionResult> ReEvaluateAsync()
        {
            var examIds = await _processor.ExecuteAsync<GetAllExamIdsQuery, IEnumerable<int>>(
                  new GetAllExamIdsQuery());
            await _publishEndpoint.Publish(new ReEvaluateMultipleExams(examIds));
            return Accepted();
        }

        [HttpGet]
        [Authorize(AuthorizationPolicies.Customer)]
        [ProducesResponseType(typeof(IReadOnlyCollection<ExamReadModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _processor.ExecuteAsync<GetUserExamsQuery, IReadOnlyCollection<ExamReadModel>>(new GetUserExamsQuery()));
        }

        [HttpDelete("{id}")]
        [Authorize(AuthorizationPolicies.Customer)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _processor.SendAsync(new DeleteExamCommand(id));
            return Ok();
        }

        [HttpPut]
        [Authorize(AuthorizationPolicies.Customer)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> EditAsync([FromBody, Required]EditExamCommand command)
        {
            if (!User.IsInRole(Roles.Admin))
            {
                command.Shared = false;
            }

            await _processor.SendAsync(command);
            return Ok();
        }
    }
}
