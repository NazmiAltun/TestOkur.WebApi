namespace TestOkur.WebApi.Application.Classroom
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Paramore.Brighter;
    using Paramore.Darker;
    using TestOkur.Common;
    using TestOkur.Domain;

    [Route("api/v1/classrooms")]
    [Authorize(AuthorizationPolicies.Customer)]
    [ApiController]
    public class ClassroomController : ControllerBase
    {
        private readonly IAmACommandProcessor _commandProcessor;
        private readonly IQueryProcessor _queryProcessor;

        public ClassroomController(IAmACommandProcessor commandProcessor, IQueryProcessor queryProcessor)
        {
            _commandProcessor = commandProcessor;
            _queryProcessor = queryProcessor;
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync(CreateClassroomCommand command)
        {
            await _commandProcessor.SendAsync(command);
            return Ok(SuccessCodes.ClassCreated);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyCollection<ClassroomReadModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _queryProcessor.ExecuteAsync(new GetUserClassroomsQuery()));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _commandProcessor.SendAsync(new DeleteClassroomCommand(id));
            return Ok();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> EditAsync(EditClassroomCommand command)
        {
            await _commandProcessor.SendAsync(command);
            return Ok();
        }
    }
}
