using Contador.Core.Models;

namespace Contador.Mobile.ViewModels
{
    public class ExpenseControlViewModel : ViewModelBase
    {
        public Expense Expense { get; }

        public ExpenseControlViewModel(Expense expense)
        {
            Expense = expense;
        }
    }
}
