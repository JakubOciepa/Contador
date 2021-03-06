﻿using System;
using System.Collections.ObjectModel;
using System.Linq;

using Contador.Core.Common;
using Contador.Services.Interfaces;

using MvvmHelpers;

using TinyIoC;

namespace Contador.Mobile.ViewModels
{
	/// <summary>
	/// View model of the <see cref="Contador.Mobile.Pages.ExpensesListPage"/> class.
	/// </summary>
	public class ExpensesListPageViewModel : BaseViewModel, IDisposable
	{
		private readonly IExpenseManager _expenseManager;

		/// <summary>
		/// Gets available <see cref="ExpenseControlViewModel"/> objects.
		/// </summary>
		public ObservableCollection<ExpenseControlViewModel> Expenses { get; }

		/// <summary>
		/// Creates instance of the <see cref="ExpenseControlViewModel"/> class.
		/// </summary>
		public ExpensesListPageViewModel()
		{
			_expenseManager = TinyIoCContainer.Current.Resolve<IExpenseManager>();

			Expenses = new ObservableCollection<ExpenseControlViewModel>();

			_expenseManager.ExpenseAdded += ExpenseAdded;
			_expenseManager.ExpenseUpdated += ExpenseUpdated;
			_expenseManager.ExpenseRemoved += ExpenseRemoved;

			LoadExpenses();
		}

		private void ExpenseAdded(object sender, Core.Models.Expense expense)
		{
			Expenses.Add(new ExpenseControlViewModel(expense));
		}

		private void ExpenseRemoved(object sender, int expenseId)
		{
			var expenseToRemove = Expenses.FirstOrDefault(ex => ex.Expense.Id == expenseId);
			Expenses.Remove(expenseToRemove);
		}

		private void ExpenseUpdated(object sender, Core.Models.Expense expense)
		{
			var updatedExpense = Expenses.FirstOrDefault(ex => ex.Expense.Id == expense.Id);
			updatedExpense.UpdateExpense(expense);

			OnPropertyChanged(nameof(Expenses));
		}

		private async void LoadExpenses()
		{
			var expenses = await _expenseManager.GetAllAsync()
				.ConfigureAwait(true);

			if (expenses is object && (ResponseCode)expenses.ResponseCode == ResponseCode.Ok)
			{
				foreach (var expense in expenses.ReturnedObject)
				{
					Expenses.Add(new ExpenseControlViewModel(expense));
				}
			}
		}

		public void Dispose()
		{
			if (_expenseManager is object)
			{
				_expenseManager.ExpenseAdded -= ExpenseAdded;
				_expenseManager.ExpenseUpdated -= ExpenseUpdated;
				_expenseManager.ExpenseRemoved -= ExpenseRemoved;
			}
		}
	}
}
