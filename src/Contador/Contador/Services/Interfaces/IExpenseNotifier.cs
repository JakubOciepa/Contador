using System;

using Contador.Core.Models;

namespace Contador.Abstractions
{
	/// <summary>
	/// Notify on expense changes.
	/// </summary>
	public interface IExpenseNotifier
	{
		/// <summary>
		/// Invoked when new(returned) expense has been added.
		/// </summary>
		event EventHandler<Expense> ExpenseAdded;

		/// <summary>
		/// Invoked when returned expense has been updated.
		/// </summary>
		event EventHandler<Expense> ExpenseUpdated;

		/// <summary>
		/// Invoked when expense of returned id has been removed.
		/// </summary>
		event EventHandler<int> ExpenseRemoved;
	}
}
