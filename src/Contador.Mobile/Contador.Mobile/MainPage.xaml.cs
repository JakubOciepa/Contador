using Contador.Services;

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
        }
    }
}
