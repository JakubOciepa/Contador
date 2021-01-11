using Contador.DAL.Abstractions;

namespace Contador.Mobile.ViewModels
{
	/// <summary>
	/// View model of the <see cref="Contador.Mobile.Pages.ExpensesListPage"/> class.
	/// </summary>
	public class ExpensesListPageViewModel : ViewModelBase
	{
		/// <summary>
		/// Creates instance of the <see cref="ExpenseControlViewModel"/> class.
		/// </summary>
		/// <param name="expenserepo"><see cref="IExpenseRepository"/> expense repository.</param>
		public ExpensesListPageViewModel(IExpenseRepository expenserepo)
		{
			var repo = expenserepo;
		}
	}
}
