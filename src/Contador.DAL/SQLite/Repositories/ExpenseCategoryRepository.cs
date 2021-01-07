using System.Collections.Generic;
using System.Threading.Tasks;

using Contador.Core.Models;
using Contador.DAL.Abstractions;

namespace Contador.DAL.SQLite.Repositories
{
    public class ExpenseCategoryRepository : IExpenseCategoryRepository
    {
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
