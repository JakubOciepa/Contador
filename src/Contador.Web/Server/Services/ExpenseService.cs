﻿using System.Collections.Generic;
using System.Threading.Tasks;

using Contador.Core.Common;
using Contador.Core.Models;
using Contador.Core.Utils.Extensions;
using Contador.DAL.Repositories;

using Microsoft.Extensions.Logging;

namespace Contador.Web.Server.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpensesRepository _expenseRepo;
        private readonly IExpenseCategoryService _expenseCategoryService;
        private readonly IUserService _usersService;
        private readonly ILogger<ExpenseService> _logger;

        /// <summary>
        /// Creates instance of <see cref="ExpenseService"/> class.
        /// </summary>
        /// <param name="expenses">Repository of expenses.</param>
        /// <param name="expenseCategory">Repository of expense categories.</param>
        /// <param name="users">Repository of users.</param>
        public ExpenseService(IExpensesRepository expenses, IExpenseCategoryService expenseCategory, IUserService users,
            ILogger<ExpenseService> logger)
        {
            _expenseRepo = expenses;
            _expenseCategoryService = expenseCategory;
            _usersService = users;
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
                list.Add(await GetExpenseApiFromCore(expense).CAF());
            }

            return new Result<IList<Expense>>(ResponseCode.Ok, list);
        }

        /// <inheritdoc/>
        public async Task<Result<Expense>> GetExpense(int id)
        {
            var result = await _expenseRepo.GetExpense(id).CAF();

            if (result == default)
            {
                _logger.LogInformation($"Expesne of the {id} not found.");
                return new Result<Expense>(ResponseCode.NotFound, default);
            }

            return new Result<Expense>(ResponseCode.Ok, Task.Run(() => GetExpenseApiFromCore(result)).Result);
        }

        /// <inheritdoc/>
        public async Task<Result<Expense>> Add(Expense expense)
        {
            var result = await _expenseRepo.Add(new DAL.Models.Expense(expense.Name, expense.Value, expense.User.Id, expense.Category.Id))
                       .CAF();

            if (result != default)
            {
                return new Result<Expense>(ResponseCode.Ok, Task.Run(() => GetExpenseApiFromCore(result)).Result);
            }

            _logger.LogWarning("Cannot add the expense.");
            return new Result<Expense>(ResponseCode.Error, default);
        }

        /// <inheritdoc/>
        public async Task<Result<Expense>> Update(int id, Expense expense)
        {
            var result = await _expenseRepo.Update(id, new DAL.Models.Expense(expense.Name, expense.Value, expense.User.Id, expense.Category.Id))
                       .CAF();

            if (result != default)
            {
                _logger.LogWarning($"Cannot update the expense of the {id}.");
                return new Result<Expense>(ResponseCode.Ok, Task.Run(() => GetExpenseApiFromCore(result)).Result);
            }

            return new Result<Expense>(ResponseCode.Error, default);
        }

        /// <inheritdoc/>
        public async Task<ResponseCode> Remove(int id)
        {
            var result = await _expenseRepo.Remove(id).CAF();

            return result ? ResponseCode.Ok : ResponseCode.Error;
        }

        private async Task<Expense> GetExpenseApiFromCore(DAL.Models.Expense coreExpense)
        {
            var category = await _expenseCategoryService.GetCategoryById(coreExpense.CategoryId).CAF();
            var user = _usersService.GetUserById(coreExpense.UserId);

            return new Expense(coreExpense.Name, coreExpense.Value, user,
                (ResponseCode)category.ResponseCode == ResponseCode.Ok ? category.ReturnedObject : default)
            {
                Id = coreExpense.Id,
            };
        }
    }
}
