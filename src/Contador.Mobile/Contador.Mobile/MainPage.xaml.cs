using System.Drawing.Printing;
using System.Net.Http;

using Contador.Services;

using Microsoft.Extensions.Logging;

using Xamarin.Forms;

namespace Contador.Mobile
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, System.EventArgs e)
        {
            var restService = new RestService();
            var expense = await restService.GetExpenseById(1);

            //print expense on screen.
        }
    }
}
