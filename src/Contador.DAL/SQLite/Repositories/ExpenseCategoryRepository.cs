using System.Collections.Generic;
using System.Threading.Tasks;

using Contador.Core.Models;
using Contador.DAL.Abstractions;

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

        public Task<ExpenseCategory> AddCategoryAsync(ExpenseCategory expenseCategory)
        {
            throw new System.NotImplementedException();
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
