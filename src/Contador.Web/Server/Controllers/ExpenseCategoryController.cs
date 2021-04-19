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
		[ProducesResponseType(typeof(IList<ExpenseCategory>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<IList<ExpenseCategory>>> GetAll()
		{
			var result = await _expenseCategoryService.GetAllAsync().CAF();

			return result.ResponseCode switch
			{
				ResponseCode.NotFound => NoContent(),
				ResponseCode.Error => BadRequest(result.Message),
				ResponseCode.Ok => Ok(result.ReturnedObject),
				_ => BadRequest(@"¯\_(ツ)_/¯ - I really don't know how this happened...")
			};
		}

		/// <summary>
		/// Gets expense category of provided id.
		/// </summary>
		/// <param name="id">Id of the requested expense category.</param>
		/// <returns>Expense category of requested id.</returns>
		[HttpGet("{id}")]
		[ProducesResponseType(typeof(ExpenseCategory), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<ExpenseCategory>> GetById([FromRoute] int id)
		{
			var result = await _expenseCategoryService.GetByIdAsync(id).CAF();

			return result.ResponseCode switch
			{
				ResponseCode.Error => BadRequest(result.Message),
				ResponseCode.NotFound => NotFound(),
				ResponseCode.Ok => Ok(result.ReturnedObject),
				_ => BadRequest("ᓚᘏᗢ my oh my...That looks bad.")
			};
		}

		/// <summary>
		/// Adds new expense category.
		/// </summary>
		/// <param name="category">Expense category to add.</param>
		/// <returns>HTTP code.</returns>
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status409Conflict)]
		public async Task<ActionResult> Add([FromBody] ExpenseCategory category)
		{
			if ((await _expenseCategoryService.GetAllAsync().CAF())
				.ReturnedObject.Any(x => x.Name == category.Name))
			{
				return Conflict(category);
			}

			var result = await _expenseCategoryService.AddAsync(category).CAF();

			return result.ResponseCode switch
			{
				ResponseCode.Error => BadRequest(result.Message),
				ResponseCode.Ok => CreatedAtAction(nameof(GetById), new { id = result.ReturnedObject.Id }, result.ReturnedObject),
				_ => BadRequest("(T_T) Seriously don't know how this happened..."),
			};
		}

		/// <summary>
		/// Updates expense category of provided id.
		/// </summary>
		/// <param name="id">Id of expense category to update.</param>
		/// <param name="category">Expense category info.</param>
		/// <returns>HTTP code.</returns>
		[HttpPut("{id}")]
		[ProducesResponseType(typeof(ExpenseCategory), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> Update([FromRoute] int id, [FromBody] ExpenseCategory category)
		{
			if (!await CategoryOfIdExists(id))
			{
				return NotFound();
			}

			var result = await _expenseCategoryService.UpdateAsync(id, category).CAF();

			return result.ResponseCode switch
			{
				ResponseCode.Ok => Ok(result.ReturnedObject),
				ResponseCode.Error => BadRequest(result.Message),
				_ => BadRequest("(¬_¬ ) You should look into the logs..."),
			};
		}

		/// <summary>
		/// Removes expense category of provided id.
		/// </summary>
		/// <param name="id">Id of expense category to remove.</param>
		/// <returns>HTTP code.</returns>
		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> Remove([FromRoute] int id)
		{
			if (!await CategoryOfIdExists(id))
			{
				return NotFound();
			}

			var result = await _expenseCategoryService.RemoveAsync(id).CAF();

			return result switch
			{
				ResponseCode.Error => BadRequest("Error occurred while removing category."),
				ResponseCode.Ok => Ok(),
				_ => BadRequest("Just check the logs my love.")
			};
		}

		private async Task<bool> CategoryOfIdExists(int id)
		{
			try
			{
				return (await _expenseCategoryService.GetByIdAsync(id).CAF()).ReturnedObject is not null;
			}
			catch
			{
				return false;
			}
		}
	}
}
