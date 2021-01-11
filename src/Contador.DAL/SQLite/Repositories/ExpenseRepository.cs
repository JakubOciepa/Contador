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

		///<inheritdoc/>
		public async Task<Expense> AddExpenseAsync(Expense expense)
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

				return await Task.FromResult(new Expense(saved.Name, saved.Value, null, null)).CAF();
			}

			return null;
		}

		///<inheritdoc/>
		public async Task<Expense> GetExpenseAsync(int expenseId)
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

				return await Task.FromResult(new Expense(expense.Name, expense.Value, user, category)).CAF();
			}

			return null;
		}

		///<inheritdoc/>
		public async Task<IList<Expense>> GetExpensesAsync()
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

				return await Task.FromResult(expenses?.ConvertAll(expense => new Expense(expense.Name, expense.Value, user,
												_expenseCategoryRepository.GetCategoryByIdAsync(expense.CategoryId).Result)))
								 .CAF();
			}

			return null;
		}

		///<inheritdoc/>
		public async Task<bool> RemoveExpenseAsync(int id)
		{
			return await Task.FromResult(await _dbConnection.Table<ExpenseDto>()
															 .DeleteAsync(expense => expense.Id == id).CAF() == 1)
							 .CAF();
		}

		///<inheritdoc/>
		public async Task<Expense> UpdateExpenseAsync(int id, Expense info)
		{
			var expenseToUpdate = await _dbConnection.Table<ExpenseDto>().FirstAsync(expense => expense.Id == id).CAF();

			if (expenseToUpdate is object)
			{
				expenseToUpdate.Name = info.Name;
				expenseToUpdate.Value = info.Value;
				expenseToUpdate.Description = info.Description;
				expenseToUpdate.ImagePath = info.ImagePath;
				expenseToUpdate.UserId = info.User.Id;
				expenseToUpdate.CategoryId = info.Category.Id;
				expenseToUpdate.ModifiedDate = DateTime.Now;

				var result = await _dbConnection.UpdateAllAsync(new List<ExpenseDto>() { expenseToUpdate }).CAF();

				return result is 1 ? await Task.FromResult(info).CAF() : null;
			}

			return null;
		}
	}
}
