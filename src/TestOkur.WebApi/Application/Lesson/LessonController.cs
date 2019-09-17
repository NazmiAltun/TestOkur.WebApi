namespace TestOkur.WebApi.Application.Lesson
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Paramore.Darker;
    using TestOkur.Common;
    using TestOkur.Infrastructure.Cqrs;
    using TestOkur.WebApi.Application.Lesson.Commands;
    using TestOkur.WebApi.Application.Lesson.Queries;

    [Route("api/v1/lessons")]
    [Authorize(AuthorizationPolicies.Customer)]
    public class LessonController : ControllerBase
    {
        private readonly IProcessor _processor;
        private readonly IQueryProcessor _queryProcessor;

        public LessonController(IQueryProcessor queryProcessor, IProcessor processor)
        {
            _processor = processor ?? throw new ArgumentNullException(nameof(processor));
            _queryProcessor = queryProcessor ?? throw new ArgumentNullException(nameof(queryProcessor));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync([FromBody, Required]CreateLessonCommand command)
        {
            await _processor.SendAsync(command);
            return Ok();
        }

        [HttpGet("shared")]
        [ProducesResponseType(typeof(IReadOnlyCollection<LessonReadModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSharedLessonListAsync()
        {
            return Ok(await _queryProcessor.ExecuteAsync(new GetSharedLessonQuery()));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _processor.SendAsync(new DeleteLessonCommand(id));
            return Ok();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> EditAsync([FromBody, Required]EditLessonCommand command)
        {
            await _processor.SendAsync(command);
            return Ok();
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyCollection<LessonReadModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _processor
                .ExecuteAsync<GetUserLessonsQuery, IReadOnlyCollection<LessonReadModel>>(
                    new GetUserLessonsQuery()));
        }
    }
}
