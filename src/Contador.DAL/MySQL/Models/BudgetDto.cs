using System;
using System.Collections.Generic;

using Contador.Core.Models;

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

		public Budget AsBudget()
		{
			return new Budget
			{
				Id = Id,
				StartDate = StartDate,
				EndDate = EndDate,
				Values = new Dictionary<string, CategoryBudget>(),
			};
		}

		/// <summary>
		/// Procedures parameters for the Budget procedures.
		/// </summary>
		public class ParameterName
		{
			/// <summary>
			/// Id parameter name.
			/// </summary>
			public const string Id = "id_p";

			/// <summary>
			/// Start date parameter name.
			/// </summary>
			public const string StartDate = "startDate_p";

			/// <summary>
			/// End date parameter name.
			/// </summary>
			public const string EndDate = "endDate_p";
		}

		/// <summary>
		/// Procedures names for the Budget.
		/// </summary>
		public class ProcedureName
		{
			/// <summary>
			/// Get all budgets procedure name.
			/// </summary>
			public const string GetAll = "budget_GetAll";

			/// <summary>
			/// Get budget by id procedure name.
			/// </summary>
			public const string GetById = "budget_GetById";

			/// <summary>
			/// Add budget procedure name.
			/// </summary>
			public const string Add = "budget_Add";

			/// <summary>
			/// Get budget by the start date procedure name.
			/// </summary>
			public const string GetByStartDate = "budget_GetByStartDate";

			/// <summary>
			/// Update budget procedure name.
			/// </summary>
			public const string Update = "budget_Update";

			/// <summary>
			/// Remove budget by id procedure name.
			/// </summary>
			public const string Delete = "budget_Delete";
		}
	}
}
