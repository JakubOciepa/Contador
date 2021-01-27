using System;

using Contador.Abstractions;
using Contador.Core.Models;

using MvvmHelpers;

using TinyIoC;

namespace Contador.Mobile.ViewModels
{
	public class EditExpensePageViewModel : BaseViewModel
	{
		private readonly IExpenseService _expenseService;
		private Expense _expense;

		private string _name;
		private decimal _value;
		private DateTime _createdDate;
		private ExpenseCategory _category;
		private string _description;
		private string _receiptImagePath;

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

		public EditExpensePageViewModel(Expense expense = default)
		{
			_expenseService = TinyIoCContainer.Current.Resolve<IExpenseService>();
			_expense = expense;
			SetupProperties();
		}

		private void SetupProperties()
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
