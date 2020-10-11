﻿using System.Collections.Generic;

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
        /// Adds provided <see cref="Expense"/> into storage.
        /// </summary>
        /// <param name="expense">Expense to add.</param>
        /// <returns>Correct <see cref="ResponseCode"/> for operation and added expense.</returns>
        Result<ResponseCode, Expense> Add(Expense expense);

        /// <summary>
        /// Updates <see cref="Expense"/> of provided id.
        /// </summary>
        /// <param name="id">Id of expense to update.</param>
        /// <param name="expense">Expense info.</param>
        /// <returns>Correct <see cref="ResponseCode"/> for operation and updated expense.</returns>
        Result<ResponseCode, Expense> Update(int id, Expense expense);

        /// <summary>
        /// Removes <see cref="Expense"/> of provided id.
        /// </summary>
        /// <param name="id">Id of expense to remove.</param>
        /// <returns>Correct <see cref="ResponseCode"/> for operation</returns>
        ResponseCode Remove(int id);
    }
}