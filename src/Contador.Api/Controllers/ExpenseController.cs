using System.Collections.Generic;
using System.Threading.Tasks;

using Contador.Api.Services;
using Contador.Core.Common;
using Contador.Core.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Contador.Api.Controllers
{
    /// <summary>
    /// Expense controller.
    /// </summary>
    [Route("/api/[controller]")]
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
        [ProducesResponseType(typeof(IList<ExpenseCategory>), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IList<Expense>>> GetExpenses()
        {
            var result = await _expenseService.GetExpenses().ConfigureAwait(false);

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
        [ProducesResponseType(typeof(ExpenseCategory), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Expense>> GetExpense([FromRoute] int id)
        {
            var result = await _expenseService.GetExpense(id).ConfigureAwait(false);

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
        /// <returns>Http code.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddExpense([FromBody] Expense expense)
        {
            var result = await _expenseService.Add(expense).ConfigureAwait(false);

            if ((ResponseCode)result.ResponseCode == ResponseCode.Ok)
            {
                return CreatedAtAction(nameof(GetExpense),
                    new { id = result.ReturnedObject.Id }, result.ReturnedObject);
            }

            return BadRequest("Error occured while saving expense.");
        }

        /// <summary>
        /// Updates expense of provided id.
        /// </summary>
        /// <param name="id">Id of expense to update.</param>
        /// <param name="expense">Expense info.</param>
        /// <returns>Http code.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ExpenseCategory), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateExpense([FromRoute] int id, [FromBody] Expense expense)
        {
            if ((await _expenseService.GetExpense(id).ConfigureAwait(false))
                    .ReturnedObject == null)
            {
                NotFound(expense);
            }

            var result = await _expenseService.Update(id, expense).ConfigureAwait(false);

            if ((ResponseCode)result.ResponseCode == ResponseCode.Ok)
            {
                return Ok(result.ReturnedObject);
            }

            return BadRequest("Error occured while updating expense.");
        }

        /// <summary>
        /// Removes expense of provided id.
        /// </summary>
        /// <param name="id">Id of expense to remove.</param>
        /// <returns>Http code.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> RemoveExpense([FromRoute] int id)
        {
            if ((await _expenseService.GetExpense(id).ConfigureAwait(false))
                    .ReturnedObject == null)
            {
                NotFound(id);
            }

            var result = await _expenseService.Remove(id).ConfigureAwait(false);

            if (result == ResponseCode.Ok)
            {
                return Ok();
            }

            return BadRequest("Error occured while removing expense.");
        }
    }
}
