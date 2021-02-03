using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Contador.Core.Models;

namespace Contador.Abstractions
{
	public interface IExpenseCategoryNotifier
	{
		public event EventHandler<ExpenseCategory> CategoryAdded;

		public event EventHandler<ExpenseCategory> CategoryUpdated;

		public event EventHandler<int> CategoryRemoved;
	}
}
