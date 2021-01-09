using System;

using Contador.Core.Models;
using Contador.DAL.Abstractions;
using Contador.DAL.SQLite.Repositories;
using Contador.Mobile.DAL;
using Contador.Mobile.ViewModels;

using SQLite;

using TinyIoC;

using Xamarin.Forms;

namespace Contador.Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            _ = new DbConnection();

            RegisterServices(TinyIoCContainer.Current);

            MainPage = new Pages.MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        private void RegisterServices(TinyIoCContainer container)
        {
            container.Register<SQLiteAsyncConnection>((_, __) => DbConnection.Database);

            container.Register<IExpenseCategoryRepository, ExpenseCategoryRepository>();
            container.Register<IExpenseRepository, ExpenseRepository>();
            container.Register<IExpenseCategoryRepository, ExpenseCategoryRepository>();

            container.Register<ExpensesListPageViewModel>();
            var model = container.Resolve<ExpensesListPageViewModel>();
            container.BuildUp(this);

            MockSomeExpenses(container);
        }

        private async void MockSomeExpenses(TinyIoCContainer container)
        {
            var expenseRepo = container.Resolve<IExpenseRepository>();

            var expense = new Expense("Czekoladki", 12.11m,
            new User() { Name = "Pysia" },
            new ExpenseCategory("Słodycze"))
            {
                CreateDate = DateTime.Today,
                Description = "Description",
            };

            //await expenseRepo.AddExpenseAsync(expense);

            var category = new ExpenseCategory("Słodycze");
            var expenseCategoryRepository = container.Resolve<IExpenseCategoryRepository>();

            //await expenseCategoryRepository.AddCategoryAsync(category);

            var categories = await expenseCategoryRepository.GetCategoriesAsync();
            var cat = await expenseCategoryRepository.GetCategoryByIdAsync(2);
            await expenseRepo.UpdateExpenseAsync(1, expense);
        }
    }
}
