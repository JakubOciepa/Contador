using System.Collections.Generic;
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

			ExpenseDto expense = (await _dbConnection
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
			IEnumerable<ExpenseDto> expenses = await _dbConnection
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

			IEnumerable<ExpenseDto> expenses = await _dbConnection
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

			IEnumerable<ExpenseDto> expenses = await _dbConnection
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
		/// Gets the count or less of latest expenses.
		/// </summary>
		/// <param name="count">Max count of latest expenses to return.</param>
		/// <returns>The count or less of latest expenses.</returns>
		public async Task<IList<Expense>> GetLatest(int count)
		{
			var parameter = new DynamicParameters();
			parameter.Add(ExpenseDto.ParameterName.Count, count);

			IEnumerable<ExpenseDto> expenses = await _dbConnection
				.QueryAsync<ExpenseDto, ExpenseCategoryDto, UserDto, ExpenseDto>(ExpenseDto.ProcedureName.GetLatest,
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
		/// Gets expenses filtered by provided values.
		/// </summary>
		/// <param name="name">Name of the expense of part of the name to filter.</param>
		/// <param name="categoryName">Name of the category to filter.</param>
		/// <param name="userName">Name of the user to filter.</param>
		/// <returns>List of the expenses that fulfill the requirements</returns>
		public async Task<IList<Expense>> GetFiltered(string name, string categoryName, string userName)
		{
			var parameters = new DynamicParameters();
			parameters.Add(ExpenseDto.ParameterName.Name, name ?? "");
			parameters.Add(ExpenseDto.ParameterName.CategoryName, categoryName ?? "");
			parameters.Add(ExpenseDto.ParameterName.UserName, userName ?? "");

			IEnumerable<ExpenseDto> expenses = await _dbConnection
				.QueryAsync<ExpenseDto, ExpenseCategoryDto, UserDto, ExpenseDto>(ExpenseDto.ProcedureName.GetFiltered,
				(expense, category, user) =>
				{
					expense.Category = category;
					expense.User = user;

					return expense;
				},
				parameters,
				commandType: CommandType.StoredProcedure)
				.CAF();

			return expenses.Cast<Expense>().ToList();
		}

		/// <summary>
		/// Gets all expenses for provided category.
		/// </summary>
		/// <param name="categoryId">Category id of searched expenses.</param>
		/// <returns><see cref="IList{Expense}"/> of expenses in this category.</returns>
		public async Task<IList<Expense>> GetByCategory(int categoryId)
		{
			var parameters = new DynamicParameters();
			parameters.Add(ExpenseDto.ParameterName.CategoryId, categoryId);

			IEnumerable<Expense> expenses = await _dbConnection
				.QueryAsync<ExpenseDto, ExpenseCategoryDto, UserDto, ExpenseDto>(ExpenseDto.ProcedureName.GetByCategory,
				(expense, category, user) =>
				{
					expense.Category = category;
					expense.User = user;

					return expense;
				},
				parameters,
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

			IEnumerable<ExpenseDto> result = await _dbConnection
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
		/// Updates the <see cref="Expense"/> of the provided id.
		/// </summary>
		/// <param name="id">Id of the expense to update.</param>
		/// <param name="info">Expense info.</param>
		/// <returns>Updated expense.</returns>
		public async Task<Expense> UpdateAsync(int id, Expense expense)
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

			IEnumerable<ExpenseDto> result = await _dbConnection
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
		/// Removes the <see cref="Expense"/> of the provided id from the storage.
		/// </summary>
		/// <param name="id">Id of the expense to remove.</param>
		/// <returns>True if removed, false otherwise.</returns>
		public async Task<bool> RemoveAsync(int id)
		{
			var param = new DynamicParameters();
			param.Add(ExpenseDto.ParameterName.Id, id);

			_ = await _dbConnection.ExecuteAsync(ExpenseDto.ProcedureName.Delete, param, commandType: CommandType.StoredProcedure).CAF();

			return !(await GetByIdAsync(id).CAF() is object);
		}
	}
}
