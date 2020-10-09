using System.Collections.Generic;

using Contador.Api.Models;
using Contador.Core.Common;
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
        public Result<ResponseCode, IList<ExpenseCategory>> GetCategories()
        {
            var result = _repository.GetCategories();

            if (result.Count == 0)
            {
                _logger.LogWarning("Can not find any expense categories");
                return new Result<ResponseCode, IList<ExpenseCategory>>(ResponseCode.NotFound, new List<ExpenseCategory>());
            }

            var list = new List<ExpenseCategory>();

            foreach (var category in result)
            {
                list.Add(new ExpenseCategory(category.Name) { Id = category.Id });
            }

            return new Result<ResponseCode, IList<ExpenseCategory>>(ResponseCode.Ok, list);
        }

        /// <inheritdoc/>
        public Result<ResponseCode, ExpenseCategory> GetCategoryById(int id)
        {
            var result = _repository.GetCategoryById(id);

            if (result == default)
            {
                _logger.LogWarning($"Can not find any expense category of the {id}.");
                return new Result<ResponseCode, ExpenseCategory>(ResponseCode.NotFound, default);
            }

            return new Result<ResponseCode, ExpenseCategory>(ResponseCode.Ok,
                new ExpenseCategory(result.Name) { Id = result.Id });
        }

        /// <inheritdoc/>
        public Result<ResponseCode, ExpenseCategory> Add(ExpenseCategory category)
        {
            var result = _repository.Add(new Core.Models.ExpenseCategory(category.Name));

            if (result != default)
            {
                _logger.LogWarning("Can not add expense category.");
                return new Result<ResponseCode, ExpenseCategory>(ResponseCode.Ok,
                    new ExpenseCategory(result.Name) { Id = result.Id });
            }

            return new Result<ResponseCode, ExpenseCategory>(ResponseCode.Error, default);
        }

        /// <inheritdoc/>
        public Result<ResponseCode, ExpenseCategory> Update(int id, ExpenseCategory category)
        {
            var result = _repository.Update(id, new Core.Models.ExpenseCategory(category.Name));

            if (result != default)
            {
                _logger.LogWarning($"Can not update expense category of the {id}.");
                return new Result<ResponseCode, ExpenseCategory>(ResponseCode.Ok,
                    new ExpenseCategory(result.Name) { Id = result.Id });
            }

            return new Result<ResponseCode, ExpenseCategory>(ResponseCode.Error, default);
        }

        /// <inheritdoc/>
        public ResponseCode Remove(int id)
        {
            var result = _repository.Remove(id);

            return result ? ResponseCode.Ok : ResponseCode.Error;
        }
    }
}
