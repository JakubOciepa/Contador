using System;

using Contador.Abstractions;
using Contador.Core.Common;
using Contador.Core.Models;

using MvvmHelpers;

using TinyIoC;

using Xamarin.Forms;

namespace Contador.Mobile.ViewModels
{
	public class EditExpenseCategoryPageViewModel : BaseViewModel
	{
		private readonly IExpenseCategoryService _categoryService;

		private ExpenseCategory _category;
		private Command _saveExpenseCategoryCommand;

		public ExpenseCategory Category
		{
			get => _category;
			set => SetProperty(ref _category, value);
		}

		public Command SaveExpenseCategoryCommand
		{
			get => _saveExpenseCategoryCommand;
			set => SetProperty(ref _saveExpenseCategoryCommand, value);
		}

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
				var result = _categoryService.AddExpenseCategoryAsync(Category).Result;

				if (result.ResponseCode is ResponseCode.Ok)
				{
					await Application.Current.MainPage.Navigation.PopAsync()
						.ConfigureAwait(true);
				}
			}
		}
	}
}
