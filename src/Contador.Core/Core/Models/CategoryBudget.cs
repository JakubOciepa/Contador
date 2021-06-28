namespace Contador.Core.Models
{
	/// <summary>
	/// Contains info about Category budget.
	/// </summary>
	public class CategoryBudget
	{
		/// <summary>
		/// Gets or sets the Id of the category budget.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the Id of the main budget.
		/// </summary>
		public int BudgetId { get; set; }

		/// <summary>
		/// Gets or sets the category id of the budget.
		/// </summary>
		public int CategoryId { get; set; }

		/// <summary>
		/// Gets or sets the value of the budget.
		/// </summary>
		public decimal Value { get; set; }
	}
}
