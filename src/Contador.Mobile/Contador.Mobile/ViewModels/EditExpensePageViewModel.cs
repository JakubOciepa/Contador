using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Contador.Abstractions;
using Contador.Core.Common;
using Contador.Core.Models;
using Contador.Mobile.Pages;

using MvvmHelpers;

using TinyIoC;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace Contador.Mobile.ViewModels
{
	public class EditExpensePageViewModel : BaseViewModel
	{
		private readonly IExpenseService _expenseService;
		private readonly IExpenseCategoryManager _categoryManager;
		private Expense _expense;

		private string _name;
		private decimal _value;
		private DateTime _createdDate;
		private ExpenseCategory _category;
		private string _description;
		private string _receiptImagePath;
		private Command _addCategoryCommand;
		private Command _saveChangesCommand;

		private IList<ExpenseCategory> _categories;

		public ObservableCollection<ExpenseCategory> Categories { get; }

		public string Name
		{
			get => _name;
			set => SetProperty(ref _name, value);
		}

		public decimal Value
		{
			get => _value;
			set => SetProperty(ref _value, value);
		}

		public DateTime CreatedDate
		{
			get => _createdDate;
			set => SetProperty(ref _createdDate, value);
		}

		public ExpenseCategory Category
		{
			get => _category;
			set => SetProperty(ref _category, value);
		}

		public string Description
		{
			get => _description;
			set => SetProperty(ref _description, value);
		}

		public string ReceiptImagePath
		{
			get => _receiptImagePath;
			set => SetProperty(ref _receiptImagePath, value);
		}

		public Command SaveChangesCommand
		{
			get => _saveChangesCommand;
			set => SetProperty(ref _saveChangesCommand, value);
		}

		public Command AddCategoryCommand
		{
			get => _addCategoryCommand;
			set => SetProperty(ref _addCategoryCommand, value);
		}

		public EditExpensePageViewModel(Expense expense = default)
		{
			_expenseService = TinyIoCContainer.Current.Resolve<IExpenseManager>();
			_categoryManager = TinyIoCContainer.Current.Resolve<IExpenseCategoryManager>();
			_expense = expense;

			Categories = new ObservableCollection<ExpenseCategory>();

			_categoryManager.CategoryAdded += AddedExpenseCategory;
			_categoryManager.CategoryUpdated += UpdatedExpenseCategory;
			_categoryManager.CategoryRemoved += RemovedExpenseCategory;

			SaveChangesCommand = new Command(SaveOrUpdate);
			AddCategoryCommand = new Command(AddCategory);
			SetupProperties();
		}

		private void RemovedExpenseCategory(object sender, int id)
		{
			var categoryToRemove = Categories.FirstOrDefault(c => c.Id == id);

			if (categoryToRemove is object)
			{
				MainThread.BeginInvokeOnMainThread(() => Categories.Remove(categoryToRemove));
			}
		}

		private void UpdatedExpenseCategory(object sender, ExpenseCategory category)
		{
			var categoryToUpdate = Categories.FirstOrDefault(c => c.Id == category.Id);

			if (categoryToUpdate is object)
			{
				var index = Categories.IndexOf(categoryToUpdate);

				MainThread.BeginInvokeOnMainThread(() => Categories[index] = category);
			}
		}

		private void AddedExpenseCategory(object sender, ExpenseCategory category)
		{
			MainThread.BeginInvokeOnMainThread(() => Categories.Add(category));
		}

		private async void AddCategory(object obj)
		{
			await Application.Current.MainPage.Navigation
				.PushAsync(new EditExpenseCategoryPage()
				{
					BindingContext = new EditExpenseCategoryPageViewModel()
				})
				.ConfigureAwait(true);
		}

		private async void SaveOrUpdate()
		{
			if (_expense is object)
			{
				UpdateExpense();
			}
			else
			{
				AddNewExpense();
			}

			await Application.Current.MainPage.Navigation.PopAsync()
				.ConfigureAwait(true);
		}

		private void UpdateExpense()
		{
			_expense.Name = Name;
			_expense.Value = Value;
			_expense.Category = Category;
			_expense.Description = Description;
			_expense.ImagePath = ReceiptImagePath;

			_expenseService.UpdateAsync(_expense.Id, _expense);
		}

		private void AddNewExpense()
		{
			var user = new User()
			{
				Name = "Kuba",
				Id = 1,
			};

			_expense = new Expense(Name, Value, user, Category)
			{
				CreateDate = CreatedDate,
				Description = Description,
				ImagePath = ReceiptImagePath,
			};

			_expenseService.AddAsync(_expense);
		}

		private async void SetupProperties()
		{
			await SetupCategories();
			SetupExpense();
		}

		private async Task SetupCategories()
		{
			var result = await _categoryManager.GetCategoriesAsync().ConfigureAwait(true);
			if (result.ResponseCode is ResponseCode.Ok)
			{
				_categories = result.ReturnedObject;
			}

			foreach (var category in _categories)
			{
				Categories.Add(category);
			}
		}

		private void SetupExpense()
		{
			if (_expense is object)
			{
				Name = _expense.Name;
				Value = _expense.Value;
				CreatedDate = _expense.CreateDate;
				Category = _categories.FirstOrDefault(cat => cat.Id == _expense.Category.Id);
				Description = _expense.Description;
				ReceiptImagePath = _expense.ImagePath;
			}
			else
			{
				CreatedDate = DateTime.Now;
			}
		}
	}
}
