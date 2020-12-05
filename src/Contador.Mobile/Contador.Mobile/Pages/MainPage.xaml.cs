using System;
using System.ComponentModel;
using System.Threading.Tasks;

using Contador.Core.Models;

using Xamarin.Forms;

namespace Contador.Mobile.Pages
{
    public partial class MainPage : ContentPage
    {
        public Expense Expense => new Expense("Cuksy", 12.11m, null, null) 
        {
            CreateDate = DateTime.Today,
        };

        public MainPage()
        {
            InitializeComponent();
        }
    }
}
