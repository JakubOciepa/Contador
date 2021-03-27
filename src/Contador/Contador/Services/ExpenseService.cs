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
	/// Provides methods to manage expenses.
	/// Gives possibility to get, add, update or remove expense.
	/// Notifies on any expense change.
	/// </summary>
	public class ExpenseService : IExpenseManager
	{
		private readonly IExpenseRepository _expenseRepo;
		private readonly ILog _logger;

		/// <summary>
		/// Invoked when the new(returned) expense has been added.
		/// </summary>
		public event EventHandler<Expense> ExpenseAdded;

		/// <summary>
		/// Invoked when the returned expense has been updated.
		/// </summary>
		public event EventHandler<Expense> ExpenseUpdated;

		/// <summary>
		/// Invoked when the expense of the returned id has been removed.
		/// </summary>
		public event EventHandler<int> ExpenseRemoved;

		/// <summary>
		/// Creates an instance of the <see cref="ExpenseService"/> class.
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
		/// <returns><see cref="Result{IList{Expense}}"/> with the proper <see cref="ResponseCode"/> and the <see cref="IList{Expense}"/>.</returns>
		public async Task<Result<IList<Expense>>> GetAllAsync()
		{
			List<Expense> expenses;

			try
			{
				expenses = await _expenseRepo.GetAllAsync().CAF() as List<Expense>;
			}
			catch (Exception ex)
			{
				_logger.Write(LogLevel.Error, $"{ex}");

				return new Result<IList<Expense>>(ResponseCode.Error, new List<Expense>()) { Message = $"{ex}" };
			}

			if (expenses?.Count is 0)
			{
				_logger.Write(LogLevel.Warning, "Expenses not found.");

				return new Result<IList<Expense>>(ResponseCode.NotFound, new List<Expense>());
			}

			return new Result<IList<Expense>>(ResponseCode.Ok, expenses);
		}

		/// <summary>
		/// Gets <see cref="Expense"/> of provided id.
		/// </summary>
		/// <param name="id">Id of requested Expense.</param>
		/// <returns><see cref="Expense"/> of provided id.</returns>
		public async Task<Result<Expense>> GetByIdAsync(int id)
		{
			Expense expense;

			try
			{
				expense = await _expenseRepo.GetByIdAsync(id).CAF();
			}
			catch (Exception ex)
			{
				_logger.Write(LogLevel.Error, $"{ex.Message}\n{ex.StackTrace}");

				return new Result<Expense>(ResponseCode.Error, null) { Message = ex.Message };
			}

			if (expense is null)
			{
				_logger.Write(LogLevel.Warning, $"Expense of the {id} not found.");

				return new Result<Expense>(ResponseCode.NotFound, null);
			}

			return new Result<Expense>(ResponseCode.Ok, expense);
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
		public async Task<Result<IList<Expense>>> GetByYearAsync(int year)
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
			Expense addedExpense;
			try
			{
				addedExpense = await _expenseRepo.AddAsync(expense).CAF();
			}
			catch (Exception ex)
			{
				_logger.Write(LogLevel.Error, $"{ex.Message}\n{ex.StackTrace}");

				return new Result<Expense>(ResponseCode.Error, null) { Message = ex.Message };
			}

			if (addedExpense is null)
			{
				var error = "Cannot add the expense";
				_logger.Write(LogLevel.Warning, error);

				return new Result<Expense>(ResponseCode.Error, null) { Message = error };
			}

			ExpenseAdded?.Invoke(this, addedExpense);

			return new Result<Expense>(ResponseCode.Ok, addedExpense);
		}

		/// <summary>
		/// Updates the <see cref="Expense"/> of provided id.
		/// </summary>
		/// <param name="id">Id of the expense to update.</param>
		/// <param name="expense">Expense info.</param>
		/// <returns>Correct <see cref="ResponseCode"/> for operation and updated expense.</returns>
		public async Task<Result<Expense>> UpdateAsync(int id, Expense expense)
		{
			Expense updated;

			try
			{
				updated = await _expenseRepo.UpdateAsync(id, expense).CAF();
			}
			catch (Exception ex)
			{
				var message = $"{ex.Message}\n{ex.StackTrace}";

				return new Result<Expense>(ResponseCode.Error, null) { Message = message };
			}

			if (updated is null)
			{
				var message = $"Cannot update the expense of the {id}.";
				_logger.Write(LogLevel.Warning, message);

				return new Result<Expense>(ResponseCode.Error, null) { Message = message };
			}

			ExpenseUpdated?.Invoke(this, updated);

			return new Result<Expense>(ResponseCode.Ok, updated);
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
