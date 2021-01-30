using System;

using Contador.Core.Models;

namespace Contador.Abstractions
{
	public interface IExpenseNotifier
	{
		event EventHandler<Expense> ExpenseAdded;

		event EventHandler<Expense> ExpenseUpdated;

		event EventHandler<int> ExpenseRemoved;
	}
}
