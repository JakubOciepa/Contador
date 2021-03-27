﻿using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

using Contador.Core.Models;
using Contador.Core.Utils.Extensions;
using Contador.DAL.Abstractions;
using Contador.DAL.MySql.Models;

using Dapper;

namespace Contador.DAL.MySql.Repositories
{
	/// <summary>
	/// Manages expenses in db.
	/// </summary>
	public class ExpenseRepository : IExpenseRepository
	{
		private readonly IDbConnection _dbConnection;

		/// <summary>
		/// Creates instance of <see cref="ExpenseRepository"/> class.
		/// </summary>
		/// <param name="dbConnection">Connection to database.</param>
		public ExpenseRepository(IDbConnection dbConnection)
		{
			_dbConnection = dbConnection;
		}

		/// <summary>
		/// Gets <see cref="Expense"/> of the provided id.
		/// </summary>
		/// <param name="expenseId">Id of the requested <see cref="Expense"/>.</param>
		/// <returns><see cref="Expense"/> of the provided Id.</returns>
		public async Task<Expense> GetByIdAsync(int expenseId)
		{
			var parameter = new DynamicParameters();
			parameter.Add(ExpenseDto.ParameterName.Id, expenseId);

			var expense = (await _dbConnection
				.QueryAsync<ExpenseDto, ExpenseCategoryDto, UserDto, ExpenseDto>(ExpenseDto.ProcedureName.GetById,
					(expense, category, user) =>
					{
						expense.Category = category;
						expense.User = user;

						return expense;
					},
					parameter,
					commandType: CommandType.StoredProcedure)
				.CAF()).FirstOrDefault();

			return expense?.AsExpense();
		}

		/// <summary>
		/// Gets all available expenses.
		/// </summary>
		/// <returns><see cref="IList{Expense}"/> of all available expenses.</returns>
		public async Task<IList<Expense>> GetAllAsync()
		{
			var expenses = await _dbConnection
				.QueryAsync<ExpenseDto, ExpenseCategoryDto, UserDto, ExpenseDto>(ExpenseDto.ProcedureName.GetAll,
					(expense, category, user) =>
					{
						expense.Category = category;
						expense.User = user;

						return expense;
					},
					commandType: CommandType.StoredProcedure)
				.CAF();

			return expenses.Cast<Expense>().ToList();
		}

		/// <summary>
		/// Gets all expenses by provided month.
		/// </summary>
		/// <param name="month">Creation month of the expenses.</param>
		/// <param name="year">Creation year of the expenses.</param>
		/// <returns><see cref="IList{Expense}"/> of all expenses from provided month.</returns>
		public async Task<IList<Expense>> GetByMonthAsync(int month, int year)
		{
			var parameter = new DynamicParameters();
			parameter.Add(ExpenseDto.ParameterName.MonthNum, month);
			parameter.Add(ExpenseDto.ParameterName.Year, year);

			var expenses = await _dbConnection
				.QueryAsync<ExpenseDto, ExpenseCategoryDto, UserDto, ExpenseDto>(ExpenseDto.ProcedureName.GetByMonth,
				(expense, category, user) =>
				{
					expense.Category = category;
					expense.User = user;

					return expense;
				},
				parameter,
				commandType: CommandType.StoredProcedure)
				.CAF();

			return expenses.Cast<Expense>().ToList();
		}
		
		/// <summary>
		/// Gets all expenses by provided year.
		/// </summary>
		/// <param name="year">Creation year of the expenses.</param>
		/// <returns><see cref="IList{Expense}"/> of all expenses from provided year.</returns>
		public async Task<IList<Expense>> GetByYearAsync(int year)
		{
			var parameter = new DynamicParameters();
			parameter.Add(ExpenseDto.ParameterName.Year, year);

			var expenses = await _dbConnection
				.QueryAsync<ExpenseDto, ExpenseCategoryDto, UserDto, ExpenseDto>(ExpenseDto.ProcedureName.GetByYear,
				(expense, category, user) =>
				{
					expense.Category = category;
					expense.User = user;

					return expense;
				},
				parameter,
				commandType: CommandType.StoredProcedure)
				.CAF();

			return expenses.Cast<Expense>().ToList();
		}

		/// <summary>
		/// Adds provided <see cref="Expense"/> to the storage.
		/// </summary>
		/// <param name="expense">Expense to add.</param>
		/// <returns>The added expense.</returns>
		public async Task<Expense> AddAsync(Expense expense)
		{
			var param = new DynamicParameters();
			param.Add(ExpenseDto.ParameterName.Name, expense.Name);
			param.Add(ExpenseDto.ParameterName.Value, expense.Value);
			param.Add(ExpenseDto.ParameterName.Description, expense.Description);
			param.Add(ExpenseDto.ParameterName.CategoryId, expense.Category.Id);
			param.Add(ExpenseDto.ParameterName.UserId, expense.User.Id);
			param.Add(ExpenseDto.ParameterName.ImagePath, expense.ImagePath);

			var result = await _dbConnection
				.QueryAsync<ExpenseDto, ExpenseCategoryDto, UserDto, ExpenseDto>(ExpenseDto.ProcedureName.Add,
				(expense, category, user) =>
				{
					expense.Category = category;
					expense.User = user;

					return expense;
				},
				param, commandType: CommandType.StoredProcedure).CAF();

			return result.First().AsExpense();
		}

		/// <summary>
		/// Updates <see cref="Expense"/> of provided id.
		/// </summary>
		/// <param name="id">Id of expense to update.</param>
		/// <param name="info">Expense info.</param>
		/// <returns>Updated expense or default.</returns>
		public async Task<Expense> UpdateExpenseAsync(int id, Expense expense)
		{
			var param = new DynamicParameters();
			param.Add(ExpenseDto.ParameterName.Id, expense.Id);
			param.Add(ExpenseDto.ParameterName.Name, expense.Name);
			param.Add(ExpenseDto.ParameterName.Value, expense.Value);
			param.Add(ExpenseDto.ParameterName.Description, expense.Description);
			param.Add(ExpenseDto.ParameterName.CategoryId, expense.Category.Id);
			param.Add(ExpenseDto.ParameterName.UserId, expense.User.Id);
			param.Add(ExpenseDto.ParameterName.ImagePath, expense.ImagePath);
			param.Add(ExpenseDto.ParameterName.CreateDate, expense.CreateDate);

			var result = await _dbConnection
				.QueryAsync<ExpenseDto, ExpenseCategoryDto, UserDto, ExpenseDto>(ExpenseDto.ProcedureName.Update,
				(expense, category, user) =>
				{
					expense.Category = category;
					expense.User = user;

					return expense;
				},
				param, commandType: CommandType.StoredProcedure).CAF();

			return result.First().AsExpense();
		}

		/// <summary>
		/// Removes <see cref="Expense"/> of provided id from storage.
		/// </summary>
		/// <param name="id">Id of expense to remove.</param>
		/// <returns>True if removed, false otherwise.</returns>
		public async Task<bool> RemoveExpenseAsync(int id)
		{
			var param = new DynamicParameters();
			param.Add(ExpenseDto.ParameterName.Id, id);

			await _dbConnection.ExecuteAsync(ExpenseDto.ProcedureName.Delete, param, commandType: CommandType.StoredProcedure).CAF();

			return !(await GetByIdAsync(id).CAF() is object);
		}
	}
}
