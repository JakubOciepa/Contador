using System.Windows.Input;

using Contador.Abstractions;
using Contador.Core.Models;
using Contador.Mobile.Pages;
using Contador.Mobile.Services;

using MvvmHelpers;

using TinyIoC;

using Xamarin.Forms;

namespace Contador.Mobile.ViewModels
{
	/// <summary>
	/// View model for <see cref="Controls.ExpenseControl"/> class.
	/// </summary>
	public class ExpenseControlViewModel : BaseViewModel
	{
		private readonly CategoryAvatarService _categoryAvatarService;
		private readonly UserAvatarService _userAvatarService;
		private readonly IExpenseService _expenseService;

		private FontImageSource _categoryGlyph;
		private Expense _expense;
		private Color _expenseColor;
		private ImageSource _userGlyph;

		/// <summary>
		/// Gets the category glyph.
		/// </summary>
		//this should be taken from some service by the category name?
		public FontImageSource CategoryGlyph
		{
			get => _categoryGlyph;
			private set => SetProperty(ref _categoryGlyph, value);
		}

		/// <summary>
		/// Gets Expense to display.
		/// </summary>
		public Expense Expense
		{
			get => _expense;
			set => SetProperty(ref _expense, value);
		}

		/// <summary>
		/// Gets or sets color of the expense text.
		/// </summary>
		public Color ExpenseColor
		{
			get => _expenseColor;
			set => SetProperty(ref _expenseColor, value);
		}

		/// <summary>
		/// Gets the user avatar glyph. This will be replaced when avatar will might be an image.
		/// </summary>
		//Same as above
		public ImageSource UserGlyph
		{
			get => _userGlyph;
			private set => SetProperty(ref _userGlyph, value);
		}

		/// <summary>
		/// Gets the command which will be invoked on edit tap.
		/// </summary>
		public ICommand EditCommand { get; private set; }

		/// <summary>
		/// Gets the command that will be invoked on remove tap.
		/// </summary>
		public ICommand RemoveCommand { get; private set; }

		/// <summary>
		/// Creates instance of the <see cref="ExpenseControlViewModel"/> class.
		/// </summary>
		/// <param name="expense">Expense to display.</param>
		public ExpenseControlViewModel(Expense expense)
		{
			_expenseService = TinyIoCContainer.Current.Resolve<IExpenseManager>();

			Expense = expense;
			_categoryAvatarService = new CategoryAvatarService();
			_userAvatarService = new UserAvatarService();

			InitializeProperties();
		}

		/// <summary>
		/// Updates the <see cref="Expense"/> of the control.
		/// </summary>
		/// <param name="expense"><see cref="Expense"/> info.</param>
		public void UpdateExpense(Expense expense)
		{
			Expense = expense;
			OnPropertyChanged(nameof(Expense));
		}

		private void InitializeProperties()
		{
			CategoryGlyph = _categoryAvatarService.GetByCategoryName(Expense.Category.Name);
			UserGlyph = _userAvatarService.GetByUserName(Expense.User.Name);

			ExpenseColor = Color.Red;
			RemoveCommand = new Command(RemoveExpense);

			EditCommand = new Command(async _
				=> await Application.Current.MainPage.Navigation
					.PushAsync(new EditExpensePage() { BindingContext = new EditExpensePageViewModel(Expense) }));
		}

		private async void RemoveExpense(object obj)
		{
			var wantRemove = await Application.Current.MainPage.DisplayAlert("Usuwanie", "Czy na pewno chcesz usunąć ten wydatek?", "TAK", "NIE")
				.ConfigureAwait(true);

			if (wantRemove)
			{
				_ = _expenseService.RemoveAsync(Expense.Id);
			}
		}
	}
}
