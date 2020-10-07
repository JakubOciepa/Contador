using System.Collections.Generic;

using Contador.Core.Models;

namespace Contador.DAL.Repositories.Interfaces
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
        IList<Expense> GetExpenses();

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
    }
}