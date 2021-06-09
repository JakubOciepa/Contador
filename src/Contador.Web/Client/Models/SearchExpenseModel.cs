using System;

namespace Contador.Web.Client.Models
{
	/// <summary>
	/// Contains information used by searching in the db.
	/// </summary>
	public class SearchExpenseModel
	{
		/// <summary>
		/// This phrase will be searched in the name of the expense.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// This phrase will be searched in the name of the Expense category.
		/// </summary>
		public string CategoryName { get; set; } 

		/// <summary>
		/// This phrase will be searched in the user name.
		/// </summary>
		public string UserName { get; set; }

		/// <summary>
		/// The minimum date of the expense creation.
		/// </summary>
		public DateTime StartDate { get; set; }

		/// <summary>
		/// The maximum date of the expense creation.
		/// </summary>
		public DateTime EndDate { get; set; }
	}
}
