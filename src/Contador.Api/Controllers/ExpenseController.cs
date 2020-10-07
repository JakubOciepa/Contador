using System.Collections.Generic;

using Contador.Api.Models;
using Contador.Api.Services;
using Contador.Core.Common;

using Microsoft.AspNetCore.Mvc;

namespace Contador.Api.Controllers
{
    /// <summary>
    /// Expense controller.
    /// </summary>
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;

        /// <summary>
        /// Creates instance of <see cref="ExpenseController> class.
        /// </summary>
        /// <param name="expenses">Reposirory of expenses.</param>
        public ExpenseController(IExpenseService expenses)
        {
            _expenseService = expenses;
        }

        /// <summary>
        /// Gets all available expenses.
        /// </summary>
        /// <returns>IEnumerable of expenses.</returns>
        [HttpGet("expenses")]
        public ActionResult<IList<Expense>> GetExpenses()
        {
            var result = _expenseService.GetExpenses();

            if (result.ResponseCode == Core.Common.ResponseCode.NotFound)
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
        [HttpGet("expenses/{id}")]
        public ActionResult<Expense> GetExpense(int id)
        {
            var result = _expenseService.GetExpense(id);

            if (result.ResponseCode == Core.Common.ResponseCode.NotFound)
            {
                return BadRequest("Expense not found.");
            }

            return Ok(result.ReturnedObject);
        }

        /// <summary>
        /// Adds new expense and adds it to db.
        /// </summary>
        /// <param name="expense">Expense to add.</param>
        /// <returns>Http code.</returns>
        [HttpPost("expenses")]
        public ActionResult AddExpense(Expense expense)
        {
            var result = _expenseService.Add(expense);

            if (result.ResponseCode == ResponseCode.Ok)
            {
                return Ok(result.ReturnedObject);
            }

            return BadRequest("Error occured while saving expense.");
        }

        /// <summary>
        /// Updates expense of provided id.
        /// </summary>
        /// <param name="id">Id of expense to update.</param>
        /// <param name="expense">Data of expense to update.</param>
        /// <returns>Http code.</returns>
        [HttpPut("expense/{id}")]
        public ActionResult UpdateExpense(int id, Expense expense)
        {
            var result = _expenseService.Update(id, expense);

            if (result.ResponseCode == ResponseCode.Ok)
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
        public ActionResult RemoveExpense(int id)
        {
            throw new System.Exception();
        }
    }
}
