using System.Collections.Generic;

using Contador.Core.Models;

namespace Contador.Core.Common
{
	/// <summary>
	/// Contains the report information of the expense category.
	/// </summary>
	public class CategoryReport
	{
		/// <summary>
		/// Average spent money by month in this category.
		/// </summary>
		public decimal AverageMonthly { get; set; }

		/// <summary>
		/// Average spent money by year in this category.
		/// </summary>
		public decimal AverageYearly { get; set; }

		/// <summary>
		/// Money spent this month in this category.
		/// </summary>
		public decimal MonthSpent { get; set; }

		/// <summary>
		/// Money spent this year in this category.
		/// </summary>
		public decimal YearSpent { get; set; }

		/// <summary>
		/// All expenses in this category.
		/// </summary>
		public IEnumerable<Expense> Expenses { get; set; }
	}
}
