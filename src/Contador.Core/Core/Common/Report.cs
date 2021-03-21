using System.Collections.Generic;

namespace Contador.Core.Common
{
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

	}
}
