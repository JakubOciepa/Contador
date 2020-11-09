using System.Net.Http;

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
            var client = new HttpClient();

            var resutl = await client.GetAsync(@"http://192.168.1.31:5000/api/expense");

        }
    }
}
