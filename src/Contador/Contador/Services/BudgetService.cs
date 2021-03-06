﻿using System;
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
	/// Manages budgets.
	/// </summary>
	public class BudgetService : IBudgetService
	{
		private IBudgetRepository _repository;
		private ILog _logger;

		/// <summary>
		/// Creates an instance of the <see cref="BudgetService"/> class.
		/// </summary>
		/// <param name="repository"></param>
		public BudgetService(IBudgetRepository repository, ILog logger)
		{
			_repository = repository;
			_logger = logger;
		}

		/// <inheritdoc/>
		public async Task<Result<List<Budget>>> GetAllBudgetsAsync()
		{
			var list = new List<Budget>();

			try
			{
				await foreach (var budget in _repository.GetAllBudgetsAsync())
				{
					list.Add(budget);
				}
			}
			catch (Exception ex)
			{
				_logger.Write(LogLevel.Error, $"{ex.Message}\n{ex.StackTrace}");

				return new Result<List<Budget>>(ResponseCode.Error, null) { Message = ex.Message };
			}

			if (list.Count is 0)
			{
				_logger.Write(LogLevel.Warning, $"No available budgets found.");

				return new Result<List<Budget>>(ResponseCode.NotFound, null);
			}

			return new Result<List<Budget>>(ResponseCode.Ok, list);
		}

		/// <inheritdoc/>
		public async Task<Result<Budget>> GetBudgetByIdAsync(int id)
		{
			Budget budget;

			try
			{
				budget = await _repository.GetBudgetByIdAsync(id).CAF();
			}
			catch (Exception ex)
			{
				_logger.Write(LogLevel.Error, $"{ex.Message}\n{ex.StackTrace}");

				return new Result<Budget>(ResponseCode.Error, null) { Message = ex.Message };
			}

			if (budget is null)
			{
				_logger.Write(LogLevel.Warning, $"Budget of the {id} not found.");

				return new Result<Budget>(ResponseCode.NotFound, null);
			}

			return new Result<Budget>(ResponseCode.Ok, budget);
		}

		/// <inheritdoc/>
		public async Task<Result<Budget>> AddBudgetAsync(Budget budget)
		{
			Budget addedBudget;

			try
			{
				if (await _repository.GetBudgetByStartDateAsync(budget.StartDate) is object)
				{
					return new Result<Budget>(ResponseCode.Error, null) { Message = "Budget already exists!" };
				}

				addedBudget = await _repository.AddBudgetAsync(budget).CAF();
			}
			catch (Exception ex)
			{
				_logger.Write(LogLevel.Error, $"{ex.Message}\n{ex.StackTrace}");

				return new Result<Budget>(ResponseCode.Error, null) { Message = ex.Message };
			}

			if (addedBudget is null)
			{
				var error = "Cannot add the budget";
				_logger.Write(LogLevel.Warning, error);

				return new Result<Budget>(ResponseCode.Error, null) { Message = error };
			}

			return new Result<Budget>(ResponseCode.Ok, addedBudget);
		}

		/// <inheritdoc/>
		public async Task<Result<CategoryBudget>> AddCategoryBudgetAsync(CategoryBudget categoryBudget)
		{
			CategoryBudget addedBudget;

			try
			{
				if (await _repository.GetCategoryBudgetByCategoryAndBudgetIdAsync(categoryBudget.BudgetId, categoryBudget.CategoryId) is object a)
				{
					return new Result<CategoryBudget>(ResponseCode.Error, null) { Message = "Budget already exists!" };
				}

				addedBudget = await _repository.AddCategoryBudgetAsync(categoryBudget).CAF();
			}
			catch (Exception ex)
			{
				_logger.Write(LogLevel.Error, $"{ex.Message}\n{ex.StackTrace}");

				return new Result<CategoryBudget>(ResponseCode.Error, null) { Message = ex.Message };
			}

			if (addedBudget is null)
			{
				var error = "Cannot add the budget";
				_logger.Write(LogLevel.Warning, error);

				return new Result<CategoryBudget>(ResponseCode.Error, null) { Message = error };
			}

			return new Result<CategoryBudget>(ResponseCode.Ok, addedBudget);
		}

		/// <inheritdoc/>
		public async Task<Result<CategoryBudget>> GetCategoryBudgetByIdAsync(int id)
		{
			CategoryBudget budget;

			try
			{
				budget = await _repository.GetCategoryBudgetByIdAsync(id).CAF();
			}
			catch (Exception ex)
			{
				_logger.Write(LogLevel.Error, $"{ex.Message}\n{ex.StackTrace}");

				return new Result<CategoryBudget>(ResponseCode.Error, null) { Message = ex.Message };
			}

			if (budget is null)
			{
				_logger.Write(LogLevel.Warning, $"Budget of the {id} not found.");

				return new Result<CategoryBudget>(ResponseCode.NotFound, null);
			}

			return new Result<CategoryBudget>(ResponseCode.Ok, budget);
		}

		/// <inheritdoc/>
		public async Task<ResponseCode> RemoveBudgetAsync(int id)
		{
			bool result;
			try
			{
				if (await _repository.GetBudgetByIdAsync(id) is null)
				{
					return ResponseCode.NotFound;
				}

				result = await _repository.RemoveBudgetAsync(id).CAF();
			}
			catch (Exception ex)
			{
				_logger.Write(LogLevel.Error, $"{ex.Message}\n{ex.StackTrace}");

				return ResponseCode.Error;
			}

			return result ? ResponseCode.Ok : ResponseCode.Error;
		}

		/// <inheritdoc/>
		public async Task<Result<Budget>> UpdateBudgetAsync(int id, Budget budget)
		{
			Budget updated;

			try
			{
				if (await _repository.GetBudgetByIdAsync(id) is null)
				{
					return new Result<Budget>(ResponseCode.NotFound, null);
				}

				updated = await _repository.UpdateBudgetAsync(id, budget).CAF();
			}
			catch (Exception ex)
			{
				var message = $"{ex.Message}\n{ex.StackTrace}";

				return new Result<Budget>(ResponseCode.Error, null) { Message = message };
			}

			if (updated is null)
			{
				var message = $"Cannot update the budget of the {id}.";
				_logger.Write(LogLevel.Warning, message);

				return new Result<Budget>(ResponseCode.Error, null) { Message = message };
			}

			return new Result<Budget>(ResponseCode.Ok, updated);
		}

		/// <inheritdoc/>
		public async Task<Result<CategoryBudget>> UpdateCategoryBudgetAsync(int id, CategoryBudget budget)
		{
			CategoryBudget updated;

			try
			{
				if (await _repository.GetCategoryBudgetByIdAsync(id) is null)
				{
					return new Result<CategoryBudget>(ResponseCode.NotFound, null);
				}

				updated = await _repository.UpdateCategoryBudgetAsync(id, budget).CAF();
			}
			catch (Exception ex)
			{
				var message = $"{ex.Message}\n{ex.StackTrace}";

				return new Result<CategoryBudget>(ResponseCode.Error, null) { Message = message };
			}

			if (updated is null)
			{
				var message = $"Cannot update the category budget of the {id}.";
				_logger.Write(LogLevel.Warning, message);

				return new Result<CategoryBudget>(ResponseCode.Error, null) { Message = message };
			}

			return new Result<CategoryBudget>(ResponseCode.Ok, updated);
		}

		/// <inheritdoc/>
		public async Task<ResponseCode> RemoveCategoryBudgetAsync(int id)
		{
			bool result;
			try
			{
				if (await _repository.GetCategoryBudgetByIdAsync(id) is null)
				{
					return ResponseCode.NotFound;
				}

				result = await _repository.RemoveCategoryBudgetAsync(id).CAF();
			}
			catch (Exception ex)
			{
				_logger.Write(LogLevel.Error, $"{ex.Message}\n{ex.StackTrace}");

				return ResponseCode.Error;
			}

			return result ? ResponseCode.Ok : ResponseCode.Error;
		}
	}
}
