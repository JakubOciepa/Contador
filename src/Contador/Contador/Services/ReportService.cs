
using Contador.Abstractions;
using Contador.Core.Common;

namespace Contador.Services
{
	/// <summary>
	/// Provides methods for the report creation.
	/// </summary>
	public class ReportService : IReportService
	{
		private readonly IExpenseManager _expenseManager;
		private readonly IExpenseCategoryManager _expenseCategoryManager;
		private readonly ILog _logger;

		/// <summary>
		/// Creates an instance of the <see cref="ReportService"/> class.
		/// </summary>
		/// <param name="expenseManager"></param>
		/// <param name="expenseCategoryManager"></param>
		/// <param name="logger"></param>
		public ReportService(IExpenseManager expenseManager, IExpenseCategoryManager expenseCategoryManager, ILog logger)
		{
			_expenseManager = expenseManager;
			_expenseCategoryManager = expenseCategoryManager;
			_logger = logger;
		}

		/// <summary>
		/// Generates <see cref="ReportShort"/> for the provided month.
		/// </summary>
		/// <param name="month">Month for the report.</param>
		/// <returns><see cref="ReportShort"/> for the provided month.</returns>
		public ReportShort GetMonthlyShortReport(int month)
		{
			return null;
		}

		/// <summary>
		/// Generates <see cref="ReportShort"/> for the provided year.
		/// </summary>
		/// <param name="year">Year for the report.</param>
		/// <returns><see cref="ReportShort"/>for the provided year.</returns>
		public ReportShort GetYearlyShortReport(int year)
		{
			return null;
		}
	}
}
