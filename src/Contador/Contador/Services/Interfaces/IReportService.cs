
using System.Threading.Tasks;

using Contador.Core.Common;

namespace Contador.Services.Interfaces
{
	/// <summary>
	/// Provides methods to get reports for expenses and categories.
	/// </summary>
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

		/// <summary>
		/// Generates <see cref="CategoryReport"/> for the category of provided id.
		/// </summary>
		/// <param name="categoryId">Category id for the report</param>
		/// <returns><see cref="CategoryReport"/> for the provided category.</returns>
		Task<Result<CategoryReport>> GetForCategoryAsync(int categoryId);
	}
}
