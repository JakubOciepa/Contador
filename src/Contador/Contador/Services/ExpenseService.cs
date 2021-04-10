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
	/// Notify on expense changes.
	/// </summary>
	public class ExpenseService : IExpenseManager
	{
		private readonly IExpenseRepository _expenseRepo;
		private readonly ILog _logger;

		/// <summary>
		/// Invoked when new(returned) expense has been added.
		/// </summary>
		public event EventHandler<Expense> ExpenseAdded;

		/// <summary>
		/// Invoked when returned expense has been updated.
		/// </summary>
		public event EventHandler<Expense> ExpenseUpdated;

		/// <summary>
		/// Invoked when expense of returned id has been removed.
		/// </summary>
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

		/// <summary>
		/// Gets all available expenses.
		/// </summary>
		/// <returns>Result which proper response code and list of expenses.</returns>
		public async Task<Result<IList<Expense>>> GetExpensesAsync()
		{
			var expenses = new List<Expense>();

			try
			{
				expenses = await _expenseRepo.GetExpensesAsync().CAF() as List<Expense>;
			}
			catch (Exception ex)
			{
				_logger.Write(LogLevel.Error, $"{ex}");
				return new Result<IList<Expense>>(ResponseCode.Error, new List<Expense>()) { Message = $"{ex}" };
			}

			if (expenses?.Count is 0)
			{
				_logger.Write(Core.Common.LogLevel.Warning, "Expenses not found.");
				return new Result<IList<Expense>>(ResponseCode.NotFound, new List<Expense>());
			}

			var list = new List<Expense>();

			foreach (var expense in expenses)
			{
				list.Add(expense);
			}

			return new Result<IList<Expense>>(ResponseCode.Ok, list);
		}

		/// <summary>
		/// Gets <see cref="Expense"/> of provided id.
		/// </summary>
		/// <param name="id">Id of requested Expense.</param>
		/// <returns><see cref="Expense"/> of provided id.</returns>
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

		/// <summary>
		/// Gets <see cref="Expense"/> for provided month.
		/// </summary>
		/// <param name="month">Month of the expenses creation.</param>
		/// <param name="year">Year of the expenses creation.</param>
		/// <returns><see cref="IList{Expense}"/> which were created in provided month.</returns>
		public async Task<Result<IList<Expense>>> GetByMonthAsync(int month, int year)
		{
			var expenses = new List<Expense>();

			try
			{
				expenses = await _expenseRepo.GetByMonthAsync(month, year).CAF() as List<Expense>;
			}
			catch (Exception ex)
			{
				var message = $"{ex}";

				_logger.Write(LogLevel.Error, $"{message}\n{ex.StackTrace}");
				return new Result<IList<Expense>>(ResponseCode.Error, expenses) { Message = message };
			}

			if (expenses.Count < 1)
			{
				_logger.Write(Core.Common.LogLevel.Warning, $"Expenses that were created at {month}-{year} not found.");
				return new Result<IList<Expense>>(ResponseCode.NotFound, null);
			}

			return new Result<IList<Expense>>(ResponseCode.Ok, expenses);
		}

		/// <summary>
		/// Gets <see cref="Expense"/> for provided year.
		/// </summary>
		/// <param name="year">Year of the expenses creation.</param>
		/// <returns><see cref="IList{Expense}"/> which were created in provided year.</returns>
		public async Task<Result<IList<Expense>>> GetByYearAsync( int year)
		{
			var expenses = new List<Expense>();

			try
			{
				expenses = await _expenseRepo.GetByYearAsync(year).CAF() as List<Expense>;
			}
			catch (Exception ex)
			{
				var message = $"{ex}";

				_logger.Write(LogLevel.Error, $"{message}\n{ex.StackTrace}");
				return new Result<IList<Expense>>(ResponseCode.Error, expenses) { Message = message };
			}

			if (expenses.Count < 1)
			{
				_logger.Write(Core.Common.LogLevel.Warning, $"Expenses that were created at {year} not found.");
				return new Result<IList<Expense>>(ResponseCode.NotFound, null);
			}

			return new Result<IList<Expense>>(ResponseCode.Ok, expenses);
		}

		/// <summary>
		/// Adds provided <see cref="Expense"/> into storage.
		/// </summary>
		/// <param name="expense">Expense to add.</param>
		/// <returns>Correct <see cref="ResponseCode"/> for operation and added expense.</returns>
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

		/// <summary>
		/// Updates <see cref="Expense"/> of provided id.
		/// </summary>
		/// <param name="id">Id of expense to update.</param>
		/// <param name="expense">Expense info.</param>
		/// <returns>Correct <see cref="ResponseCode"/> for operation and updated expense.</returns>
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

		/// <summary>
		/// Removes <see cref="Expense"/> of provided id.
		/// </summary>
		/// <param name="id">Id of expense to remove.</param>
		/// <returns>Correct <see cref="ResponseCode"/> for operation</returns>
		public async Task<ResponseCode> RemoveAsync(int id)
		{
			var result = await _expenseRepo.RemoveExpenseAsync(id).CAF();

			ExpenseRemoved?.Invoke(this, id);

			return result ? ResponseCode.Ok : ResponseCode.Error;
		}
	}
}
