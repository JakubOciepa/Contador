using System;
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
	/// Provides methods to get, add, update or remove expense.
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public class ExpenseController : ControllerBase
	{
		private readonly IExpenseService _expenseService;

		/// <summary>
		/// Creates the instance of the <see cref="ExpenseController"> class.
		/// </summary>
		/// <param name="expenses">Repository of expenses.</param>
		public ExpenseController(IExpenseService expenses)
		{
			_expenseService = expenses;
		}

		/// <summary>
		/// Gets all available expenses.
		/// </summary>
		/// <returns><see cref="IList{Expense}"/> of the expenses.</returns>
		[HttpGet]
		[ProducesResponseType(typeof(IList<Expense>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<IList<Expense>>> GetAll()
		{
			var result = await _expenseService.GetAllAsync().CAF();

			return result.ResponseCode switch
			{
				ResponseCode.NotFound => NoContent(),
				ResponseCode.Error => BadRequest(result.Message),
				ResponseCode.Ok => Ok(result.ReturnedObject),
				_ => BadRequest(@"¯\_(ツ)_/¯ - I really don't know how this happened...")
			};
		}

		/// <summary>
		/// Gets expense by the provided id.
		/// </summary>
		/// <param name="id">Id of the requested expense.</param>
		/// <returns>Expense of the requested id.</returns>
		[HttpGet("{id}")]
		[ProducesResponseType(typeof(Expense), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<Expense>> GetById([FromRoute] int id)
		{
			var result = await _expenseService.GetByIdAsync(id).CAF();

			return result.ResponseCode switch
			{
				ResponseCode.Error => BadRequest(result.Message),
				ResponseCode.NotFound => NotFound(),
				ResponseCode.Ok => Ok(result.ReturnedObject),
				_ => BadRequest("ᓚᘏᗢ my oh my...That looks bad.")
			};
		}

		/// <summary>
		/// Gets provided count or less of latest expenses.
		/// </summary>
		/// <param name="count">Amount of expenses to return.</param>
		/// <returns>Provided count or less of the latest </returns>
		[HttpGet("latest")]
		[ProducesResponseType(typeof(Expense), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<IList<Expense>>> GetLatest([FromQuery] int count = 0)
		{
			var result = await _expenseService.GetLatest(count).CAF();

			return result.ResponseCode switch
			{
				ResponseCode.NotFound => NoContent(),
				ResponseCode.Error => BadRequest(result.Message),
				ResponseCode.Ok => Ok((result.ReturnedObject)),
				_ => BadRequest(@"Not enough mana...")
			};
		}

		/// <summary>
		/// Gets the filtered expenses by the provided values.
		/// </summary>
		/// <param name="name">A part of the name of the expense.</param>
		/// <param name="categoryName">A part of the expense category name.</param>
		/// <param name="userName">A part of the user name.</param>
		/// <param name="createDateFrom">Minimal date of the expense creation.</param>
		/// <param name="createDateTo">Maximum date of the expense creation.</param>
		/// <returns></returns>
		[HttpGet("filter")]
		public async Task<ActionResult<IList<Expense>>> GetFiltered(
			[FromQuery] string name = null,
			[FromQuery] string categoryName = null,
			[FromQuery] string userName = "",
			[FromQuery] DateTime createDateFrom = default,
			[FromQuery] DateTime createDateTo = default)
		{
			var result = await _expenseService.GetFiltered(name, categoryName, userName, createDateFrom, createDateTo);

			return result.ResponseCode switch
			{
				ResponseCode.NotFound => NoContent(),
				ResponseCode.Error => BadRequest(result.Message),
				ResponseCode.Ok => Ok((result.ReturnedObject)),
				_ => BadRequest(@"Not enough mana...")
			};

			return null;
		}

		/// <summary>
		/// Adds new expense.
		/// </summary>
		/// <param name="expense">Expense to add.</param>
		/// <returns>HTTP code with created expense if all succeeded.</returns>
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status409Conflict)]
		public async Task<ActionResult> Add([FromBody] Expense expense)
		{
			if ((await _expenseService.GetAllAsync().CAF())
				.ReturnedObject
					.Where(e => e.CreateDate.DayOfYear == expense.CreateDate.DayOfYear
							&& e.CreateDate.Year == expense.CreateDate.Year)
						.Any(e => e.Equals(expense)))
			{
				return Conflict(expense);
			}

			var result = await _expenseService.AddAsync(expense).CAF();

			return result.ResponseCode switch
			{
				ResponseCode.Error => BadRequest(result.Message),
				ResponseCode.Ok => CreatedAtAction(nameof(GetById), new { id = result.ReturnedObject.Id }, result.ReturnedObject),
				_ => BadRequest("(T_T) Seriously don't know how this happened..."),
			};
		}

		/// <summary>
		/// Updates the expense of the provided id.
		/// </summary>
		/// <param name="id">Id of the expense to update.</param>
		/// <param name="expense">Expense info.</param>
		/// <returns>HTTP code.</returns>
		[HttpPut("{id}")]
		[ProducesResponseType(typeof(Expense), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> Update([FromRoute] int id, [FromBody] Expense expense)
		{
			if (!await ExpenseOfIdExists(id))
			{
				return NotFound();
			}

			var result = await _expenseService.UpdateAsync(id, expense).CAF();

			return result.ResponseCode switch
			{
				ResponseCode.Ok => Ok(result.ReturnedObject),
				ResponseCode.Error => BadRequest(result.Message),
				_ => BadRequest("(¬_¬ ) You should look into the logs..."),
			};
		}

		/// <summary>
		/// Removes the expense of the provided id.
		/// </summary>
		/// <param name="id">Id of the expense to remove.</param>
		/// <returns>HTTP code.</returns>
		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> Remove([FromRoute] int id)
		{
			if (!await ExpenseOfIdExists(id))
			{
				return NotFound();
			}

			var result = await _expenseService.RemoveAsync(id).CAF();

			return result switch
			{
				ResponseCode.Error => BadRequest("Error occurred while removing expense."),
				ResponseCode.Ok => Ok(),
				_ => BadRequest("Just check the logs my love.")
			};
		}

		private async Task<bool> ExpenseOfIdExists(int id)
		{
			try
			{
				return (await _expenseService.GetByIdAsync(id).CAF()).ReturnedObject is not null;
			}
			catch
			{
				return false;
			}
		}
	}
}
