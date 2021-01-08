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
            var categoryToSave = new ExpenseCategoryDto(expenseCategory.Name);

            if (await _connection.InsertAsync(categoryToSave).CAF() != 0)
            {
                var savedCategory = await _connection.Table<ExpenseCategoryDto>()
                    .FirstAsync(category => category.Name == categoryToSave.Name).CAF();

                return await Task.FromResult(new ExpenseCategory(savedCategory.Name) { Id = savedCategory.Id }).CAF();
            }

            return null;
        }

        public Task<IList<ExpenseCategory>> GetCategoriesAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<ExpenseCategory> GetCategoryByIdAsync(int categoryId)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> RemoveCategoryAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<ExpenseCategory> UpdateCategoryAsync(int id, ExpenseCategory expenseCategory)
        {
            throw new System.NotImplementedException();
        }
    }
}
