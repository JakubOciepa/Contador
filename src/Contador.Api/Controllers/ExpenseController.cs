using Contador.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Contador.Api.Controllers
{
    /// <summary>
    /// Expense controller.
    /// </summary>
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        /// <summary>
        /// Gets all available expense.
        /// </summary>
        /// <returns>IEnumerable of expenses.</returns>
        [HttpGet("expenses")]
        public ActionResult<IEnumerable<Expense>> GetExpenses()
        {
            return new[]
            {
                new Expense("Marysia", 123, 0,"słodycze"),
                new Expense("Marysia", 123, 0,"słodycze"),
                new Expense("Marysia", 123, 0,"słodycze"),
            };
        }

        /// <summary>
        /// Gets expense of provided id.
        /// </summary>
        /// <param name="id">Id of the requested expense.</param>
        /// <returns>Expense of requested id.</returns>
        [HttpGet("expenses/{id}")]
        public ActionResult<Expense> GetExpense(int id)
        {
            return new Expense("Marysia", 123, 0, "słodycze");
        }

        /// <summary>
        /// Creates new expense and add it to db.
        /// </summary>
        /// <param name="expense">Expense to add.</param>
        /// <returns>Http code.</returns>
        [HttpPost("expenses")]
        public ActionResult CreateExpense(Expense expense)
        {
            return Ok();
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
            return Ok();
        }

        /// <summary>
        /// Removes expense of provided id.
        /// </summary>
        /// <param name="id">Id of expense to remove.</param>
        /// <returns>Http code.</returns>
        [HttpDelete("expense/{id}")]
        public ActionResult RemoveExpense(int id)
        {
            return Ok();
        }
    }
}
