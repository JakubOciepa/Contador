﻿using System.Collections.Generic;
using System.Threading.Tasks;

using Contador.DAL.Models;

namespace Contador.DAL.Repositories
{
    /// <summary>
    /// Manages expenses in db.
    /// </summary>
    public interface IExpensesRepository
    {
        /// <summary>
        /// Gets <see cref="Expense"/> of provided id.
        /// </summary>
        /// <param name="expenseId">Id of requested <see cref="Expense"/>.</param>
        /// <returns><see cref="Expense"/> of provided Id.</returns>
        Expense GetExpense(int expenseId);

        /// <summary>
        /// Gets all available expenses.
        /// </summary>
        /// <returns><see cref="IList{Expense}"/> of all available expenses.</returns>
        Task<IList<Expense>> GetExpenses();

        /// <summary>
        /// Adds provided <see cref="Expense"/> to storage.
        /// </summary>
        /// <param name="expense">Expense to add.</param>
        /// <returns>Added expense or default</returns>
        Expense Add(Expense expense);

        /// <summary>
        /// Updates <see cref="Expense"/> of provided id.
        /// </summary>
        /// <param name="id">Id of expense to update.</param>
        /// <param name="info">Expense info.</param>
        /// <returns>Updated expense or default.</returns>
        Expense Update(int id, Expense info);

        /// <summary>
        /// Removes <see cref="Expense"/> of provided id from storage.
        /// </summary>
        /// <param name="id">Id of expense to remove.</param>
        /// <returns>True if removed, false otherwise.</returns>
        bool Remove(int id);
    }
}