
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

		public ReportShort GetMonthlyShortReport(int month)
		{
			return null;
		}

		public ReportShort GetYearlyShortReport(int year)
		{
			return null;
		}
	}
}
