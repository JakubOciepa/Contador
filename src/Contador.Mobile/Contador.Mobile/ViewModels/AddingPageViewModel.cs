
using Contador.Mobile.Pages;

using MvvmHelpers;

using Xamarin.Forms;

namespace Contador.Mobile.ViewModels
{
	public class AddingPageViewModel : BaseViewModel
	{
		public Command AddExpenseCommand { get; set; }

		public AddingPageViewModel()
		{
			AddExpenseCommand = new Command(
				async () => await Application.Current.MainPage.Navigation
					.PushAsync(new EditExpensePage() { BindingContext = new EditExpensePageViewModel() }));
		}

	}
}
