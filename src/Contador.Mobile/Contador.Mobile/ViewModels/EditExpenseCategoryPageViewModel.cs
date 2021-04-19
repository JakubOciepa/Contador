using Contador.Abstractions;
using Contador.Core.Common;
using Contador.Core.Models;

using MvvmHelpers;

using TinyIoC;

using Xamarin.Forms;

namespace Contador.Mobile.ViewModels
{
	/// <summary>
	/// View model for the <see cref="Pages.EditExpenseCategoryPage"/> view.
	/// </summary>
	public class EditExpenseCategoryPageViewModel : BaseViewModel
	{
		private readonly IExpenseCategoryService _categoryService;
		private ExpenseCategory _category;

		/// <summary>
		/// Gets or sets the <see cref="ExpenseCategory"/> property.
		/// </summary>
		public ExpenseCategory Category
		{
			get => _category;
			set => SetProperty(ref _category, value);
		}

		/// <summary>
		/// Gets or sets the command that will be invoked after tap on the save button.
		/// </summary>
		public Command SaveExpenseCategoryCommand { get; set; }

		/// <summary>
		/// Creates instance of the <see cref="EditExpenseCategoryPageViewModel"/> class.
		/// </summary>
		/// <param name="category">Expense category to edit.</param>
		public EditExpenseCategoryPageViewModel(ExpenseCategory category = null)
		{
			_categoryService = TinyIoCContainer.Current.Resolve<IExpenseCategoryManager>();

			_category = category;

			SaveExpenseCategoryCommand = new Command(SaveCategory);

			SetupCategory();
		}

		private void SetupCategory()
		{
			if (_category is null)
			{
				_category = new ExpenseCategory(string.Empty);
			}
		}

		private async void SaveCategory(object obj)
		{
			if (Category is object && !string.IsNullOrEmpty(Category.Name))
			{
				var result = _categoryService.AddAsync(Category).Result;

				if (result.ResponseCode is ResponseCode.Ok)
				{
					await Application.Current.MainPage.DisplayAlert("Zapisane!", "Kategoria została pomyślnie zapisana!", "OK")
						.ConfigureAwait(true);

					_ = Application.Current.MainPage.Navigation.PopAsync();
				}
			}
		}
	}
}
