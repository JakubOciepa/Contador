
using Contador.Core.Common;

namespace Contador.Abstractions
{
	public interface IReportService
	{
		Report GetMonthlyShortReport(int month);

		Report GetYearlyShortReport(int year);
	}
}
