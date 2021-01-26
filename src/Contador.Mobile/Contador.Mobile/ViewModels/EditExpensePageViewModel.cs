
using Contador.Abstractions;

using MvvmHelpers;

using TinyIoC;

namespace Contador.Mobile.ViewModels
{
	public class EditExpensePageViewModel : BaseViewModel
	{
		private readonly IExpenseService _expenseService;

		public EditExpensePageViewModel()
		{
			_expenseService = TinyIoCContainer.Current.Resolve<IExpenseService>();
		}
	}
}
