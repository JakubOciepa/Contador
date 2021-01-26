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
	/// <inheritdoc/>
	public class ExpenseService : IExpenseService
	{
		private readonly IExpenseRepository _expenseRepo;
		private readonly ILog _logger;

		public event EventHandler<Expense> ExpenseAdded;
		public event EventHandler<Expense> ExpenseUpdated;
		public event EventHandler<int> ExpenseRemoved;

		/// <summary>
		/// Creates instance of <see cref="ExpenseService"/> class.
		/// </summary>
		/// <param name="expenses">Repository of expenses.</param>
		public ExpenseService(IExpenseRepository expenses, ILog logger)
		{
			_expenseRepo = expenses;
			_logger = logger;
		}

		/// <inheritdoc/>
		public async Task<Result<IList<Expense>>> GetExpensesAsync()
		{
			var result = await _expenseRepo.GetExpensesAsync().CAF();

			if (result?.Count is 0)
			{
				_logger.Write(Core.Common.LogLevel.Warning, "Expenses not found.");
				return new Result<IList<Expense>>(ResponseCode.NotFound, new List<Expense>());
			}

			var list = new List<Expense>();

			foreach (var expense in result)
			{
				list.Add(expense);
			}

			return new Result<IList<Expense>>(ResponseCode.Ok, list);
		}

		/// <inheritdoc/>
		public async Task<Result<Expense>> GetExpenseAsync(int id)
		{
			var result = await _expenseRepo.GetExpenseAsync(id).CAF();

			if (result is null)
			{
				_logger.Write(Core.Common.LogLevel.Warning, $"Expense of the {id} not found.");
				return new Result<Expense>(ResponseCode.NotFound, null);
			}

			return new Result<Expense>(ResponseCode.Ok, result);
		}

		/// <inheritdoc/>
		public async Task<Result<Expense>> AddAsync(Expense expense)
		{
			var result = await _expenseRepo.AddExpenseAsync(expense).CAF();

			if (result is null)
			{
				_logger.Write(Core.Common.LogLevel.Warning, "Cannot add the expense.");
				return new Result<Expense>(ResponseCode.Error, null);
			}

			ExpenseAdded?.Invoke(this, result);

			return new Result<Expense>(ResponseCode.Ok, result);

		}

		/// <inheritdoc/>
		public async Task<Result<Expense>> UpdateAsync(int id, Expense expense)
		{
			var result = await _expenseRepo.UpdateExpenseAsync(id, expense).CAF();

			if (result is null)
			{
				_logger.Write(Core.Common.LogLevel.Warning, $"Cannot update the expense of the {id}.");
				return new Result<Expense>(ResponseCode.Error, null);
			}

			ExpenseUpdated?.Invoke(this, result);

			return new Result<Expense>(ResponseCode.Ok, result);
		}

		/// <inheritdoc/>
		public async Task<ResponseCode> RemoveAsync(int id)
		{
			var result = await _expenseRepo.RemoveExpenseAsync(id).CAF();

			ExpenseRemoved?.Invoke(this, id);

			return result ? ResponseCode.Ok : ResponseCode.Error;
		}
	}
}
