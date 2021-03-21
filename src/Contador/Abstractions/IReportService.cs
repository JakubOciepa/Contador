
using Contador.Core.Common;

namespace Contador.Abstractions
{
	public interface IReportService
	{
		ReportShort GetMonthlyShortReport(int month);

		ReportShort GetYearlyShortReport(int year);
	}
}
