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
        private readonly SQLiteAsyncConnection _connection;

        public ExpenseCategoryRepository(SQLiteAsyncConnection connection)
        {
            _connection = connection;
        }

        public async Task<ExpenseCategory> AddCategoryAsync(ExpenseCategory expenseCategory)
        {
            if (await GetCategoryByName(expenseCategory.Name).CAF() is object)
                return null;

            var categoryToSave = new ExpenseCategoryDto(expenseCategory.Name);

            if (await _connection.InsertAsync(categoryToSave).CAF() != 0)
            {
                var savedCategory = await _connection.Table<ExpenseCategoryDto>()
                    .FirstAsync(category => category.Name == categoryToSave.Name).CAF();

                return await Task.FromResult(new ExpenseCategory(savedCategory.Name) { Id = savedCategory.Id }).CAF();
            }

            return null;
        }

        public async Task<IList<ExpenseCategory>> GetCategoriesAsync()
        {
            var categories = await _connection.Table<ExpenseCategoryDto>().ToListAsync().CAF();

            return await Task.FromResult(categories
                             .ConvertAll(category => new ExpenseCategory(category.Name) { Id = category.Id }))
                             .CAF();
        }

        public async Task<ExpenseCategory> GetCategoryByIdAsync(int categoryId)
        {
            var category = await _connection.Table<ExpenseCategoryDto>()
                                            .FirstOrDefaultAsync(category => category.Id == categoryId)
                                            .CAF();

            if (category is object)
            {
                return await Task.FromResult(new ExpenseCategory(category.Name) { Id = category.Id }).CAF();
            }

            return null;
        }

        public Task<bool> RemoveCategoryAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<ExpenseCategory> UpdateCategoryAsync(int id, ExpenseCategory expenseCategory)
        {
            throw new System.NotImplementedException();
        }

        private async Task<ExpenseCategoryDto> GetCategoryByName(string name)
        {
            return await _connection.Table<ExpenseCategoryDto>()
                                    .FirstOrDefaultAsync(category => category.Name == name)
                                    .CAF();
        }
    }
}
