using System.Collections.Generic;
using System.Threading.Tasks;

using Contador.Core.Common;
using Contador.Core.Models;
using Contador.Core.Utils.Extensions;
using Contador.DAL.Repositories;

using Microsoft.Extensions.Logging;

namespace Contador.Web.Server.Services
{
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
        public async Task<Result<IList<ExpenseCategory>>> GetCategories()
        {
            var result = await _repository.GetCategories().CAF();

            if (result.Count == 0)
            {
                _logger.LogWarning("Can not find any expense categories");

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
        public async Task<Result<ExpenseCategory>> GetCategoryById(int id)
        {
            var result = await _repository.GetCategoryById(id).CAF();

            if (result == default)
            {
                _logger.LogWarning($"Can not find any expense category of the {id}.");
                return new Result<ExpenseCategory>(ResponseCode.NotFound, default);
            }

            return new Result<ExpenseCategory>(ResponseCode.Ok, new ExpenseCategory(result.Name) { Id = result.Id });
        }

        /// <inheritdoc/>
        public async Task<Result<ExpenseCategory>> Add(ExpenseCategory category)
        {
            var result = await _repository.Add(new DAL.Models.ExpenseCategory(category.Name)).CAF();

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
