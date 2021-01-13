namespace Contador.Mobile.ViewModels
{
	public class MainViewModel : ViewModelBase
	{
		public readonly ExpensesListPageViewModel _expensesListPageVM;

		public ExpensesListPageViewModel ExpensesListVM => _expensesListPageVM;

		public MainViewModel(ExpensesListPageViewModel listPageViewModel)
		{
			_expensesListPageVM = listPageViewModel;
		}
	}
}
