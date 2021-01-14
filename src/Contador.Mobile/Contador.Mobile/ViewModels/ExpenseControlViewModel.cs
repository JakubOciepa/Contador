using Contador.Core.Models;
using Contador.Mobile.Services;

using MvvmHelpers;

using Xamarin.Forms;

namespace Contador.Mobile.ViewModels
{
	/// <summary>
	/// View model for <see cref="Contador.Mobile.Controls.ExpenseControl"/> class.
	/// </summary>
	public class ExpenseControlViewModel : BaseViewModel
	{
		private readonly CategoryAvatarService _categoryAvatarService;
		private readonly UserAvatarService _userAvatarService;

		private Expense _expense;
		private FontImageSource _categoryGlyph;
		private ImageSource _userGlyph;
		private Color _expenseColor;

		/// <summary>
		/// Gets Expense to display.
		/// </summary>
		public Expense Expense
		{
			get => _expense;
			private set => SetProperty(ref _expense, value);
		}

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
		/// Gets the user avatar glyph. This will be replaced when avatar will might be an image.
		/// </summary>
		//Same as above
		public ImageSource UserGlyph
		{
			get => _userGlyph;
			private set => SetProperty(ref _userGlyph, value);
		}

		/// <summary>
		/// Text color of the expense.
		/// </summary>
		public Color ExpenseColor
		{
			get => _expenseColor;
			set => SetProperty(ref _expenseColor, value);
		} 

		/// <summary>
		/// Command which will invoke on edit tap.
		/// </summary>
		public Command EditCommand { get; private set; }

		/// <summary>
		/// Creates instance of the <see cref="ExpenseControlViewModel"/> class.
		/// </summary>
		/// <param name="expense">Expense to display.</param>
		public ExpenseControlViewModel(Expense expense)
		{
			Expense = expense;
			_categoryAvatarService = new CategoryAvatarService();
			_userAvatarService = new UserAvatarService();
			ExpenseColor = Color.Red;

			InitializeProperties();
		}

		private void InitializeProperties()
		{
			CategoryGlyph = _categoryAvatarService.GetByCategoryName(Expense.Category.Name);
			UserGlyph = _userAvatarService.GetByUserName(Expense.User.Name);

			EditCommand = new Command(async _
				=> await Application.Current.MainPage.DisplayAlert("Tap triggered", "Edit button tapped!", "OK")
													  .ConfigureAwait(true));
		}
	}
}
