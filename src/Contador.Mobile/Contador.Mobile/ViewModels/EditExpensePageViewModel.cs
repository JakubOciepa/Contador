using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

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
	/// <summary>
	/// View model for the <see cref="EditExpensePage"/> view.
	/// </summary>
	public class EditExpensePageViewModel : BaseViewModel
	{
		private readonly IExpenseService _expenseService;
		private readonly IExpenseCategoryManager _categoryManager;

		private Expense _expense;

		private ExpenseCategory _category;
		private DateTime _createdDate;
		private string _description;
		private string _name;
		private string _receiptImagePath;
		private decimal _value;

		/// <summary>
		/// Gets the available <see cref="ExpenseCategory"/>.
		/// </summary>
		public ObservableCollection<ExpenseCategory> Categories { get; }

		/// <summary>
		/// Gets or sets current category of the <see cref="Expense"/>.
		/// </summary>
		public ExpenseCategory Category
		{
			get => _category;
			set => SetProperty(ref _category, value);
		}

		/// <summary>
		/// Gets or sets the date of the <see cref="Expense"/> creation.
		/// </summary>
		public DateTime CreatedDate
		{
			get => _createdDate;
			set => SetProperty(ref _createdDate, value);
		}

		/// <summary>
		/// Gets or set the description of the <see cref="Expense"/>.
		/// </summary>
		public string Description
		{
			get => _description;
			set => SetProperty(ref _description, value);
		}

		/// <summary>
		/// Gets or sets the Name of the <see cref="Expense"/>.
		/// </summary>
		public string Name
		{
			get => _name;
			set => SetProperty(ref _name, value);
		}

		/// <summary>
		/// Gets or sets the path of the receipt image.
		/// </summary>
		public string ReceiptImagePath
		{
			get => _receiptImagePath;
			set => SetProperty(ref _receiptImagePath, value);
		}

		/// <summary>
		/// Gets or sets the value of the <see cref="Expense"/>.
		/// </summary>
		public decimal Value
		{
			get => _value;
			set => SetProperty(ref _value, value);
		}

		/// <summary>
		/// Gets the command that will be invoked after tap on add category button.
		/// </summary>
		public ICommand AddCategoryCommand { get; private set; }

		/// <summary>
		/// Gets the command that will be invoked on appearing of the <see cref="EditExpensePage"/> view.
		/// </summary>
		public ICommand AppearingCommand { get; private set; }

		/// <summary>
		/// Gets the command that will be invoked after tap on Save button.
		/// </summary>
		public ICommand SaveChangesCommand { get; private set; }

		/// <summary>
		/// Creates instance of the <see cref="EditExpensePageViewModel"/> class.
		/// </summary>
		/// <param name="expense"><see cref="Expense"/> to edit.</param>
		public EditExpensePageViewModel(Expense expense = default)
		{
			_expenseService = TinyIoCContainer.Current.Resolve<IExpenseManager>();
			_categoryManager = TinyIoCContainer.Current.Resolve<IExpenseCategoryManager>();

			_expense = expense;

			Categories = new ObservableCollection<ExpenseCategory>();

			SaveChangesCommand = new Command(SaveOrUpdate);
			AddCategoryCommand = new Command(AddCategory);

			AppearingCommand = new Command(Appearing);

			_categoryManager.CategoryAdded += AddedExpenseCategory;
			_categoryManager.CategoryUpdated += UpdatedExpenseCategory;
			_categoryManager.CategoryRemoved += RemovedExpenseCategory;
		}

		private async void Appearing(object obj)
		{
			Categories.Clear();

			await SetupCategories().ConfigureAwait(true);

			SetupExpense();
		}

		private async Task SetupCategories()
		{
			var result = await _categoryManager.GetCategoriesAsync().ConfigureAwait(true);
			if (result.ResponseCode is ResponseCode.Ok)
			{
				foreach (var category in result.ReturnedObject)
				{
					Categories.Add(category);
				}
			}
		}

		private void SetupExpense()
		{
			if (_expense is object)
			{
				Name = _expense.Name;
				Value = _expense.Value;
				CreatedDate = _expense.CreateDate;
				Category = Categories.FirstOrDefault(cat => cat.Id == _expense.Category.Id);
				Description = _expense.Description;
				ReceiptImagePath = _expense.ImagePath;
			}
			else
			{
				CreatedDate = DateTime.Now;
			}
		}
		private void AddedExpenseCategory(object sender, ExpenseCategory category)
		{
			MainThread.BeginInvokeOnMainThread(() => Categories.Add(category));
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

			await Application.Current.MainPage.DisplayAlert("Zapisane!", "Wydatek został zapisany...", "OK")
				.ConfigureAwait(true);

			_ = Application.Current.MainPage.Navigation.PopAsync().ConfigureAwait(true);
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

			_expense = (await _expenseService.AddAsync(_expense)).ReturnedObject;
		}
		private async void UpdateExpense()
		{
			_expense.Name = Name;
			_expense.Value = Value;
			_expense.Category = Category;
			_expense.Description = Description;
			_expense.ImagePath = ReceiptImagePath;

			_expense = (await _expenseService.UpdateAsync(_expense.Id, _expense)).ReturnedObject;
		}

		private void AddCategory(object obj)
		{
			_ = Application.Current.MainPage.Navigation.PushAsync(
				new EditExpenseCategoryPage()
				{
					BindingContext = new EditExpenseCategoryPageViewModel()
				});
		}
	}
}
