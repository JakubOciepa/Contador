namespace Contador.DAL.MySQL.Models
{
	/// <summary>
	/// Reflects the CategoryBudget structure from db.
	/// </summary>
	public class CategoryBudgetDto
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

		/// <summary>
		/// Procedures parameters of the category budget procedures.
		/// </summary>
		public static class ParameterName
		{
			/// <summary>
			/// Budget id parameter name.
			/// </summary>
			public const string BudgetId = "budgetId_p";
			
			/// <summary>
			/// Category Id parameter name.
			/// </summary>
			public const string CategoryId = "cateogryId_p";
			
			/// <summary>
			/// Value parameter name.
			/// </summary>
			public const string Value = "value_p";
		}

		/// <summary>
		/// Procedures names of the category budget.
		/// </summary>
		public static class ProcedureName
		{
			/// <summary>
			/// Adding category budget procedure name.
			/// </summary>
			public const string Add = "categoryBudget_Add";

			public const string GetByBudgetId = "categoryBudget_GetByBudgetId";
		}
	}
}
