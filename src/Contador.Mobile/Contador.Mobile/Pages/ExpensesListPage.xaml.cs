using System;
using System.Collections.ObjectModel;

using Contador.Core.Models;
using Contador.Mobile.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Contador.Mobile.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ExpensesListPage : ContentView
	{
		public ExpensesListPage()
		{
			InitializeComponent();
		}
	}
}
