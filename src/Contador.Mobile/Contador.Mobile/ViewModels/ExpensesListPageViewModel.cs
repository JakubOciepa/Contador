using System;
using System.Collections.ObjectModel;

using Contador.Abstractions;
using Contador.Core.Common;
using Contador.Core.Models;
using Contador.DAL.Abstractions;

using TinyIoC;

namespace Contador.Mobile.ViewModels
{
	/// <summary>
	/// View model of the <see cref="Contador.Mobile.Pages.ExpensesListPage"/> class.
	/// </summary>
	public class ExpensesListPageViewModel : ViewModelBase
	{
		private readonly IExpenseService _expenseService;

		private ObservableCollection<ExpenseControlViewModel> _expenses;

		public ObservableCollection<ExpenseControlViewModel> Expenses
		{
			get => _expenses;
			set => SetField(ref _expenses, value);
		}

		/// <summary>
		/// Creates instance of the <see cref="ExpenseControlViewModel"/> class.
		/// </summary>
		/// <param name="expenseService"><see cref="IExpenseRepository"/> expense repository.</param>
		public ExpensesListPageViewModel()
		{
			_expenseService = TinyIoCContainer.Current.Resolve<IExpenseService>();
			Expenses = new ObservableCollection<ExpenseControlViewModel>()
			{
				new ExpenseControlViewModel(new Expense("Cuksy", 12.11m,
			new User() { Name = "Pysia" },
			new ExpenseCategory("Słodycze"))
					{
						CreateDate = DateTime.Today,
						Description = "Description",
					})
			};
		}

		private ObservableCollection<ExpenseControlViewModel> LoadExpenses()
		{
			var collection = new ObservableCollection<ExpenseControlViewModel>();
			var expenses = _expenseService.GetExpensesAsync().Result;

			if (expenses is object && (ResponseCode)expenses.ResponseCode == ResponseCode.Ok)
			{
				foreach (var expense in expenses.ReturnedObject)
				{
					collection.Add(new ExpenseControlViewModel(expense));
				}
			}

			return collection;
		}
	}
}
