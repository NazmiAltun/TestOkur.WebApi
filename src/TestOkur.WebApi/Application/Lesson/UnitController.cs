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

    [Route("api/v1/units")]
    [Authorize(AuthorizationPolicies.Customer)]
    public class UnitController : ControllerBase
    {
        private readonly IProcessor _processor;

        public UnitController(IProcessor processor)
        {
            _processor = processor ?? throw new ArgumentNullException(nameof(processor));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync([FromBody, Required]CreateUnitCommand command)
        {
            await _processor.SendAsync(command);
            return Ok();
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyCollection<UnitReadModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _processor.ExecuteAsync<GetUserUnitsQuery, IReadOnlyCollection<UnitReadModel>>(new GetUserUnitsQuery()));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _processor.SendAsync(new DeleteUnitCommand(id));
            return Ok();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> EditAsync([FromBody, Required]EditUnitCommand command)
        {
            await _processor.SendAsync(command);
            return Ok();
        }

        [HttpPost("{unitId}/subjects")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddSubjectAsync(int unitId, [FromBody, Required]AddSubjectCommand command)
        {
            command.UnitId = unitId;
            await _processor.SendAsync(command);
            return Ok();
        }

        [HttpDelete("{unitId}/subjects/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteSubjectAsync(int unitId, int id)
        {
            await _processor.SendAsync(
                new DeleteSubjectCommand(unitId, id));
            return Ok();
        }

        [HttpPut("{unitId}/subjects")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> EditSubjectAsync(int unitId, [FromBody, Required]EditSubjectCommand command)
        {
            command.UnitId = unitId;
            await _processor.SendAsync(command);
            return Ok();
        }
    }
}
