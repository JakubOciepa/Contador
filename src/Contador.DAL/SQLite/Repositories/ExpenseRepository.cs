using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Contador.Core.Models;
using Contador.Core.Utils.Extensions;
using Contador.DAL.Abstractions;
using Contador.DAL.SQLite.Models;

using SQLite;

namespace Contador.DAL.SQLite.Repositories
{
	/// <summary>
	/// Manages expenses in db.
	/// </summary>
	public class ExpenseRepository : IExpenseRepository
	{
		private readonly SQLiteAsyncConnection _dbConnection;
		private readonly IExpenseCategoryRepository _expenseCategoryRepository;

		/// <summary>
		/// Creates instance of the <see cref="ExpenseRepository"/> class.
		/// </summary>
		/// <param name="connection"><see cref="SQLiteAsyncConnection"/> connection.</param>
		/// <param name="expenseCategoryRepository"><see cref="IExpenseCategoryRepository"/> Expense category repository.</param>
		public ExpenseRepository(SQLiteAsyncConnection connection, IExpenseCategoryRepository expenseCategoryRepository)
		{
			_dbConnection = connection;
			_expenseCategoryRepository = expenseCategoryRepository;
		}

		/// <summary>
		/// Adds provided <see cref="Expense"/> to the storage.
		/// </summary>
		/// <param name="expense">Expense to add.</param>
		/// <returns>The added expense.</returns>
		public async Task<Expense> AddAsync(Expense expense)
		{
			var expenseToSave = new ExpenseDto()
			{
				Name = expense.Name,
				Value = expense.Value,
				Description = expense.Description,
				ImagePath = expense.ImagePath,
				CreateDate = DateTime.Now,
				ModifiedDate = DateTime.Now,
				CategoryId = expense.Category.Id,
				UserId = expense.User.Id
			};

			if (await _dbConnection.InsertAsync(expenseToSave).CAF() != 0)
			{
				var saved = await _dbConnection.Table<ExpenseDto>()
					.FirstAsync(item => item.Name == expenseToSave.Name && item.CreateDate == expenseToSave.CreateDate)
					.CAF();

				var user = new User()
				{
					Name = "Kuba",
					Id = 1,
					Email = string.Empty,
				};

				var category = await _expenseCategoryRepository.GetCategoryByIdAsync(saved.CategoryId).CAF();

				return await Task.FromResult(new Expense(saved.Name, saved.Value, user, category)
				{
					Id = saved.Id,
					CreateDate = saved.CreateDate,
					Description = saved.Description,
					ImagePath = saved.ImagePath,
				}).CAF();
			}

			return null;
		}

		/// <summary>
		/// Gets <see cref="Expense"/> of provided id.
		/// </summary>
		/// <param name="expenseId">Id of requested <see cref="Expense"/>.</param>
		/// <returns><see cref="Expense"/> of provided Id.</returns>
		public async Task<Expense> GetByIdAsync(int expenseId)
		{
			var expense = await _dbConnection.Table<ExpenseDto>().FirstAsync(item => item.Id == expenseId).CAF();

			if (expense is object)
			{
				var user = new User()
				{
					Name = "Kuba",
					Id = 1,
					Email = string.Empty,
				};

				ExpenseCategory category = await _expenseCategoryRepository.GetCategoryByIdAsync(expense.CategoryId).CAF();

				return await Task.FromResult(new Expense(expense.Name, expense.Value, user, category)
				{
					Id = expense.Id,
					Description = expense.Description,
					CreateDate = expense.CreateDate,
					ImagePath = expense.ImagePath,
				}
				).CAF();
			}

			return null;
		}

		/// <summary>
		/// Gets all available expenses.
		/// </summary>
		/// <returns><see cref="IList{Expense}"/> of all available expenses.</returns>
		public async Task<IList<Expense>> GetAllAsync()
		{
			var expenses = await _dbConnection.Table<ExpenseDto>().ToListAsync().CAF();

			if (expenses is object)
			{
				var user = new User()
				{
					Name = "Kuba",
					Id = 1,
					Email = string.Empty,
				};

				return await Task.FromResult(expenses?.ConvertAll
					(expense => new Expense(expense.Name, expense.Value, user, _expenseCategoryRepository.GetCategoryByIdAsync(expense.CategoryId).Result)
					{
						Id = expense.Id,
						Description = expense.Description,
						CreateDate = expense.CreateDate,
						ImagePath = expense.ImagePath,
					})
					).CAF();
			}

			return null;
		}

		/// <summary>
		/// Removes <see cref="Expense"/> of provided id from storage.
		/// </summary>
		/// <param name="id">Id of expense to remove.</param>
		/// <returns>True if removed, false otherwise.</returns>
		public async Task<bool> RemoveExpenseAsync(int id)
		{
			return await Task.FromResult(await _dbConnection.Table<ExpenseDto>()
															 .DeleteAsync(expense => expense.Id == id).CAF() == 1)
							 .CAF();
		}

		/// <summary>
		/// Updates <see cref="Expense"/> of provided id.
		/// </summary>
		/// <param name="id">Id of expense to update.</param>
		/// <param name="info">Expense info.</param>
		/// <returns>Updated expense or default.</returns>
		public async Task<Expense> UpdateExpenseAsync(int id, Expense info)
		{
			var expenseToUpdate = await _dbConnection.Table<ExpenseDto>()
													.FirstOrDefaultAsync(expense => expense.Id == id)
													.ConfigureAwait(true);

			if (expenseToUpdate is object)
			{
				expenseToUpdate.Name = info.Name;
				expenseToUpdate.Value = info.Value;
				expenseToUpdate.Description = info.Description;
				expenseToUpdate.ImagePath = info.ImagePath;
				expenseToUpdate.UserId = info.User.Id;
				expenseToUpdate.CategoryId = info.Category.Id;
				expenseToUpdate.ModifiedDate = DateTime.Now;

				var result = await _dbConnection.UpdateAllAsync(new List<ExpenseDto>() { expenseToUpdate })
					.ConfigureAwait(true);

				return result is 1 ? await Task.FromResult(info).CAF() : null;
			}

			return null;
		}

		/// <summary>
		/// Gets all expenses by provided month.
		/// </summary>
		/// <param name="month">Creation month of the expenses.</param>
		/// <param name="year">Creation year of the expenses.</param>
		/// <returns><see cref="IList{Expense}"/> of all expenses from provided month.</returns>
		public Task<IList<Expense>> GetByMonthAsync(int month, int year)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Gets all expenses by provided year.
		/// </summary>
		/// <param name="year">Creation year of the expenses.</param>
		/// <returns><see cref="IList{Expense}"/> of all expenses from provided year.</returns>
		public Task<IList<Expense>> GetByYearAsync(int year)
		{
			throw new NotImplementedException();
		}
	}
}
