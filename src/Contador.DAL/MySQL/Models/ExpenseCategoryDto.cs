using System;

using Contador.Core.Models;

namespace Contador.DAL.Models
{
	/// <summary>
	/// The category of the expenses.
	/// </summary>
	public class ExpenseCategoryDto : ExpenseCategory
	{
		/// <summary>
		/// Date when the ExpenseCategory has been created.
		/// </summary>
		public DateTime CreatedDate { get; }

		/// <summary>
		/// Date when the ExpenseCategory has been edited last time.
		/// </summary>
		public DateTime ModifiedDate { get; set; }

		/// <summary>
		/// Creates instance of <see cref="ExpenseCategoryDto"/> class.
		/// </summary>
		public ExpenseCategoryDto() : base(string.Empty)
		{
		}

		/// <summary>
		/// Provides the same values but packet in the <see cref="ExpenseCategory"/> object.
		/// </summary>
		/// <returns>Instance of <see cref="ExpenseCategory"/>.</returns>
		public ExpenseCategory AsExpenseCategory()
		{
			return new ExpenseCategory(Name) { Id = this.Id };
		}

		/// <summary>
		/// Contains names of parameters required by expense category stored procedures.
		/// </summary>
		public static class ParameterName
		{
			/// <summary>
			/// Id parameter name.
			/// </summary>
			public static readonly string Id = "id_p";

			/// <summary>
			/// Name parameter name.
			/// </summary>
			public static readonly string Name = "name_p";
		}

		/// <summary>
		/// Contains names of expense category stored procedures.
		/// </summary>
		public static class ProcedureName
		{
			/// <summary>
			/// Name of the procedure which adds new expense category into the database.
			/// </summary>
			public static readonly string Add = "expenseCategory_Add";

			/// <summary>
			/// Name of the procedure which removes expense  category from the database.
			/// </summary>
			public static readonly string Delete = "expenseCategory_Delete";

			/// <summary>
			/// Name of the procedure which gets all available expense categories from the database.
			/// </summary>
			public static readonly string GetAll = "expenseCategory_GetAll";

			/// <summary>
			/// Name of the procedure which gets expense category by id from the database.
			/// </summary>
			public static readonly string GetById = "expenseCategory_GetById";

			/// <summary>
			/// Name of the procedure which updates expense category from the database.
			/// </summary>
			public static readonly string Update = "expenseCategory_Update";
		}
	}
}
