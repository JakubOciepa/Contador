using System;

namespace Contador.DAL.MySQL.Models
{
	/// <summary>
	/// Reflects Budget structure from db.
	/// </summary>
	public class BudgetDto
	{
		/// <summary>
		/// Gets or sets Id of the budget.
		/// </summary>
		public int Id { get; set; }
		
		/// <summary>
		/// Gets or sets start date of the budget.
		/// </summary>
		public DateTime StartDate { get; set; }

		/// <summary>
		/// Gets or sets the end date of the budget.
		/// </summary>
		public DateTime EndDate { get; set; }
	}
}
