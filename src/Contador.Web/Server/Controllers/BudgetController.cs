using System;
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
	/// Provides functions to manage budgets.
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public class BudgetController : Controller
	{
		private readonly IBudgetService _budgetService;

		/// <summary>
		/// Creates an instance of the <see cref="BudgetController"/> class.
		/// </summary>
		/// <param name="budgetService">Service to manage budgets.</param>
		public BudgetController(IBudgetService budgetService)
		{
			_budgetService = budgetService;
		}

		/// <summary>
		/// Gets budget of provided id.
		/// </summary>
		/// <param name="id">Id of the requested budget.</param>
		/// <returns>Budget of requested id.</returns>
		[HttpGet("{id}")]
		[ProducesResponseType(typeof(Budget), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<Budget>> GetById([FromRoute] int id)
		{
			var result = await _budgetService.GetByIdAsync(id).CAF();

			return result.ResponseCode switch
			{
				ResponseCode.Error => BadRequest(result.Message),
				ResponseCode.NotFound => NotFound(),
				ResponseCode.Ok => Ok(result.ReturnedObject),
				_ => BadRequest("ᓚᘏᗢ my oh my...That looks bad.")
			};
		}

		/// <summary>
		/// Adds new budget.
		/// </summary>
		/// <param name="category">Budget model to add.</param>
		/// <returns>HTTP code.</returns>
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status409Conflict)]
		public async Task<ActionResult> Add([FromBody] BudgetModel budget)
		{
			var result = await _budgetService.AddAsync(new Budget()
			{
				StartDate = budget.StartDate,
				EndDate  = budget.EndDate
			}).CAF();

			return result.ResponseCode switch
			{
				ResponseCode.Error => BadRequest(result.Message),
				ResponseCode.Ok => CreatedAtAction(nameof(GetById), new { id = result.ReturnedObject.Id }, result.ReturnedObject),
				_ => BadRequest("(T_T) Seriously don't know how this happened..."),
			};
		}

		public class BudgetModel
		{
			public int Id { get; set; }
			public DateTime StartDate { get; set; }
			public DateTime EndDate { get; set; }
		}
	}
}
