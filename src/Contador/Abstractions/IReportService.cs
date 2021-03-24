
using System.Threading.Tasks;

using Contador.Core.Common;

namespace Contador.Abstractions
{
	public interface IReportService
	{
		/// <summary>
		/// Generates <see cref="ReportShort"/> for the provided month.
		/// </summary>
		/// <param name="month">Month for the report.</param>
		/// <param name="year">Year for the report.</param>
		/// <returns><see cref="ReportShort"/> for the provided month.</returns>
		Task<Result<ReportShort>> GetMonthlyShortReportAsync(int month, int year);

		/// <summary>
		/// Generates <see cref="ReportShort"/> for the provided year.
		/// </summary>
		/// <param name="year">Year for the report.</param>
		/// <returns><see cref="ReportShort"/>for the provided year.</returns>
		Task<Result<ReportShort>> GetYearlyShortReportAsync(int year);
	}
}
