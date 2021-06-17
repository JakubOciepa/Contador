using System;
using System.Collections.Generic;
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
		/// Gets collection of available budgets.
		/// </summary>
		/// <returns>List of available.</returns>
		[HttpGet]
		[ProducesResponseType(typeof(Budget), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<List<Budget>>> GetAllBudgets()
		{
			var result = await _budgetService.GetAllBudgetsAsync().CAF();

			return result.ResponseCode switch
			{
				ResponseCode.Error => BadRequest(result.Message),
				ResponseCode.NotFound => NotFound(),
				ResponseCode.Ok => Ok(result.ReturnedObject),
				_ => BadRequest("ᓚᘏᗢ my oh my...That looks bad.")
			};
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
		public async Task<ActionResult<Budget>> GetBudgetById([FromRoute] int id)
		{
			var result = await _budgetService.GetBudgetByIdAsync(id).CAF();

			return result.ResponseCode switch
			{
				ResponseCode.Error => BadRequest(result.Message),
				ResponseCode.NotFound => NotFound(),
				ResponseCode.Ok => Ok(result.ReturnedObject),
				_ => BadRequest("ᓚᘏᗢ my oh my...That looks bad.")
			};
		}

		/// <summary>
		/// Gets category budget of provided id.
		/// </summary>
		/// <param name="id">Id of the requested budget.</param>
		/// <returns>Budget of requested id.</returns>
		[HttpGet("categorybudget/{id}")]
		[ProducesResponseType(typeof(CategoryBudget), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<CategoryBudget>> GetCategoryBudgetById([FromRoute] int id)
		{
			var result = await _budgetService.GetCategoryBudgetByIdAsync(id).CAF();

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
		public async Task<ActionResult> AddBudget([FromBody] BudgetModel budget)
		{
			var result = await _budgetService.AddBudgetAsync(new Budget()
			{
				StartDate = budget.StartDate,
				EndDate = budget.EndDate
			}).CAF();

			return result.ResponseCode switch
			{
				ResponseCode.Error => BadRequest(result.Message),
				ResponseCode.Ok => CreatedAtAction(nameof(GetBudgetById), new { id = result.ReturnedObject.Id }, result.ReturnedObject),
				_ => BadRequest("(T_T) Seriously don't know how this happened..."),
			};
		}

		/// <summary>
		/// Adds new category budget.
		/// </summary>
		/// <param name="category">Budget model to add.</param>
		/// <returns>HTTP code.</returns>
		[HttpPost("categorybudget")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status409Conflict)]
		public async Task<ActionResult> AddCategoryBudget([FromBody] CategoryBudget budget)
		{
			var result = await _budgetService.AddCategoryBudgetAsync(budget).CAF();

			return result.ResponseCode switch
			{
				ResponseCode.Error => BadRequest(result.Message),
				ResponseCode.Ok => CreatedAtAction(nameof(GetCategoryBudgetById), new { id = result.ReturnedObject.Id }, result.ReturnedObject),
				_ => BadRequest("(T_T) Seriously don't know how this happened..."),
			};
		}

		/// <summary>
		/// Updates the budget of the provided id.
		/// </summary>
		/// <param name="id">Id of the budget to update.</param>
		/// <param name="budget">Budget info.</param>
		/// <returns>HTTP code.</returns>
		[HttpPut("{id}")]
		[ProducesResponseType(typeof(Budget), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> UpdateBudget([FromRoute] int id, [FromBody] BudgetModel budget)
		{
			var result = await _budgetService.UpdateBudgetAsync(id, new Budget
			{
				Id = budget.Id,
				StartDate = budget.StartDate,
				EndDate = budget.EndDate
			}).CAF();

			return result.ResponseCode switch
			{
				ResponseCode.NotFound => NotFound(),
				ResponseCode.Ok => Ok(result.ReturnedObject),
				ResponseCode.Error => BadRequest(result.Message),
				_ => BadRequest("(¬_¬ ) You should look into the logs..."),
			};
		}
		/// <summary>
		/// Updates the category budget of the provided id.
		/// </summary>
		/// <param name="id">Id of the category budget to update.</param>
		/// <param name="budget">Category budget info.</param>
		/// <returns>HTTP code.</returns>
		[HttpPut("categorybudget/{id}")]
		[ProducesResponseType(typeof(Expense), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> UpdateCategoryBudget([FromRoute] int id, [FromBody] CategoryBudget budget)
		{
			var result = await _budgetService.UpdateCategoryBudgetAsync(id, budget).CAF();

			return result.ResponseCode switch
			{
				ResponseCode.NotFound => NotFound(),
				ResponseCode.Ok => Ok(result.ReturnedObject),
				ResponseCode.Error => BadRequest(result.Message),
				_ => BadRequest("(¬_¬ ) You should look into the logs..."),
			};
		}

		/// <summary>
		/// Removes the budget of the provided id and its category budgets.
		/// </summary>
		/// <param name="id">Id of the budget to remove.</param>
		/// <returns>HTTP code.</returns>
		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> RemoveBudget([FromRoute] int id)
		{
			var result = await _budgetService.RemoveBudgetAsync(id).CAF();

			return result switch
			{
				ResponseCode.NotFound => NotFound(),
				ResponseCode.Error => BadRequest("Error occurred while removing budget."),
				ResponseCode.Ok => Ok(),
				_ => BadRequest("Just check the logs my love.")
			};
		}

		/// <summary>
		/// Removes the category budget of the provided id.
		/// </summary>
		/// <param name="id">Id of the category budget to remove.</param>
		/// <returns>HTTP code.</returns>
		[HttpDelete("categorybudget/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> RemoveCategoryBudget([FromRoute] int id)
		{
			var result = await _budgetService.RemoveCategoryBudgetAsync(id).CAF();

			return result switch
			{
				ResponseCode.NotFound => NotFound(),
				ResponseCode.Error => BadRequest("Error occurred while removing category budget."),
				ResponseCode.Ok => Ok(),
				_ => BadRequest("Just check the logs my love.")
			};
		}

		/// <summary>
		/// Contains info about Budget for API.
		/// </summary>
		public class BudgetModel
		{
			/// <summary>
			/// Id of the Budget.
			/// </summary>
			public int Id { get; set; }

			/// <summary>
			/// Start date of the budget.
			/// </summary>
			public DateTime StartDate { get; set; }

			/// <summary>
			/// End date of the budget.
			/// </summary>
			public DateTime EndDate { get; set; }
		}
	}
}
