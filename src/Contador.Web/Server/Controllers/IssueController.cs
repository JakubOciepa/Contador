
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Contador.Core.Common;
using Contador.Core.Models;
using Contador.Core.Utils.Extensions;
using Contador.Services.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Contador.Web.Server.Controllers
{
	/// <summary>
	/// Manages Contador issues.
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public class IssueController : Controller
	{
		private readonly IIssueService _issueService;

		/// <summary>
		/// Creates instance of <see cref="IssueController"> class.
		/// </summary>
		/// <param name="expensecategory">Repository of Contador issues.</param>
		public IssueController(IIssueService issueService)
		{
			_issueService= issueService;
		}

		/// <summary>
		/// Gets all available Contador issues.
		/// </summary>
		/// <returns>IList of Contador issues.</returns>
		[HttpGet]
		[ProducesResponseType(typeof(IList<Issue>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<IList<Issue>>> GetAll()
		{
			var result = await _issueService.GetAllAsync().CAF();

			return result.ResponseCode switch
			{
				ResponseCode.NotFound => NoContent(),
				ResponseCode.Error => BadRequest(result.Message),
				ResponseCode.Ok => Ok(result.ReturnedObject),
				_ => BadRequest(@"¯\_(ツ)_/¯ - I really don't know how this happened...")
			};
		}

		/// <summary>
		/// Gets Contador issue of provided id.
		/// </summary>
		/// <param name="id">Id of the requested Contador issue.</param>
		/// <returns>Contador issue of requested id.</returns>
		[HttpGet("{id}")]
		[ProducesResponseType(typeof(ExpenseCategory), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<Issue>> GetById([FromRoute] int id)
		{
			var result = await _issueService.GetAllAsync().CAF();

			return result.ResponseCode switch
			{
				ResponseCode.Error => BadRequest(result.Message),
				ResponseCode.NotFound => NotFound(),
				ResponseCode.Ok => Ok(result.ReturnedObject.Where(i => i.Id == id)),
				_ => BadRequest("ᓚᘏᗢ my oh my...That looks bad.")
			};
		}

		/// <summary>
		/// Adds new Contador issue.
		/// </summary>
		/// <param name="issue">Contador issue to add.</param>
		/// <returns>HTTP code.</returns>
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status409Conflict)]
		public async Task<ActionResult> Add([FromBody] Issue issue)
		{
			if ((await _issueService.GetAllAsync().CAF())
				.ReturnedObject.Any(x => x.Name == issue.Name))
			{
				return Conflict(issue);
			}

			var result = await _issueService.AddAsync(issue).CAF();

			return result.ResponseCode switch
			{
				ResponseCode.Error => BadRequest(result.Message),
				ResponseCode.Ok => CreatedAtAction(nameof(GetById), new { id = result.ReturnedObject.Id }, result.ReturnedObject),
				_ => BadRequest("(T_T) Seriously don't know how this happened..."),
			};
		}
	}
}
