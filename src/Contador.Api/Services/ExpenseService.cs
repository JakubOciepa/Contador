using System.Collections.Generic;

using Contador.Api.Models;
using Contador.Core.Common;
using Contador.DAL.Repositories.Interfaces;

namespace Contador.Api.Services
{
    /// <summary>
    /// Expense manager.
    /// </summary>
    public class ExpenseService : IExpenseService
    {
        private readonly IExpensesRepository _expenseRepo;
        private readonly IExpenseCategoryRepository _expenseCategoryRepo;
        private readonly IUsersRepository _userRepository;

        /// <summary>
        /// Creates instance of <see cref="ExpenseService"/> class.
        /// </summary>
        /// <param name="expenses">Repository of expenses.</param>
        /// <param name="expenseCategory">Repository of expense categories.</param>
        /// <param name="users">Repository of users.</param>
        public ExpenseService(IExpensesRepository expenses, IExpenseCategoryRepository expenseCategory, IUsersRepository users)
        {
            _expenseRepo = expenses;
            _expenseCategoryRepo = expenseCategory;
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

            var category = _expenseCategoryRepo.GetCategoryById(result.CategoryId);
            var user = _userRepository.GetUserById(result.UserId);
            var expense = new Expense(result.Id, result.Name, result.Value, user, category);

            return new Result<ResponseCode, Expense>(ResponseCode.Ok, expense);
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
                var user = _userRepository.GetUserById(expense.UserId);
                var category = _expenseCategoryRepo.GetCategoryById(expense.CategoryId);

                list.Add(new Expense(expense.Id, expense.Name, expense.Value, user, category));
            }

            return new Result<ResponseCode, IList<Expense>>(ResponseCode.Ok, list);
        }
    }
}
