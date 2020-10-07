using Contador.Api.Models;
using Contador.Core.Common;
using System.Collections.Generic;

namespace Contador.Api.Services
{
    /// <summary>
    /// Expense manager.
    /// </summary>
    public interface IExpenseService
    {
        /// <summary>
        /// Gets all available expenses.
        /// </summary>
        /// <returns>Result wich proper response code and list of expenses.</returns>
        Result<ResponseCode, IList<Expense>> GetExpenses();

        /// <summary>
        /// Gets <see cref="Expense"/> of provided id.
        /// </summary>
        /// <param name="id">Id of requested Epxense.</param>
        /// <returns><see cref="Expense"/> of provided id.</returns>
        Result<ResponseCode, Expense> GetExpense(int id);
    }
}