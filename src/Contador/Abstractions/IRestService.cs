using System.Threading.Tasks;

using Contador.Core.Common;
using Contador.Core.Models;

namespace Contador.Abstractions
{
    /// <summary>
    /// Service which consumes REST API requests from Contador REST API.
    /// </summary>
    public interface IRestService
    {
        /// <summary>
        /// Gets expense from API by provided id.
        /// </summary>
        /// <param name="id">Id of desired expense.</param>
        /// <returns>Expense of provided id or <see cref="default"/>.</returns>
        Task<Result<Expense>> GetExpenseByIdAsync(int id);
    }
}
