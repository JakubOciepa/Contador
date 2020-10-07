using Contador.Api.Services.Interfaces;
using Contador.Core.Models;
using Contador.DAL.Repositories;

namespace Contador.Api.Services
{
    /// <inheritdoc/>
    public class ExpenseCategoryService : IExpenseCategoryService
    {
        private readonly IExpenseCategoryRepository _repository;

        /// <summary>
        /// Creates instance of <see cref="ExpenseCategoryService"/> class.
        /// </summary>
        /// <param name="repository">Expense category repository.</param>
        public ExpenseCategoryService(IExpenseCategoryRepository repository)
        {
            _repository = repository;
        }

        /// <inheritdoc/>
        public ExpenseCategory GetCategoryById(int id)
        {
            return _repository.GetCategoryById(id);
        }
    }
}
