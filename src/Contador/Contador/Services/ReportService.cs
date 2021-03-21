
using Contador.Abstractions;
using Contador.Core.Common;

namespace Contador.Services
{
	public class ReportService : IReportService
	{
		private readonly IExpenseManager _expenseManager;
		private readonly IExpenseCategoryManager _expenseCategoryManager;
		private readonly ILog _logger;

		public ReportService(IExpenseManager expenseManager, IExpenseCategoryManager expenseCategoryManager, ILog logger)
		{
			_expenseManager = expenseManager;
			_expenseCategoryManager = expenseCategoryManager;
			_logger = logger;
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
