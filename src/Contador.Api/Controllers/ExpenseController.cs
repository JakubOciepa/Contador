using System.Collections.Generic;

using Contador.Api.Services;
using Contador.Core.Common;
using Contador.Core.Models;

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
        [HttpGet("expense")]
        public ActionResult<IList<Expense>> GetExpenses()
        {
            var result = _expenseService.GetExpenses();

            if ((ResponseCode)result.ResponseCode == ResponseCode.NotFound)
            {
                return BadRequest("No expense found");
            }

            return Ok(result.ReturnedObject);
        }

        /// <summary>
        /// Gets expense of provided id.
        /// </summary>
        /// <param name="id">Id of the requested expense.</param>
        /// <returns>Expense of requested id.</returns>
        [HttpGet("expense/{id}")]
        public ActionResult<Expense> GetExpense([FromRoute] int id)
        {
            var result = _expenseService.GetExpense(id);

            if ((ResponseCode)result.ResponseCode == ResponseCode.NotFound)
            {
                return BadRequest("Expense not found.");
            }

            return Ok(result.ReturnedObject);
        }

        /// <summary>
        /// Adds new expense.
        /// </summary>
        /// <param name="expense">Expense to add.</param>
        /// <returns>Http code.</returns>
        [HttpPost("expense")]
        public ActionResult AddExpense(Expense expense)
        {
            var result = _expenseService.Add(expense);

            if ((ResponseCode)result.ResponseCode == ResponseCode.Ok)
            {
                return Ok(result.ReturnedObject);
            }

            return BadRequest("Error occured while saving expense.");
        }

        /// <summary>
        /// Updates expense of provided id.
        /// </summary>
        /// <param name="id">Id of expense to update.</param>
        /// <param name="expense">Expense info.</param>
        /// <returns>Http code.</returns>
        [HttpPut("expense/{id}")]
        public ActionResult UpdateExpense([FromRoute]int id, Expense expense)
        {
            var result = _expenseService.Update(id, expense);

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
        [HttpDelete("expense/{id}")]
        public ActionResult RemoveExpense([FromRoute] int id)
        {
            var result = _expenseService.Remove(id);

            if (result == ResponseCode.Ok)
            {
                return Ok();
            }

            return BadRequest("Error occured while removing expense.");
        }
    }
}
