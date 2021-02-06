using Contador.Mobile.Pages;

using MvvmHelpers;

using Xamarin.Forms;

namespace Contador.Mobile.ViewModels
{
	/// <summary>
	/// View model for the <see cref="AddingPage"/> class.
	/// </summary>
	public class AddingPageViewModel : BaseViewModel
	{
		/// <summary>
		/// Command which will be invoked on Add expense tap.
		/// </summary>
		public Command AddExpenseCommand { get; set; }

		/// <summary>
		/// Initializes instance of the <see cref="AddingPageViewModel"/> class.
		/// </summary>
		public AddingPageViewModel()
		{
			AddExpenseCommand = new Command(
				async () => await Application.Current.MainPage.Navigation
					.PushAsync(new EditExpensePage() { BindingContext = new EditExpensePageViewModel() }));
		}
	}
}
