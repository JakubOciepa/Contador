using System;

namespace Contador.Common
{
	public static class DateTimeHelper
	{
		public static string GetMonthYearDateString(DateTime date) => $"{date:MMMM yyyy}";
	}
}
