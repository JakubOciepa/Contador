using System;
using System.Collections.ObjectModel;

using Contador.Abstractions;
using Contador.Core.Common;
using Contador.Core.Models;

using MvvmHelpers;

using TinyIoC;

namespace Contador.Mobile.ViewModels
{
	/// <summary>
	/// View model of the <see cref="Contador.Mobile.Pages.ExpensesListPage"/> class.
	/// </summary>
	public class ExpensesListPageViewModel : BaseViewModel
	{
		private readonly IExpenseService _expenseService;
		public ObservableCollection<ExpenseControlViewModel> Expenses { get; }

		/// <summary>
		/// Creates instance of the <see cref="ExpenseControlViewModel"/> class.
		/// </summary>
		public ExpensesListPageViewModel()
		{
			_expenseService = TinyIoCContainer.Current.Resolve<IExpenseService>();

			Expenses = new ObservableCollection<ExpenseControlViewModel>();

			LoadExpenses();
		}

		private void LoadExpenses()
		{
			var expenses = _expenseService.GetExpensesAsync().Result;

			if (expenses is object && (ResponseCode)expenses.ResponseCode == ResponseCode.Ok)
			{
				foreach (var expense in expenses.ReturnedObject)
				{
					Expenses.Add(new ExpenseControlViewModel(expense));
				}
			}
		}
	}
}
