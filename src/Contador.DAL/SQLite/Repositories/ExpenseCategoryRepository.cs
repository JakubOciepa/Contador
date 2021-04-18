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
	/// Manages expense categories in db.
	/// </summary>
	public class ExpenseCategoryRepository : IExpenseCategoryRepository
	{
		private readonly SQLiteAsyncConnection _dbConnection;

		/// <summary>
		/// Creates instance of the <see cref="ExpenseCategoryRepository"/> class.
		/// </summary>
		/// <param name="connection"><see cref="SQLiteAsyncConnection"/> connection.</param>
		public ExpenseCategoryRepository(SQLiteAsyncConnection connection)
		{
			_dbConnection = connection;
		}

		/// <summary>
		/// Adds expense category to storage.
		/// </summary>
		/// <param name="expenseCategory">Expense category to add.</param>
		/// <returns>Added expense category</returns>
		public async Task<ExpenseCategory> AddAsync(ExpenseCategory expenseCategory)
		{
			if (await GetCategoryByName(expenseCategory.Name).CAF() is object)
				return null;

			var categoryToSave = new ExpenseCategoryDto(expenseCategory.Name);

			if (await _dbConnection.InsertAsync(categoryToSave).CAF() != 0)
			{
				var savedCategory = await _dbConnection.Table<ExpenseCategoryDto>()
					.FirstAsync(category => category.Name == categoryToSave.Name).CAF();

				return await Task.FromResult(new ExpenseCategory(savedCategory.Name) { Id = savedCategory.Id }).CAF();
			}

			return null;
		}

		/// <summary>
		/// Gets all available expense categories.
		/// </summary>
		/// <returns><see cref="IList{ExpenseCategory}"/> of all available categories.</returns>
		public async Task<IList<ExpenseCategory>> GetAllAsync()
		{
			var categories = await _dbConnection.Table<ExpenseCategoryDto>().ToListAsync().CAF();

			if (categories is object)
			{
				return await Task.FromResult(categories
								 .ConvertAll(category => new ExpenseCategory(category.Name) { Id = category.Id }))
								 .CAF();
			}

			return null;
		}

		/// <summary>
		/// Gets category by its id.
		/// </summary>
		/// <param name="categoryId">Id of requested <see cref="ExpenseCategory"/>.</param>
		/// <returns><see cref="ExpenseCategory"/> of requested Id.</returns>
		public async Task<ExpenseCategory> GetByIdAsync(int categoryId)
		{
			var category = await _dbConnection.Table<ExpenseCategoryDto>()
											.FirstOrDefaultAsync(category => category.Id == categoryId)
											.CAF();

			if (category is object)
			{
				return await Task.FromResult(new ExpenseCategory(category.Name) { Id = category.Id }).CAF();
			}

			return null;
		}

		/// <summary>
		/// Removes expense category from storage.
		/// </summary>
		/// <param name="id">Id of expense category to remove.</param>
		/// <returns>True if removed, false otherwise.</returns>
		public async Task<bool> RemoveAsync(int id)
		{
			return await Task.FromResult(await _dbConnection.Table<ExpenseCategoryDto>()
															 .DeleteAsync(category => category.Id == id)
															 .CAF() == 1)
							 .CAF();
		}

		/// <summary>
		/// Updates expense category of provided id.
		/// </summary>
		/// <param name="id">Id of expense category to update.</param>
		/// <param name="expenseCategory">Category info.</param>
		/// <returns>Updated category.</returns>
		public async Task<ExpenseCategory> UpdateAsync(int id, ExpenseCategory expenseCategory)
		{
			var categoryToUpdate = await _dbConnection.Table<ExpenseCategoryDto>().FirstAsync(category => category.Id == id).CAF();

			if (categoryToUpdate is object)
			{
				categoryToUpdate.Name = expenseCategory.Name;

				var result = await _dbConnection.UpdateAllAsync(new List<ExpenseCategoryDto>() { categoryToUpdate }).CAF();

				return result is 1 ? await Task.FromResult(expenseCategory).CAF() : null;
			}

			return null;
		}


		private async Task<ExpenseCategoryDto> GetCategoryByName(string name)
		{
			return await _dbConnection.Table<ExpenseCategoryDto>()
									.FirstOrDefaultAsync(category => category.Name == name)
									.CAF();
		}
	}
}
