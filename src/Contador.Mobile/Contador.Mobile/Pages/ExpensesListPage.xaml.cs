using System;

using Contador.Core.Models;
using Contador.Mobile.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Contador.Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExpensesListPage : ContentView
    {
        public Expense Expense => new Expense("Cuksy", 12.11m,
            new User() { Name = "Pysia" },
            new ExpenseCategory("Słodycze"))
        {
            CreateDate = DateTime.Today,
            Description = "Description",
        };

        public ExpenseControlViewModel ControlViewModel => new ExpenseControlViewModel(Expense);

        public ExpensesListPage()
        {
            InitializeComponent();
        }
    }
}
