using System;
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

		/// <summary>
		/// Gets <see cref="Budget"/> by id.
		/// </summary>
		/// <param name="id">Id of requested budget.</param>
		/// <returns>Correct budget or default</returns>
		public async Task<Result<Budget>> GetByIdAsync(int id)
		{
			Budget budget;

			try
			{
				budget = await _repository.GetByIdAsync(id).CAF();
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

		/// <summary>
		/// Adds the budget to the repository.
		/// </summary>
		/// <param name="budget">Budget to add.</param>
		/// <returns>Added budget.</returns>
		public async Task<Result<Budget>> AddAsync(Budget budget)
		{
			Budget addedBudget;

			try
			{
				if (_repository.GetByStartDateAsync(budget.StartDate) is object)
				{
					return new Result<Budget>(ResponseCode.Error, null) { Message = "Budget already exists!" };
				}

				addedBudget = await _repository.AddAsync(budget).CAF();
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
	}
}
