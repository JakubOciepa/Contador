using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Contador.Abstractions;
using Contador.Core.Common;
using Contador.Core.Models;
using Contador.Core.Utils.Extensions;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Contador.Web.Server.Controllers
{
	/// <summary>
	/// Expense controller.
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public class ExpenseCategoryController : ControllerBase
	{
		private readonly IExpenseCategoryService _expenseCategoryService;

		/// <summary>
		/// Creates instance of <see cref="ExpenseCategoryController"> class.
		/// </summary>
		/// <param name="expensecategory">Repository of expense categories.</param>
		public ExpenseCategoryController(IExpenseCategoryService expensecategory)
		{
			_expenseCategoryService = expensecategory;
		}

		/// <summary>
		/// Gets all available expense categories.
		/// </summary>
		/// <returns>IList of expense categories.</returns>
		[HttpGet]
		[ProducesResponseType(typeof(IList<ExpenseCategory>), 200)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<IList<ExpenseCategory>>> GetExpenseCategories()
		{
			var result = await _expenseCategoryService.GetCategoriesAsync().CAF();

			if ((ResponseCode)result.ResponseCode == ResponseCode.NotFound)
			{
				return NotFound("No expense category found");
			}

			return Ok(result.ReturnedObject);
		}

		/// <summary>
		/// Gets expense category of provided id.
		/// </summary>
		/// <param name="id">Id of the requested expense category.</param>
		/// <returns>Expense category of requested id.</returns>
		[HttpGet("{id}")]
		[ProducesResponseType(typeof(ExpenseCategory), 200)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ExpenseCategory>> GetExpenseCategory([FromRoute] int id)
		{
			var result = await _expenseCategoryService.GetCategoryByIdAsync(id).CAF();

			if ((ResponseCode)result.ResponseCode == ResponseCode.NotFound)
			{
				return NotFound("Expense category not found.");
			}

			return Ok(result.ReturnedObject);
		}

		/// <summary>
		/// Adds new expense category.
		/// </summary>
		/// <param name="category">Expense category to add.</param>
		/// <returns>HTTP code.</returns>
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status409Conflict)]
		public async Task<ActionResult> AddExpenseCategory([FromBody] ExpenseCategory category)
		{
			if ((await _expenseCategoryService.GetCategoriesAsync().CAF())
				.ReturnedObject.Any(x => x.Name == category.Name))
			{
				return Conflict(category);
			}

			var result = await _expenseCategoryService.AddExpenseCategoryAsync(category).CAF();

			if ((ResponseCode)result.ResponseCode == ResponseCode.Ok)
			{
				return CreatedAtAction(nameof(GetExpenseCategory),
					new { id = result.ReturnedObject.Id }, result.ReturnedObject);
			}

			return BadRequest("Error occurred while saving expense category.");
		}

		/// <summary>
		/// Updates expense category of provided id.
		/// </summary>
		/// <param name="id">Id of expense category to update.</param>
		/// <param name="category">Expense category info.</param>
		/// <returns>HTTP code.</returns>
		[HttpPut("{id}")]
		[ProducesResponseType(typeof(ExpenseCategory), 200)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> UpdateExpense([FromRoute] int id, [FromBody] ExpenseCategory category)
		{
			if ((await _expenseCategoryService.GetCategoriesAsync().CAF())
				.ReturnedObject.All(x => x.Name != category.Name))
			{
				return NotFound(category);
			}

			var result = await _expenseCategoryService.UpdateExpenseCategoryAsync(id, category).CAF();

			if ((ResponseCode)result.ResponseCode == ResponseCode.Ok)
			{
				return Ok(result.ReturnedObject);
			}

			return BadRequest("Error occurred while updating expense category.");
		}

		/// <summary>
		/// Removes expense category of provided id.
		/// </summary>
		/// <param name="id">Id of expense category to remove.</param>
		/// <returns>HTTP code.</returns>
		/// [HttpGet("{id}")]
		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> RemoveExpense([FromRoute] int id)
		{
			if ((await _expenseCategoryService.GetCategoriesAsync().CAF())
				.ReturnedObject.All(x => x.Id != id))
			{
				return NotFound(id);
			}

			var result = await _expenseCategoryService.RemoveExpenseCategoryAsync(id).CAF();

			if (result == ResponseCode.Ok)
			{
				return Ok();
			}

			return BadRequest("Error occurred while removing expense category.");
		}
	}
}
