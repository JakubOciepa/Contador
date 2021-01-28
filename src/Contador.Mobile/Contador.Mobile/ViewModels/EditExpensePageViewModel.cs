using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Contador.Abstractions;
using Contador.Core.Common;
using Contador.Core.Models;

using MvvmHelpers;

using TinyIoC;

using Xamarin.Forms;

namespace Contador.Mobile.ViewModels
{
	public class EditExpensePageViewModel : BaseViewModel
	{
		private readonly IExpenseService _expenseService;
		private readonly IExpenseCategoryService _categoryService;
		private Expense _expense;

		private string _name;
		private decimal _value;
		private DateTime _createdDate;
		private ExpenseCategory _category;
		private string _description;
		private string _receiptImagePath;
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

		public EditExpensePageViewModel(Expense expense = default)
		{
			_expenseService = TinyIoCContainer.Current.Resolve<IExpenseService>();
			_categoryService = TinyIoCContainer.Current.Resolve<IExpenseCategoryService>();
			_expense = expense;

			Categories = new ObservableCollection<ExpenseCategory>();

			SaveChangesCommand = new Command(SaveOrUpdate);

			SetupProperties();
		}

		private void SaveOrUpdate()
		{
			if (_expense is object)
			{
				UpdateExpense();
			}
			else
			{
				AddNewExpense();
			}
		}

		private async void UpdateExpense()
		{
			_expense.Name = Name;
			_expense.Value = Value;
			_expense.Category = Category;
			_expense.Description = Description;
			_expense.ImagePath = ReceiptImagePath;

			await _expenseService.UpdateAsync(_expense.Id, _expense);
		}

		private async void AddNewExpense()
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

			await _expenseService.AddAsync(_expense);
		}

		private async void SetupProperties()
		{
			SetupExpense();
			await SetupCategories();
		}

		private async Task SetupCategories()
		{
			var result = await _categoryService.GetCategoriesAsync();
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
				Category = _expense.Category;
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
