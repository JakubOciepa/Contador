using System.Collections.Generic;

using Contador.Api.Models;
using Contador.Core.Common;

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

        /// <summary>
        /// Adds provided expense into storage.
        /// </summary>
        /// <param name="expense">Expense to add.</param>
        /// <returns>Correct <see cref="ResponseCode"/> for operation and added expense.</returns>
        Result<ResponseCode, Expense> Add(Expense expense);
    }
}