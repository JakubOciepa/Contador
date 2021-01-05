using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Contador.Core.Models;
using Contador.DAL.Abstractions;
using Contador.DAL.SQLite.Models;

using SQLite;

namespace Contador.DAL.SQLite.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly SQLiteAsyncConnection _dbConnection;

        public ExpenseRepository(string dbPath)
        {
            _dbConnection = new SQLiteAsyncConnection(dbPath);
            _dbConnection.CreateTableAsync<ExpenseDto>().Wait();
        }

        public Task<Expense> AddExpenseAsync(Expense expense)
        {
            throw new NotImplementedException();
        }

        public Task<Expense> GetExpenseAsync(int expenseId)
        {
            throw new NotImplementedException();
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
