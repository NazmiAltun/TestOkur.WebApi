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
	using Paramore.Brighter;
	using Paramore.Darker;
	using TestOkur.Common;
	using TestOkur.Domain.Model.ScoreModel;

	[Route("api/v1/score-formulas")]
	[Authorize(AuthorizationPolicies.Customer)]
	public class ScoreFormulaController : Controller
	{
		private readonly IAmACommandProcessor _commandProcessor;
		private readonly IQueryProcessor _queryProcessor;

		public ScoreFormulaController(IQueryProcessor queryProcessor, IAmACommandProcessor commandProcessor)
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

			await _commandProcessor.SendAsync(new CloneScoreFormulaCommand());
			list = await _queryProcessor.ExecuteAsync(new GetUserScoreFormulasQuery());

			return Ok(list);
		}

		[HttpPut]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> UpdateAsync([FromBody, Required] BulkEditScoreFormulaCommand command)
		{
			await _commandProcessor.SendAsync(command);
			return Ok();
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> SaveAsync([FromBody, Required] SaveExamScoreFormulaCommand command)
		{
			await _commandProcessor.SendAsync(command);
			return Ok();
		}
	}
}
