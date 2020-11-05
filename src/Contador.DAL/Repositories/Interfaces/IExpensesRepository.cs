﻿using System.Collections.Generic;
using System.Threading.Tasks;

using Contador.Core.Models;

namespace Contador.DAL.Repositories
{
    /// <summary>
    /// Manages expenses in db.
    /// </summary>
    public interface IExpensesRepository
    {
        /// <summary>
        /// Gets <see cref="ExpenseDto"/> of provided id.
        /// </summary>
        /// <param name="expenseId">Id of requested <see cref="ExpenseDto"/>.</param>
        /// <returns><see cref="ExpenseDto"/> of provided Id.</returns>
        Task<Expense> GetExpense(int expenseId);

        /// <summary>
        /// Gets all available expenses.
        /// </summary>
        /// <returns><see cref="IList{Expense}"/> of all available expenses.</returns>
        Task<IList<Expense>> GetExpenses();

        /// <summary>
        /// Adds provided <see cref="ExpenseDto"/> to storage.
        /// </summary>
        /// <param name="expense">Expense to add.</param>
        /// <returns>Added expense or default</returns>
        Task<Expense> Add(Expense expense);

        /// <summary>
        /// Updates <see cref="ExpenseDto"/> of provided id.
        /// </summary>
        /// <param name="id">Id of expense to update.</param>
        /// <param name="info">Expense info.</param>
        /// <returns>Updated expense or default.</returns>
        Task<Expense> Update(int id, Expense info);

        /// <summary>
        /// Removes <see cref="ExpenseDto"/> of provided id from storage.
        /// </summary>
        /// <param name="id">Id of expense to remove.</param>
        /// <returns>True if removed, false otherwise.</returns>
        Task<bool> Remove(int id);
    }
}