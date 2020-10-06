using Contador.Core.Models;
using System.Collections.Generic;

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
    }
}