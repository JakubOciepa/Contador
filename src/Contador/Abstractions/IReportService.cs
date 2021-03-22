
using Contador.Core.Common;

namespace Contador.Abstractions
{
	public interface IReportService
	{
		/// <summary>
		/// Generates <see cref="ReportShort"/> for the provided month.
		/// </summary>
		/// <param name="month">Month for the report.</param>
		/// <returns><see cref="ReportShort"/> for the provided month.</returns>
		ReportShort GetMonthlyShortReport(int month);

		/// <summary>
		/// Generates <see cref="ReportShort"/> for the provided year.
		/// </summary>
		/// <param name="year">Year for the report.</param>
		/// <returns><see cref="ReportShort"/>for the provided year.</returns>
		ReportShort GetYearlyShortReport(int year);
	}
}
