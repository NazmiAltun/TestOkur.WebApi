namespace TestOkur.WebApi.Application.Exam
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using TestOkur.Common;
    using TestOkur.Infrastructure.Cqrs;
    using TestOkur.WebApi.Application.Exam.Commands;
    using TestOkur.WebApi.Application.Exam.Queries;

    [Route("api/v1/exams")]
    [Authorize(AuthorizationPolicies.Customer)]
    public class ExamController : ControllerBase
    {
        private readonly IProcessor _processor;

        public ExamController(IProcessor processor)
        {
            _processor = processor ?? throw new ArgumentNullException(nameof(processor));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync([FromBody, Required]CreateExamCommand command)
        {
            await _processor.SendAsync(command);
            return Ok();
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyCollection<ExamReadModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _processor.ExecuteAsync<GetUserExamsQuery, IReadOnlyCollection<ExamReadModel>>(new GetUserExamsQuery()));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _processor.SendAsync(new DeleteExamCommand(id));
            return Ok();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> EditAsync([FromBody, Required]EditExamCommand command)
        {
            await _processor.SendAsync(command);
            return Ok();
        }
    }
}
