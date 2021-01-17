using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MvvmHelpers;

using Xamarin.Forms;

namespace Contador.Mobile.ViewModels
{
    public class AddingPageViewModel : BaseViewModel
    {
        public Command AddExpenseCommand { get; set; }

		public AddingPageViewModel()
		{
			AddExpenseCommand = new Command(async () 
				=> await Application.Current.MainPage.DisplayAlert("Tap triggered", "Add expense button tapped!", "OK")
													  .ConfigureAwait(true));
		}

	}
}
