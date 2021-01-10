using System;

using Contador.Core.Models;
using Contador.Mobile.ViewModels;

using Xamarin.Forms;

namespace Contador.Mobile.Pages
{
	public partial class MainPage : ContentPage
	{
		public Expense Expense => new Expense("Cuksy", 12.11m,
			new User() { Name = "Pysia" },
			new ExpenseCategory("Słodycze"))
		{
			CreateDate = DateTime.Today,
			Description = "Description",
		};

		public ExpenseControlViewModel ControlViewModel => new ExpenseControlViewModel(Expense);

		public MainPage()
		{
			InitializeComponent();
		}
	}
}
