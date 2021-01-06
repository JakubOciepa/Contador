
using System;

using Contador.Core.Models;
using Contador.DAL.Abstractions;
using Contador.DAL.SQLite.Repositories;
using Contador.Mobile.DAL;

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
            container.Register<IExpenseRepository>((_, __) => new ExpenseRepository(DbConnection.Database));

            container.BuildUp(this);

            var expenseRepo = container.Resolve<IExpenseRepository>();

            var expense = new Expense("Cuksy", 12.11m,
            new User() { Name = "Pysia" },
            new ExpenseCategory("Słodycze"))
            {
                CreateDate = DateTime.Today,
                Description = "Description",
            };

            expenseRepo.AddExpenseAsync(expense);
        }
    }
}
