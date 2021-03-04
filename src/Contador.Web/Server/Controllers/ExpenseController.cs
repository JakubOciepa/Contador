using System.Collections.Generic;
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
	public class ExpenseController : ControllerBase
	{
		private readonly IExpenseService _expenseService;

		/// <summary>
		/// Creates instance of <see cref="ExpenseController> class.
		/// </summary>
		/// <param name="expenses">Repository of expenses.</param>
		public ExpenseController(IExpenseService expenses)
		{
			_expenseService = expenses;
		}

		/// <summary>
		/// Gets all available expenses.
		/// </summary>
		/// <returns>IList of expenses.</returns>
		[HttpGet]
		[ProducesResponseType(typeof(IList<Expense>), 200)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<IList<Expense>>> GetExpenses()
		{
			var result = await _expenseService.GetExpensesAsync().CAF();

			if ((ResponseCode)result.ResponseCode == ResponseCode.NotFound)
			{
				return NotFound("No expense found");
			}

			return Ok(result.ReturnedObject);
		}

		/// <summary>
		/// Gets expense of provided id.
		/// </summary>
		/// <param name="id">Id of the requested expense.</param>
		/// <returns>Expense of requested id.</returns>
		[HttpGet("{id}")]
		[ProducesResponseType(typeof(Expense), 200)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<Expense>> GetExpense([FromRoute] int id)
		{
			var result = await _expenseService.GetExpenseAsync(id).CAF();

			if ((ResponseCode)result.ResponseCode == ResponseCode.NotFound)
			{
				return NotFound("Expense not found.");
			}

			return Ok(result.ReturnedObject);
		}

		/// <summary>
		/// Adds new expense.
		/// </summary>
		/// <param name="expense">Expense to add.</param>
		/// <returns>HTTP code.</returns>
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> AddExpense([FromBody] Expense expense)
		{
			var result = await _expenseService.AddAsync(expense).CAF();

			if ((ResponseCode)result.ResponseCode == ResponseCode.Ok)
			{
				return CreatedAtAction(nameof(GetExpense),
					new { id = result.ReturnedObject.Id }, result.ReturnedObject);
			}

			return BadRequest("Error occurred while saving expense.");
		}

		/// <summary>
		/// Updates expense of provided id.
		/// </summary>
		/// <param name="id">Id of expense to update.</param>
		/// <param name="expense">Expense info.</param>
		/// <returns>HTTP code.</returns>
		[HttpPut("{id}")]
		[ProducesResponseType(typeof(Expense), 200)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> UpdateExpense([FromRoute] int id, [FromBody] Expense expense)
		{
			if ((await _expenseService.GetExpenseAsync(id).CAF())
					.ReturnedObject == null)
			{
				NotFound(expense);
			}

			var result = await _expenseService.UpdateAsync(id, expense).CAF();

			if ((ResponseCode)result.ResponseCode == ResponseCode.Ok)
			{
				return Ok(result.ReturnedObject);
			}

			return BadRequest("Error occurred while updating expense.");
		}

		/// <summary>
		/// Removes expense of provided id.
		/// </summary>
		/// <param name="id">Id of expense to remove.</param>
		/// <returns>HTTP code.</returns>
		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> RemoveExpense([FromRoute] int id)
		{
			if ((await _expenseService.GetExpenseAsync(id).CAF())
					.ReturnedObject == null)
			{
				NotFound(id);
			}

			var result = await _expenseService.RemoveAsync(id).CAF();

			if (result == ResponseCode.Ok)
			{
				return Ok();
			}

			return BadRequest("Error occurred while removing expense.");
		}
	}
}
