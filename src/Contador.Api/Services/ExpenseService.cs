using Contador.Api.Models;
using Contador.DAL.Repositories;
using System.Collections.Generic;

namespace Contador.Api.Services
{
    public class ExpenseService
    {
        private readonly ExpensesRepository _expenseRepo;
        private readonly ExpenseCategoryRepository _expenseCategoryRepo;

        public ExpenseService(ExpensesRepository expenses, ExpenseCategoryRepository expenseCategory)
        {
            _expenseRepo = expenses;
            _expenseCategoryRepo = expenseCategory;
        }

        public IEnumerable<Expense> GetExpenses()
        {
            var dbExpenses = _expenseRepo.GetExpenses();

            if (dbExpenses.Count == 0)
                return null;

            return MapExpensesToApi(dbExpenses);
        }

        private IEnumerable<Expense> MapExpensesToApi(IEnumerable<Core.Models.Expense> dbExpenses)
        {
            foreach (var expense in dbExpenses)
            {
                yield return new Expense(expense.Name, expense.Value, expense.UserId, _expenseCategoryRepo.GetCategoryNameById(expense.CategoryId));
            }
        }
    }
}
