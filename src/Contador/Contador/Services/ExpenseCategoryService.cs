using System.Collections.Generic;
using System.Threading.Tasks;

using Contador.Abstractions;
using Contador.Core.Common;
using Contador.Core.Models;
using Contador.Core.Utils.Extensions;
using Contador.DAL.Abstractions;

namespace Contador.Services
{
    /// <inheritdoc/>
    public class ExpenseCategoryService : IExpenseCategoryService
    {
        private readonly IExpenseCategoryRepository _repository;
        private readonly ILog _logger;

        /// <summary>
        /// Creates instance of <see cref="ExpenseCategoryService"/> class.
        /// </summary>
        /// <param name="repository">Expense category repository.</param>
        public ExpenseCategoryService(IExpenseCategoryRepository repository, ILog logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<Result<IList<ExpenseCategory>>> GetCategoriesAsync()
        {
            var result = await _repository.GetCategories().CAF();

            if (result.Count == 0)
            {
                _logger.Write(Core.Common.LogLevel.Error, "Can not find any expense categories");

                return new Result<IList<ExpenseCategory>>(ResponseCode.NotFound, new List<ExpenseCategory>());
            }

            var list = new List<ExpenseCategory>();

            foreach (var category in result)
            {
                list.Add(new ExpenseCategory(category.Name) { Id = category.Id });
            }

            return new Result<IList<ExpenseCategory>>(ResponseCode.Ok, list);
        }

        /// <inheritdoc/>
        public async Task<Result<ExpenseCategory>> GetCategoryByIdAsync(int id)
        {
            var result = await _repository.GetCategoryById(id).CAF();

            if (result == default)
            {
                _logger.Write(Core.Common.LogLevel.Error, $"Can not find any expense category of the {id}.");
                return new Result<ExpenseCategory>(ResponseCode.NotFound, default);
            }

            return new Result<ExpenseCategory>(ResponseCode.Ok, new ExpenseCategory(result.Name) { Id = result.Id });
        }

        /// <inheritdoc/>
        public async Task<Result<ExpenseCategory>> AddExpenseAsync(ExpenseCategory category)
        {
            var result = await _repository.Add(category).CAF();

            if (result != default)
            {
                _logger.Write(Core.Common.LogLevel.Warning, "Can not add expense category.");
                return new Result<ExpenseCategory>(ResponseCode.Ok,
                    new ExpenseCategory(result.Name) { Id = result.Id });
            }

            return new Result<ExpenseCategory>(ResponseCode.Error, default);
        }

        /// <inheritdoc/>
        public async Task<Result<ExpenseCategory>> UpdateExpenseAsync(int id, ExpenseCategory category)
        {
            var result = await _repository.Update(id, category).CAF();

            if (result != default)
            {
                _logger.Write(Core.Common.LogLevel.Warning, $"Can not update expense category of the {id}.");
                return new Result<ExpenseCategory>(ResponseCode.Ok,
                    new ExpenseCategory(result.Name) { Id = result.Id });
            }

            return new Result<ExpenseCategory>(ResponseCode.Error, default);
        }

        /// <inheritdoc/>
        public async Task<ResponseCode> RemoveExpenseAsync(int id)
        {
            var result = await _repository.Remove(id).CAF();

            return result ? ResponseCode.Ok : ResponseCode.Error;
        }
    }
}
