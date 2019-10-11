namespace TestOkur.WebApi.Application.Classroom
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using TestOkur.Common;
    using TestOkur.Domain;
    using TestOkur.Infrastructure.CommandsQueries;

    [Route("api/v1/classrooms")]
    [Authorize(AuthorizationPolicies.Customer)]
    public class ClassroomController : ControllerBase
    {
        private readonly IProcessor _processor;

        public ClassroomController(IProcessor processor)
        {
            _processor = processor ?? throw new ArgumentNullException(nameof(processor));
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync([FromBody, Required]CreateClassroomCommand command)
        {
            await _processor.SendAsync(command);
            return Ok(SuccessCodes.ClassCreated);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyCollection<ClassroomReadModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _processor.ExecuteAsync<GetUserClassroomsQuery, IReadOnlyCollection<ClassroomReadModel>>(new GetUserClassroomsQuery()));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _processor.SendAsync(new DeleteClassroomCommand(id));
            return Ok();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> EditAsync([FromBody, Required]EditClassroomCommand command)
        {
            await _processor.SendAsync(command);
            return Ok();
        }
    }
}
