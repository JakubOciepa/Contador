using Contador.DAL.Abstractions;

namespace Contador.Mobile.ViewModels
{
    public class ExpensesListPageViewModel : ViewModelBase
    {
        public ExpensesListPageViewModel(IExpenseRepository expenserepo)
        {
            var repo = expenserepo;
        }

    }
}
