using System.Collections.Generic;
using System.Threading.Tasks;

using Contador.Core.Models;
using Contador.Core.Utils.Extensions;
using Contador.DAL.Abstractions;
using Contador.DAL.SQLite.Models;

using SQLite;

namespace Contador.DAL.SQLite.Repositories
{
	public class ExpenseCategoryRepository : IExpenseCategoryRepository
	{
		private readonly SQLiteAsyncConnection _dbConnection;

		public ExpenseCategoryRepository(SQLiteAsyncConnection connection)
		{
			_dbConnection = connection;
		}

		public async Task<ExpenseCategory> AddCategoryAsync(ExpenseCategory expenseCategory)
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

		public async Task<IList<ExpenseCategory>> GetCategoriesAsync()
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

		public async Task<ExpenseCategory> GetCategoryByIdAsync(int categoryId)
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

		public async Task<bool> RemoveCategoryAsync(int id)
		{
			return await Task.FromResult(await _dbConnection.Table<ExpenseCategoryDto>()
															 .DeleteAsync(category => category.Id == id)
															 .CAF() == 1)
							 .CAF();
		}

		public async Task<ExpenseCategory> UpdateCategoryAsync(int id, ExpenseCategory expenseCategory)
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
