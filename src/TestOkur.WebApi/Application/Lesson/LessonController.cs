namespace TestOkur.WebApi.Application.Lesson
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Paramore.Brighter;
    using Paramore.Darker;
    using TestOkur.Common;
    using TestOkur.WebApi.Application.Lesson.Commands;
    using TestOkur.WebApi.Application.Lesson.Queries;

    [Route("api/v1/lessons")]
    [Authorize(AuthorizationPolicies.Customer)]
    [ApiController]
    public class LessonController : ControllerBase
    {
        private readonly IAmACommandProcessor _commandProcessor;
        private readonly IQueryProcessor _queryProcessor;

        public LessonController(IQueryProcessor queryProcessor, IAmACommandProcessor commandProcessor)
        {
            _commandProcessor = commandProcessor ?? throw new ArgumentNullException(nameof(commandProcessor));
            _queryProcessor = queryProcessor ?? throw new ArgumentNullException(nameof(queryProcessor));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync(CreateLessonCommand command)
        {
            await _commandProcessor.SendAsync(command);
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
            await _commandProcessor.SendAsync(new DeleteLessonCommand(id));
            return Ok();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> EditAsync(EditLessonCommand command)
        {
            await _commandProcessor.SendAsync(command);
            return Ok();
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyCollection<LessonReadModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _queryProcessor.ExecuteAsync(new GetUserLessonsQuery()));
        }
    }
}
