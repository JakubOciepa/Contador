using System;
using System.Collections.Generic;

namespace Contador.Core.Models
{
	/// <summary>
	/// Budget model
	/// </summary>
	public class Budget
	{
		/// <summary>
		/// Gets of sets the id of the budget.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets start date of the budget.
		/// </summary>
		public DateTime StartDate { get; set; }

		/// <summary>
		/// Gets of sets end date of the budget.
		/// </summary>
		public DateTime EndDate { get; set; }

		/// <summary>
		/// Gets or sets the budget values.
		/// </summary>
		public Dictionary<string, decimal> Values { get; set; }
	}
}
