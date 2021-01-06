
using Contador.DAL.Abstractions;
using Contador.DAL.SQLite.Repositories;

using TinyIoC;

using Xamarin.Forms;

namespace Contador.Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            RegisterServices(TinyIoCContainer.Current);

            MainPage = new Pages.MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        private void RegisterServices(TinyIoCContainer container)
        {
            var expenseRepo = new ExpenseRepository(string.Empty);

            container.Register<IExpenseRepository>((_,__) => new ExpenseRepository(string.Empty));

            container.BuildUp(this);
        }
    }
}
