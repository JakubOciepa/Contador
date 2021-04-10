using System.Collections.Generic;

namespace Contador.Core.Common
{
	/// <summary>
	/// Contains short info about expenses and categories.
	/// </summary>
	public class ReportShort
	{
		/// <summary>
		/// Total value of the expenses.
		/// </summary>
		public decimal ExpensesTotal { get; set; }

		/// <summary>
		/// Total value for each expense categories.
		/// </summary>
		public IDictionary<string, decimal> CategoriesTotals { get; set; }

		/// <summary>
		/// The percentage values of each expense category total values according to the total value.
		/// </summary>
		public IDictionary<string, int> CategoriesPercentages { get; set; }

		/// <summary>
		/// <see cref="ReportShort"/> with dummy/empty data.
		/// </summary>
		public static ReportShort Empty
			=> new ReportShort()
			{
				ExpensesTotal = 0,
				CategoriesTotals = new Dictionary<string, decimal>() { { "empty", 0m } },
				CategoriesPercentages = new Dictionary<string, int>() { { "empty", 0 } }
			};
	}
}
