using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Contador.Core.Common;
using Contador.Core.Models;
using Contador.Core.Utils.Extensions;
using Contador.DAL.Abstractions;
using Contador.Services.Interfaces;

namespace Contador.Services
{
	/// <summary>
	/// Notify on expense category changes.
	/// </summary>
	public class ExpenseCategoryManager : IExpenseCategoryManager
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
		/// Creates instance of <see cref="ExpenseCategoryManager"/> class.
		/// </summary>
		/// <param name="repository">Expense category repository.</param>
		/// <param name="logger">Logger</param>
		public ExpenseCategoryManager(IExpenseCategoryRepository repository, ILog logger)
		{
			_repository = repository;
			_logger = logger;
		}

		/// <summary>
		/// Gets all available expense categories.
		/// </summary>
		/// <returns><see cref="IList{ExpenseCategory}"/> of all available categories.</returns>
		public async Task<Result<IList<ExpenseCategory>>> GetAllAsync()
		{
			List<ExpenseCategory> categories;

			try
			{
				categories = await _repository.GetAllAsync().CAF() as List<ExpenseCategory>;
			}
			catch (Exception ex)
			{
				_logger.Write(LogLevel.Error, $"{ex}");

				return new Result<IList<ExpenseCategory>>(ResponseCode.Error, new List<ExpenseCategory>()) { Message = $"{ex}" };
			}

			if (categories?.Count is 0)
			{
				_logger.Write(LogLevel.Warning, "Categories not found.");

				return new Result<IList<ExpenseCategory>>(ResponseCode.NotFound, new List<ExpenseCategory>());
			}

			return new Result<IList<ExpenseCategory>>(ResponseCode.Ok, categories);
		}

		/// <summary>
		/// Gets <see cref="ExpenseCategory"/> by id.
		/// </summary>
		/// <param name="id">Id of requested expense category</param>
		/// <returns>Correct ExpenseCategory or default</returns>
		public async Task<Result<ExpenseCategory>> GetByIdAsync(int id)
		{
			ExpenseCategory category;

			try
			{
				category = await _repository.GetByIdAsync(id).CAF();
			}
			catch (Exception ex)
			{
				_logger.Write(LogLevel.Error, $"{ex.Message}\n{ex.StackTrace}");

				return new Result<ExpenseCategory>(ResponseCode.Error, null) { Message = ex.Message };
			}

			if (category is null)
			{
				_logger.Write(LogLevel.Warning, $"Category of the {id} not found.");

				return new Result<ExpenseCategory>(ResponseCode.NotFound, null);
			}

			return new Result<ExpenseCategory>(ResponseCode.Ok, category);
		}

		/// <summary>
		/// Adds expense category.
		/// </summary>
		/// <param name="category">Expense category to add.</param>
		/// <returns><see cref="ResponseCode"/> for operation and added category.</returns>
		public async Task<Result<ExpenseCategory>> AddAsync(ExpenseCategory category)
		{
			ExpenseCategory addedCategory;

			try
			{
				addedCategory = await _repository.AddAsync(category).CAF();
			}
			catch (Exception ex)
			{
				_logger.Write(LogLevel.Error, $"{ex.Message}\n{ex.StackTrace}");

				return new Result<ExpenseCategory>(ResponseCode.Error, null) { Message = ex.Message };
			}

			if (addedCategory is null)
			{
				var error = "Cannot add the category";
				_logger.Write(LogLevel.Warning, error);

				return new Result<ExpenseCategory>(ResponseCode.Error, null) { Message = error };
			}

			CategoryAdded?.Invoke(this, addedCategory);

			return new Result<ExpenseCategory>(ResponseCode.Ok, addedCategory);
		}

		/// <summary>
		/// Updates expense category of provided id.
		/// </summary>
		/// <param name="id">Id of category to update.</param>
		/// <param name="category">Category info.</param>
		/// <returns>Updated category</returns>
		public async Task<Result<ExpenseCategory>> UpdateAsync(int id, ExpenseCategory category)
		{
			ExpenseCategory updated;

			try
			{
				updated = await _repository.UpdateAsync(id, category).CAF();
			}
			catch (Exception ex)
			{
				var message = $"{ex.Message}\n{ex.StackTrace}";

				return new Result<ExpenseCategory>(ResponseCode.Error, null) { Message = message };
			}

			if (updated is null)
			{
				var message = $"Cannot update the category of the {id}.";
				_logger.Write(LogLevel.Warning, message);

				return new Result<ExpenseCategory>(ResponseCode.Error, null) { Message = message };
			}

			CategoryUpdated?.Invoke(this, updated);

			return new Result<ExpenseCategory>(ResponseCode.Ok, updated);
		}

		/// <summary>
		/// Removes expense category of provided id.
		/// </summary>
		/// <param name="id">Id of category to remove.</param>
		/// <returns><see cref="ResponseCode"/> of operation.</returns>
		public async Task<ResponseCode> RemoveAsync(int id)
		{
			bool result;
			try
			{
				result = await _repository.RemoveAsync(id).CAF();
			}
			catch (Exception ex)
			{
				_logger.Write(LogLevel.Error, $"{ex.Message}\n{ex.StackTrace}");

				return ResponseCode.Error;
			}

			CategoryRemoved?.Invoke(this, id);

			return result ? ResponseCode.Ok : ResponseCode.Error;
		}
	}
}
