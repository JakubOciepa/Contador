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
	public class BudgetService
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

		public async Task<Result<Budget>> AddAsync(Budget budget)
		{
			Budget addedBudget;

			try
			{
				//Check if budget exists for the month;

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
