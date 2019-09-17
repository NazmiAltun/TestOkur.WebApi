﻿namespace TestOkur.WebApi.Application.Score
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Paramore.Darker;
    using TestOkur.Common;
    using TestOkur.Infrastructure.Cqrs;

    [Route("api/v1/score-formulas")]
    [Authorize(AuthorizationPolicies.Customer)]
    public class ScoreFormulaController : ControllerBase
	{
		private readonly IContextCommandProcessor _commandProcessor;
		private readonly IQueryProcessor _queryProcessor;

		public ScoreFormulaController(IQueryProcessor queryProcessor, IContextCommandProcessor commandProcessor)
		{
			_commandProcessor = commandProcessor ?? throw new ArgumentNullException(nameof(commandProcessor));
			_queryProcessor = queryProcessor ?? throw new ArgumentNullException(nameof(queryProcessor));
		}

		[HttpGet]
		[ProducesResponseType(typeof(IReadOnlyCollection<ScoreFormulaReadModel>), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetAsync()
		{
			var list = await _queryProcessor.ExecuteAsync(new GetUserScoreFormulasQuery());

			if (list.Any())
			{
				return Ok(list);
			}

			await _commandProcessor.ExecuteAsync(new CloneScoreFormulaCommand());
			list = await _queryProcessor.ExecuteAsync(new GetUserScoreFormulasQuery());

			return Ok(list);
		}

		[HttpPut]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> UpdateAsync([FromBody, Required] BulkEditScoreFormulaCommand command)
		{
			await _commandProcessor.ExecuteAsync(command);
			return Ok();
		}

		[HttpDelete]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> DeleteAsync(int id)
		{
			await _commandProcessor.ExecuteAsync(new DeleteUserScoreFormulasCommand());
			return Ok();
		}
	}
}
