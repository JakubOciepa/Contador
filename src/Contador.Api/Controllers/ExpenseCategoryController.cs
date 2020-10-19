using System.Collections.Generic;
using System.Threading.Tasks;

using Contador.Api.Services;
using Contador.Core.Common;
using Contador.Core.Models;

using Microsoft.AspNetCore.Mvc;

namespace Contador.Api.Controllers
{
    /// <summary>
    /// Expense controller.
    /// </summary>
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
        [HttpGet("expensecategory")]
        [ProducesResponseType(typeof(IList<ExpenseCategory>), 200)]
        public async Task<ActionResult<IList<ExpenseCategory>>> GetExpenseCategories()
        {
            var result = await _expenseCategoryService.GetCategories().ConfigureAwait(false);

            if ((ResponseCode)result.ResponseCode == ResponseCode.NotFound)
            {
                return BadRequest("No expense category found");
            }

            return Ok(result.ReturnedObject);
        }

        /// <summary>
        /// Gets expense category of provided id.
        /// </summary>
        /// <param name="id">Id of the requested expense category.</param>
        /// <returns>Expense category of requested id.</returns>
        [HttpGet("expensecategory/{id}")]
        public async Task<ActionResult<ExpenseCategory>> GetExpenseCategory([FromRoute] int id)
        {
            var result = await Task.Run(() => _expenseCategoryService.GetCategoryById(id)).ConfigureAwait(false);
            if ((ResponseCode)result.ResponseCode == ResponseCode.NotFound)
            {
                return BadRequest("Expense category not found.");
            }

            return Ok(result.ReturnedObject);
        }

        /// <summary>
        /// Adds new expense category.
        /// </summary>
        /// <param name="category">Expense category to add.</param>
        /// <returns>Http code.</returns>
        [HttpPost("expensecategory")]
        public async Task<ActionResult> AddExpenseCategory(ExpenseCategory category)
        {
            var result = await _expenseCategoryService.Add(category);

            if ((ResponseCode)result.ResponseCode == ResponseCode.Ok)
            {
                return Ok(result.ReturnedObject);
            }

            return BadRequest("Error occured while saving expense category.");
        }

        /// <summary>
        /// Updates expense category of provided id.
        /// </summary>
        /// <param name="id">Id of expense category to update.</param>
        /// <param name="category">Expense category info.</param>
        /// <returns>Http code.</returns>
        [HttpPut("expensecategory/{id}")]
        public ActionResult UpdateExpense([FromRoute] int id, ExpenseCategory category)
        {
            var result = _expenseCategoryService.Update(id, category);

            if ((ResponseCode)result.ResponseCode == ResponseCode.Ok)
            {
                return Ok(result.ReturnedObject);
            }

            return BadRequest("Error occured while updating expense category.");
        }

        /// <summary>
        /// Removes expense category of provided id.
        /// </summary>
        /// <param name="id">Id of expense category to remove.</param>
        /// <returns>Http code.</returns>
        [HttpDelete("expensecategory/{id}")]
        public ActionResult RemoveExpense([FromRoute] int id)
        {
            var result = _expenseCategoryService.Remove(id);

            if (result == ResponseCode.Ok)
            {
                return Ok();
            }

            return BadRequest("Error occured while removing expense category.");
        }
    }
}
