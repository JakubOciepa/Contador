using System;

using Contador.Core.Models;

namespace Contador.Abstractions
{
	/// <summary>
	/// Notify on expense category changes.
	/// </summary>
	public interface IExpenseCategoryNotifier
	{
		/// <summary>
		/// Invoked when new(returned) category has been added.
		/// </summary>
		public event EventHandler<ExpenseCategory> CategoryAdded;

		/// <summary>
		/// Invoked when returned category has been updated.
		/// </summary>
		public event EventHandler<ExpenseCategory> CategoryUpdated;

		/// <summary>
		/// Invoked when category of returned id has been removed.
		/// </summary>
		public event EventHandler<int> CategoryRemoved;
	}
}
