using System;
using System.Threading.Tasks;

using Contador.Abstractions;
using Contador.Core.Models;
using Contador.DAL.Abstractions;
using Contador.DAL.SQLite.Repositories;
using Contador.Mobile.DAL;
using Contador.Mobile.ViewModels;
using Contador.Services;

using Plugin.SharedTransitions;

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
			var container = TinyIoCContainer.Current;
			RegisterServices(container);

			var mainVM = container.Resolve<MainViewModel>();

			MainPage = new SharedTransitionNavigationPage(new Pages.MainPage() { BindingContext = mainVM })
			{
				BarBackgroundColor = Color.Transparent
			};
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

			//repositories
			container.Register<IExpenseCategoryRepository, ExpenseCategoryRepository>();
			container.Register<IExpenseRepository, ExpenseRepository>();
			container.Register<IExpenseCategoryRepository, ExpenseCategoryRepository>();

			//services
			container.Register<IExpenseManager, ExpenseService>().AsSingleton();
			container.Register<IExpenseCategoryManager, ExpenseCategoryService>().AsSingleton();

			//viewmodels
			container.Register<ExpensesListPageViewModel>();
			container.Register<MainViewModel>();

			container.BuildUp(this);

			//await MockSomeExpenses(container);
		}

		private async Task MockSomeExpenses(TinyIoCContainer container)
		{
			var category = new ExpenseCategory("Słodycze");
			var expenseCategoryRepository = container.Resolve<IExpenseCategoryRepository>();

			await expenseCategoryRepository.AddCategoryAsync(category);

			var categories = await expenseCategoryRepository.GetCategoriesAsync();
			var cat = await expenseCategoryRepository.GetCategoryByIdAsync(2);

			category.Name = "Słodkości";
			var expenseRepo = container.Resolve<IExpenseRepository>();

			var expense = new Expense("Czekoladki", 12.11m,
			new User() { Id = "1", UserName = "Pysia" },
			new ExpenseCategory("Słodycze"))
			{
				Id = 1,
				CreateDate = DateTime.Today,
				Description = "Description",
			};

			expense.Category.Id = 1;
			await expenseRepo.AddAsync(expense);
			await expenseRepo.AddAsync(expense);
			await expenseRepo.AddAsync(expense);

			//await expenseRepo.UpdateExpenseAsync(1, expense);
			//await expenseRepo.RemoveExpenseAsync(10);
			//await expenseCategoryRepository.RemoveCategoryAsync(1);
			//await expenseCategoryRepository.UpdateCategoryAsync(2, category);
		}
	}
}
