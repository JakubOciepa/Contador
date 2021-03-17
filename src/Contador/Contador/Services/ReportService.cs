
using Contador.Abstractions;
using Contador.Core.Common;

namespace Contador.Services
{
	public class ReportService : IReportService
	{
		private readonly IExpenseManager _expenseManager;
		private readonly IExpenseCategoryManager _expenseCategoryManager;

		public ReportService(IExpenseManager expenseManager, IExpenseCategoryManager expenseCategoryManager)
		{
			_expenseManager = expenseManager;
			_expenseCategoryManager = expenseCategoryManager;
		}

		public Report GetMonthlyShortReport(int month)
		{
			return null;
		}

		public Report GetYearlyShortReport(int year)
		{
			return null;
		}
	}
}
