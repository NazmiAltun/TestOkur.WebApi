namespace TestOkur.WebApi.Application.Score
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using TestOkur.Common;
    using TestOkur.Infrastructure.Cqrs;

    [Route("api/v1/score-formulas")]
    [Authorize(AuthorizationPolicies.Customer)]
    public class ScoreFormulaController : ControllerBase
    {
        private readonly IProcessor _processor;

        public ScoreFormulaController(IProcessor processor)
        {
            _processor = processor ?? throw new ArgumentNullException(nameof(processor));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyCollection<ScoreFormulaReadModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync()
        {
            var list = await _processor
                .ExecuteAsync<GetUserScoreFormulasQuery, IReadOnlyCollection<ScoreFormulaReadModel>>(
                new GetUserScoreFormulasQuery());

            if (list.Any())
            {
                return Ok(list);
            }

            await _processor.SendAsync(new CloneScoreFormulaCommand());
            list = await _processor
                .ExecuteAsync<GetUserScoreFormulasQuery, IReadOnlyCollection<ScoreFormulaReadModel>>(
                    new GetUserScoreFormulasQuery());

            return Ok(list);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateAsync([FromBody, Required] BulkEditScoreFormulaCommand command)
        {
            await _processor.SendAsync(command);
            return Ok();
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _processor.SendAsync(new DeleteUserScoreFormulasCommand());
            return Ok();
        }
    }
}
