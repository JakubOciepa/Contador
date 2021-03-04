using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Contador.Abstractions;
using Contador.Core.Common;
using Contador.Core.Models;
using Contador.Core.Utils.Extensions;
using Contador.DAL.Abstractions;

namespace Contador.Services
{
	/// <summary>
	/// Notify on expense category changes.
	/// </summary>
	public class ExpenseCategoryService : IExpenseCategoryManager
	{
		private readonly IExpenseCategoryRepository _repository;
		private readonly ILog _logger;

		/// <summary>
		/// Invoked when new(returned) category has been added.
		/// </summary>
		public event EventHandler<ExpenseCategory> CategoryAdded;

		/// <summary>
		/// Invoked when returned category has been updated.
		/// </summary>
		public event EventHandler<ExpenseCategory> CategoryUpdated;

		/// <summary>
		/// Invoked when category of returned id has been removed.
		/// </summary>
		public event EventHandler<int> CategoryRemoved;

		/// <summary>
		/// Creates instance of <see cref="ExpenseCategoryService"/> class.
		/// </summary>
		/// <param name="repository">Expense category repository.</param>
		public ExpenseCategoryService(IExpenseCategoryRepository repository, ILog logger)
		{
			_repository = repository;
			_logger = logger;
		}

		/// <summary>
		/// Gets all available expense categories.
		/// </summary>
		/// <returns><see cref="IList{ExpenseCategory}"/> of all available categories.</returns>
		public async Task<Result<IList<ExpenseCategory>>> GetCategoriesAsync()
		{
			var result = await _repository.GetCategoriesAsync().CAF();

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

		/// <summary>
		/// Gets <see cref="ExpenseCategory"/> by id.
		/// </summary>
		/// <param name="id">Id of requested expense category</param>
		/// <returns>Correct ExpenseCategory or default</returns>
		public async Task<Result<ExpenseCategory>> GetCategoryByIdAsync(int id)
		{
			var result = await _repository.GetCategoryByIdAsync(id).CAF();

			if (result is null)
			{
				_logger.Write(Core.Common.LogLevel.Error, $"Can not find any expense category of the {id}.");
				return new Result<ExpenseCategory>(ResponseCode.NotFound, default);
			}

			return new Result<ExpenseCategory>(ResponseCode.Ok, new ExpenseCategory(result.Name) { Id = result.Id });
		}

		/// <summary>
		/// Adds expense category.
		/// </summary>
		/// <param name="category">Expense category to add.</param>
		/// <returns><see cref="ResponseCode"/> for operation and added category.</returns>
		public async Task<Result<ExpenseCategory>> AddExpenseCategoryAsync(ExpenseCategory category)
		{
			var result = await _repository.AddCategoryAsync(category).CAF();

			if (result is null)
			{
				_logger.Write(Core.Common.LogLevel.Warning, "Can not add expense category.");
				return new Result<ExpenseCategory>(ResponseCode.Error, default);
			}

			CategoryAdded?.Invoke(this, result);

			return new Result<ExpenseCategory>(ResponseCode.Ok, result);
		}

		/// <summary>
		/// Updates expense category of provided id.
		/// </summary>
		/// <param name="id">Id of category to update.</param>
		/// <param name="category">Category info.</param>
		/// <returns>Updated category</returns>
		public async Task<Result<ExpenseCategory>> UpdateExpenseCategoryAsync(int id, ExpenseCategory category)
		{
			var result = await _repository.UpdateCategoryAsync(id, category).CAF();

			if (result is null)
			{
				_logger.Write(Core.Common.LogLevel.Warning, $"Can not update expense category of the {id}.");
				return new Result<ExpenseCategory>(ResponseCode.Error, default);
			}

			CategoryUpdated?.Invoke(this, result);

			return new Result<ExpenseCategory>(ResponseCode.Ok, result);
		}

		/// <summary>
		/// Removes expense category of provided id.
		/// </summary>
		/// <param name="id">Id of category to remove.</param>
		/// <returns><see cref="ResponseCode"/> of operation.</returns>
		public async Task<ResponseCode> RemoveExpenseCategoryAsync(int id)
		{
			var removed = await _repository.RemoveCategoryAsync(id).CAF();

			if (removed)
			{
				CategoryRemoved?.Invoke(this, id);

				return ResponseCode.Ok;
			}

			return ResponseCode.Error;
		}
	}
}
