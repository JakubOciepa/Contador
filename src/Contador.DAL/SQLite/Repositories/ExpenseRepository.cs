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
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly SQLiteAsyncConnection _dbConnection;
        private readonly IExpenseCategoryRepository _expenseCategoryRepository;

        public ExpenseRepository(SQLiteAsyncConnection connection, IExpenseCategoryRepository expenseCategoryRepository)
        {
            _dbConnection = connection;
            _expenseCategoryRepository = expenseCategoryRepository;
        }

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

                return await Task.FromResult<Expense>(new Expense(saved.Name, saved.Value, null, null)).CAF();
            }

            return null;
        }

        public async Task<Expense> GetExpenseAsync(int expenseId)
        {
            var result = await _dbConnection.Table<ExpenseDto>().FirstAsync(item => item.Id == expenseId).CAF();

            if (result is object)
            {
                var user = new User()
                {
                    Name = "Kuba",
                    Id = 1,
                    Email = string.Empty,
                };

                var category = new ExpenseCategory("Słodycze") { Id = 0, };

                return await Task.FromResult(new Expense(result.Name, result.Value, user, category)).CAF();
            }

            return null;
        }

        public Task<IList<Expense>> GetExpensesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveExpenseAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Expense> UpdateExpenseAsync(int id, Expense info)
        {
            throw new NotImplementedException();
        }
    }
}
