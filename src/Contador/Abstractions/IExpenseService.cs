﻿using System.Collections.Generic;
using System.Threading.Tasks;

using Contador.Core.Common;
using Contador.Core.Models;

namespace Contador.Abstractions
{
    /// <summary>
    /// Expense manager.
    /// </summary>
    public interface IExpenseService
    {
        /// <summary>
        /// Gets all available expenses.
        /// </summary>
        /// <returns>Result which proper response code and list of expenses.</returns>
        Task<Result<IList<Expense>>> GetExpensesAsync();

        /// <summary>
        /// Gets <see cref="Expense"/> of provided id.
        /// </summary>
        /// <param name="id">Id of requested Expense.</param>
        /// <returns><see cref="Expense"/> of provided id.</returns>
        Task<Result<Expense>> GetExpenseAsync(int id);

        /// <summary>
        /// Adds provided <see cref="Expense"/> into storage.
        /// </summary>
        /// <param name="expense">Expense to add.</param>
        /// <returns>Correct <see cref="ResponseCode"/> for operation and added expense.</returns>
        Task<Result<Expense>> AddAsync(Expense expense);

        /// <summary>
        /// Updates <see cref="Expense"/> of provided id.
        /// </summary>
        /// <param name="id">Id of expense to update.</param>
        /// <param name="expense">Expense info.</param>
        /// <returns>Correct <see cref="ResponseCode"/> for operation and updated expense.</returns>
        Task<Result<Expense>> UpdateAsync(int id, Expense expense);

        /// <summary>
        /// Removes <see cref="Expense"/> of provided id.
        /// </summary>
        /// <param name="id">Id of expense to remove.</param>
        /// <returns>Correct <see cref="ResponseCode"/> for operation</returns>
        Task<ResponseCode> RemoveAsync(int id);
    }
}
