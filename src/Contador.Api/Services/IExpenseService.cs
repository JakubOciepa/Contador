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
    }
}