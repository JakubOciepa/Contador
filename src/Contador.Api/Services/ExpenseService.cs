using Contador.Api.Models;
using Contador.Core.Common;
using Contador.DAL.Repositories;
using Contador.DAL.Repositories.Interfaces;
using System.Collections.Generic;

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

        /// <summary>
        /// Gets all available expenses.
        /// </summary>
        /// <returns>Result wich proper response code and list of expenses.</returns>
        public Result<ResponseCode, IList<Expense>> GetExpenses()
        {
            var dbExpenses = _expenseRepo.GetExpenses();

            if (dbExpenses.Count == 0)
            {
                return new Result<ResponseCode, IList<Expense>>(ResponseCode.NotFound, new List<Expense>());
            }

            var list = new List<Expense>();

            foreach (var expense in dbExpenses)
            {
                var user = _userRepository.GetUserById(expense.UserId);
                var category = _expenseCategoryRepo.GetCategoryById(expense.CategoryId);

                list.Add(new Expense(expense.Name, expense.Value, user, category));
            }

            return new Result<ResponseCode, IList<Expense>>(ResponseCode.Ok, list);
        }
    }
}
