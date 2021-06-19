using System;

namespace Contador.Common
{
	public static class DateTimeHelper
	{
		public static string GetMonthYearDateString(DateTime date) => date.Month switch
		{
			1 => $"Styczeń {date.Year}",
			2 => $"Luty {date.Year}",
			3 => $"Marzec {date.Year}",
			4 => $"Kwiecień {date.Year}",
			5 => $"Maj {date.Year}",
			6 => $"Czerwiec {date.Year}",
			7 => $"Lipiec {date.Year}",
			8 => $"Sierpień {date.Year}",
			9 => $"Wrzesień {date.Year}",
			10 => $"Październik {date.Year}",
			11 => $"Listopad {date.Year}",
			12 => $"Grudzień {date.Year}",
			_ => date.Year.ToString(),
		};
	}
}
