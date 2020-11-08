using System.Collections.Generic;
using System.Threading.Tasks;

using Contador.Abstractions;
using Contador.Core.Common;
using Contador.Core.Models;
using Contador.Core.Utils.Extensions;
using Contador.DAL.Abstractions;

using Microsoft.Extensions.Logging;

namespace Contador.Services
{
    /// <inheritdoc/>
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepo;
        private readonly ILogger<ExpenseService> _logger;

        /// <summary>
        /// Creates instance of <see cref="ExpenseService"/> class.
        /// </summary>
        /// <param name="expenses">Repository of expenses.</param>
        public ExpenseService(IExpenseRepository expenses, ILogger<ExpenseService> logger)
        {
            _expenseRepo = expenses;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<Result<IList<Expense>>> GetExpenses()
        {
            var result = await _expenseRepo.GetExpenses().CAF();

            if (result.Count == 0)
            {
                _logger.LogInformation("Expenses not found.");
                return new Result<IList<Expense>>(ResponseCode.NotFound, new List<Expense>());
            }

            var list = new List<Expense>();

            foreach (var expense in result)
            {
                list.Add(expense);
            }

            return new Result<IList<Expense>>(ResponseCode.Ok, list);
        }

        /// <inheritdoc/>
        public async Task<Result<Expense>> GetExpense(int id)
        {
            var result = await _expenseRepo.GetExpense(id).CAF();

            if (result == default)
            {
                _logger.LogInformation($"Expense of the {id} not found.");
                return new Result<Expense>(ResponseCode.NotFound, default);
            }

            return new Result<Expense>(ResponseCode.Ok, result);
        }

        /// <inheritdoc/>
        public async Task<Result<Expense>> Add(Expense expense)
        {
            var result = await _expenseRepo.Add(expense).CAF();

            if (result != default)
            {
                return new Result<Expense>(ResponseCode.Ok, result);
            }

            _logger.LogWarning("Cannot add the expense.");
            return new Result<Expense>(ResponseCode.Error, default);
        }

        /// <inheritdoc/>
        public async Task<Result<Expense>> Update(int id, Expense expense)
        {
            var result = await _expenseRepo.Update(id, expense).CAF();

            if (result != default)
            {
                _logger.LogWarning($"Cannot update the expense of the {id}.");
                return new Result<Expense>(ResponseCode.Ok, result);
            }

            return new Result<Expense>(ResponseCode.Error, default);
        }

        /// <inheritdoc/>
        public async Task<ResponseCode> Remove(int id)
        {
            var result = await _expenseRepo.Remove(id).CAF();

            return result ? ResponseCode.Ok : ResponseCode.Error;
        }
    }
}
