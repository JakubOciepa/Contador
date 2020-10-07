using System.Collections.Generic;

using Contador.Api.Models;
using Contador.Api.Services.Interfaces;
using Contador.Core.Common;
using Contador.DAL.Repositories;

namespace Contador.Api.Services
{
    /// <summary>
    /// Expense manager.
    /// </summary>
    public class ExpenseService : IExpenseService
    {
        private readonly IExpensesRepository _expenseRepo;
        private readonly IExpenseCategoryService _expenseCategoryService;
        private readonly IUsersRepository _userRepository;

        /// <summary>
        /// Creates instance of <see cref="ExpenseService"/> class.
        /// </summary>
        /// <param name="expenses">Repository of expenses.</param>
        /// <param name="expenseCategory">Repository of expense categories.</param>
        /// <param name="users">Repository of users.</param>
        public ExpenseService(IExpensesRepository expenses, IExpenseCategoryService expenseCategory, IUsersRepository users)
        {
            _expenseRepo = expenses;
            _expenseCategoryService = expenseCategory;
            _userRepository = users;
        }

        /// <inheritdoc/>
        public Result<ResponseCode, Expense> GetExpense(int id)
        {
            var result = _expenseRepo.GetExpense(id);

            if (result == default)
            {
                return new Result<ResponseCode, Expense>(ResponseCode.NotFound, default);
            }

            return new Result<ResponseCode, Expense>(ResponseCode.Ok, GetExpenseApiFromCore(result));
        }

        /// <inheritdoc/>
        public Result<ResponseCode, IList<Expense>> GetExpenses()
        {
            var result = _expenseRepo.GetExpenses();

            if (result.Count == 0)
            {
                return new Result<ResponseCode, IList<Expense>>(ResponseCode.NotFound, new List<Expense>());
            }

            var list = new List<Expense>();

            foreach (var expense in result)
            {
                list.Add(GetExpenseApiFromCore(expense));
            }

            return new Result<ResponseCode, IList<Expense>>(ResponseCode.Ok, list);
        }

        /// <inheritdoc/>
        public Result<ResponseCode, Expense> Add(Expense expense)
        {
            var result = _expenseRepo.Add(new Core.Models.Expense(expense.Name, expense.Value, expense.User.Id, expense.Category.Id));

            if (result != default)
            {
                return new Result<ResponseCode, Expense>(ResponseCode.Ok, GetExpenseApiFromCore(result));
            }

            return new Result<ResponseCode, Expense>(ResponseCode.Error, default);
        }


        /// <inheritdoc/>
        public Result<ResponseCode, Expense> Update(int id, Expense expense)
        {
            var result = _expenseRepo.Update(id, new Core.Models.Expense(expense.Name, expense.Value, expense.User.Id, expense.Category.Id));

            if (result != default)
            {
                return new Result<ResponseCode, Expense>(ResponseCode.Ok, GetExpenseApiFromCore(result));
            }

            return new Result<ResponseCode, Expense>(ResponseCode.Error, default);
        }

        /// <inheritdoc/>
        public ResponseCode Remove(int id)
        {
            var result = _expenseRepo.Remove(id);

            return result ? ResponseCode.Ok : ResponseCode.Error;
        }

        private Expense GetExpenseApiFromCore(Core.Models.Expense coreExpense)
        {
            var category = _expenseCategoryService.GetCategoryById(coreExpense.CategoryId);
            var user = _userRepository.GetUserById(coreExpense.UserId);

            return new Expense(coreExpense.Name, coreExpense.Value, user, category)
            {
                Id = coreExpense.Id,
            };
        }
    }
}
