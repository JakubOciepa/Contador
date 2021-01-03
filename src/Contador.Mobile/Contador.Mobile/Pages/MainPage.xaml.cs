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

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var animation = new Animation();
            animation.WithConcurrent((g) => MyGrid.HeightRequest = g, 70, 150, Easing.SpringOut);
            animation.Commit(Page, "ResizeAnimation", length: 3000);
        }
    }
}
