using System.Collections.Generic;
using System.Threading.Tasks;

using Contador.Core.Common;
using Contador.Core.Models;
using Contador.DAL.Repositories;

using Microsoft.Extensions.Logging;

namespace Contador.Api.Services
{
    /// <inheritdoc/>
    public class ExpenseCategoryService : IExpenseCategoryService
    {
        private readonly IExpenseCategoryRepository _repository;
        private readonly ILogger<ExpenseCategoryService> _logger;

        /// <summary>
        /// Creates instance of <see cref="ExpenseCategoryService"/> class.
        /// </summary>
        /// <param name="repository">Expense category repository.</param>
        public ExpenseCategoryService(IExpenseCategoryRepository repository, ILogger<ExpenseCategoryService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <inheritdoc/>
        public Task<Result<IList<ExpenseCategory>>> GetCategories()
        {
            var result = _repository.GetCategories();

            if (result.Count == 0)
            {
                _logger.LogWarning("Can not find any expense categories");

                return new Task<Result<IList<ExpenseCategory>>>(()
                    => new Result<IList<ExpenseCategory>>(ResponseCode.NotFound, new List<ExpenseCategory>()));
            }

            var list = new List<ExpenseCategory>();

            foreach (var category in result)
            {
                list.Add(new ExpenseCategory(category.Name) { Id = category.Id });
            }

            return new Task<Result<IList<ExpenseCategory>>>(()
                => new Result<IList<ExpenseCategory>>(ResponseCode.Ok, list));
        }

        /// <inheritdoc/>
        public Task<Result<ExpenseCategory>> GetCategoryById(int id)
        {
            var result = _repository.GetCategoryById(id);

            if (result == default)
            {
                _logger.LogWarning($"Can not find any expense category of the {id}.");
                return new Task<Result<ExpenseCategory>>(
                    () => new Result<ExpenseCategory>(ResponseCode.NotFound, default));
            }

            return new Task<Result<ExpenseCategory>>(() => new Result<ExpenseCategory>(ResponseCode.Ok,
                new ExpenseCategory(result.Name) { Id = result.Id }));
        }

        /// <inheritdoc/>
        public Result<ExpenseCategory> Add(ExpenseCategory category)
        {
            var result = _repository.Add(new DAL.Models.ExpenseCategory(category.Name));

            if (result != default)
            {
                _logger.LogWarning("Can not add expense category.");
                return new Result<ExpenseCategory>(ResponseCode.Ok,
                    new ExpenseCategory(result.Name) { Id = result.Id });
            }

            return new Result<ExpenseCategory>(ResponseCode.Error, default);
        }

        /// <inheritdoc/>
        public Result<ExpenseCategory> Update(int id, ExpenseCategory category)
        {
            var result = _repository.Update(id, new DAL.Models.ExpenseCategory(category.Name));

            if (result != default)
            {
                _logger.LogWarning($"Can not update expense category of the {id}.");
                return new Result<ExpenseCategory>(ResponseCode.Ok,
                    new ExpenseCategory(result.Name) { Id = result.Id });
            }

            return new Result<ExpenseCategory>(ResponseCode.Error, default);
        }

        /// <inheritdoc/>
        public ResponseCode Remove(int id)
        {
            var result = _repository.Remove(id);

            return result ? ResponseCode.Ok : ResponseCode.Error;
        }
    }
}
