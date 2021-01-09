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
            var expense = await _dbConnection.Table<ExpenseDto>().FirstAsync(item => item.Id == expenseId).CAF();

            if (expense is object)
            {
                var user = new User()
                {
                    Name = "Kuba",
                    Id = 1,
                    Email = string.Empty,
                };

                var category = new ExpenseCategory("Słodycze") { Id = 0, };

                return await Task.FromResult(new Expense(expense.Name, expense.Value, user, category)).CAF();
            }

            return null;
        }

        public async Task<IList<Expense>> GetExpensesAsync()
        {
            var expenses = await _dbConnection.Table<ExpenseDto>().ToListAsync().CAF();

            var user = new User()
            {
                Name = "Kuba",
                Id = 1,
                Email = string.Empty,
            };

            var category = new ExpenseCategory("Słodycze") { Id = 0, };

            return await Task.FromResult(expenses
                             .ConvertAll(expense => new Expense(expense.Name, expense.Value, user, category)))
                             .CAF();
        }

        public Task<bool> RemoveExpenseAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Expense> UpdateExpenseAsync(int id, Expense info)
        {
            var expenseToUpdate = await _dbConnection.Table<ExpenseDto>().FirstAsync(expense => expense.Id == id).CAF();

            expenseToUpdate.Name = info.Name;
            expenseToUpdate.Value = info.Value;
            expenseToUpdate.Description = info.Description;
            expenseToUpdate.ImagePath = info.ImagePath;
            expenseToUpdate.UserId = info.User.Id;
            expenseToUpdate.CategoryId = info.Category.Id;
            expenseToUpdate.ModifiedDate = DateTime.Now;

            var result = await _dbConnection.UpdateAllAsync(new List<ExpenseDto>() { expenseToUpdate }).CAF();

            return await Task.FromResult(info).CAF();
        }
    }
}
