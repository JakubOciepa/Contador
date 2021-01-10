﻿using Contador.Core.Models;
using Contador.Mobile.Services;

using Xamarin.Forms;

namespace Contador.Mobile.ViewModels
{
	/// <summary>
	/// View model for <see cref="Contador.Mobile.Controls.ExpenseControl"/> class.
	/// </summary>
	public class ExpenseControlViewModel : ViewModelBase
	{
		private readonly CategoryAvatarService _categoryAvatarService;
		private readonly UserAvatarService _userAvatarService;

		/// <summary>
		/// Gets Expense to display.
		/// </summary>
		public Expense Expense { get; }

		/// <summary>
		/// Gets the category glyph.
		/// </summary>
		//this should be taken from some service by the category name?
		public FontImageSource CategoryGlyph { get; private set; }

		/// <summary>
		/// Gets the user avatar glyph. This will be replaced when avatar will might be an image.
		/// </summary>
		//Same as above
		public ImageSource UserGlyph { get; private set; }

		/// <summary>
		/// Text color of the expense.
		/// </summary>
		public Color ExpenseColor { get; set; } = Color.Red;

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
