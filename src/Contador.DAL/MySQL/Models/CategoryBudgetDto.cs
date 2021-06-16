using Contador.Core.Models;

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

		public CategoryBudget AsCategoryBudget()
		{
			return new CategoryBudget
			{
				Id = Id,
				BudgetId = BudgetId,
				CategoryId = CategoryId,
				Value = Value
			};
		}

		/// <summary>
		/// Procedures parameters of the category budget procedures.
		/// </summary>
		public static class ParameterName
		{
			/// <summary>
			/// Id parameter name.
			/// </summary>
			public const string Id = "id_p";

			/// <summary>
			/// Budget id parameter name.
			/// </summary>
			public const string BudgetId = "budgetId_p";
			
			/// <summary>
			/// Category Id parameter name.
			/// </summary>
			public const string CategoryId = "categoryId_p";
			
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

			/// <summary>
			/// Gets category budget by the id procedure name.
			/// </summary>
			public const string GetById = "categoryBudget_GetById";

			/// <summary>
			/// Getting category budget by budget if procedure name.
			/// </summary>
			public const string GetByBudgetId = "categoryBudget_GetByBudgetId";

			/// <summary>
			/// Getting category budget by the category and budget id.
			/// </summary>
			public const string GetByCategoryAndBudgetId = "categoryBudget_GetByCategoryAndBudgetId";
		}
	}
}
