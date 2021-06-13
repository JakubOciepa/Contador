using System;

using Contador.Core.Models;

namespace Contador.DAL.MySQL.Models
{
	/// <summary>
	/// Expense info.
	/// </summary>
	public class ExpenseDto : Expense
	{
		/// <summary>
		/// The id of the <see cref="Category"/> that the expense belongs.
		/// </summary>
		public int CategoryId { get; set; }

		/// <summary>
		/// Id of the <see cref="UserDto"/> which is the creator of this expense.
		/// </summary>
		public string UserId { get; set; }

		/// <summary>
		/// Date when the expense has been edited last time.
		/// </summary>
		public DateTime ModifiedDate { get; set; }

		/// <summary>
		/// Initializes instance of the <see cref="ExpenseDto"/> class.
		/// </summary>
		public ExpenseDto() : base()
		{
		}

		/// <summary>
		/// Creates string with most important info about Expense.
		/// </summary>
		/// <returns>String with this info.</returns>
		public override string ToString()
		{
			return $"Expense: Id = {Id}, Name = {Name}, Value = {Value}";
		}

		/// <summary>
		/// Provides the same values but packet in the <see cref="Expense"/> object.
		/// </summary>
		/// <returns>Instance of <see cref="Expense"/>.</returns>
		public Expense AsExpense()
		{
			return new Expense(Name, Value, User, Category)
			{
				Id = this.Id,
				CreateDate = CreateDate,
				Description = Description,
				ImagePath = ImagePath
			};
		}

		/// <summary>
		/// Contains names of parameters required by expense stored procedures.
		/// </summary>
		public static class ParameterName
		{
			/// <summary>
			/// CategoryId parameter name.
			/// </summary>
			public const string CategoryId = "categoryId_p";

			/// <summary>
			/// CategoryName parameter name.
			/// </summary>
			public const string CategoryName = "categoryName_p";

			/// <summary>
			/// Description parameter name.
			/// </summary>
			public const string Description = "description_p";

			/// <summary>
			/// Id parameter name.
			/// </summary>
			public const string Id = "id_p";

			/// <summary>
			/// ImagePath parameter name.
			/// </summary>
			public const string ImagePath = "imagePath_p";

			/// <summary>
			/// Name parameter name.
			/// </summary>
			public const string Name = "name_p";

			/// <summary>
			/// UserId parameter name.
			/// </summary>
			public const string UserId = "userId_p";

			/// <summary>
			/// UserName parameter name.
			/// </summary>
			public const string UserName = "userName_p";

			/// <summary>
			/// Value parameter name.
			/// </summary>
			public const string Value = "value_p";

			/// <summary>
			/// Created date parameter name.
			/// </summary>
			public const string CreateDate = "createDate_p";

			/// <summary>
			/// Month number parameter name.
			/// </summary>
			public const string MonthNum = "month_p";

			/// <summary>
			/// Year parameter name.
			/// </summary>
			public const string Year = "year_p";

			/// <summary>
			/// Count parameter name.
			/// </summary>
			public const string Count = "count_p";
		}

		/// <summary>
		/// Contains names of expense stored procedures.
		/// </summary>
		public static class ProcedureName
		{
			/// <summary>
			/// Name of the procedure which adds new expense into the database.
			/// </summary>
			public const string Add = "expense_Add";

			/// <summary>
			/// Name of the procedure which removes expense from the database.
			/// </summary>
			public const string Delete = "expense_Delete";

			/// <summary>
			/// Name of the procedure which gets all available expenses from the database.
			/// </summary>
			public const string GetAll = "expense_GetAll";

			/// <summary>
			/// Name of the procedure which gets expense by id from the database.
			/// </summary>
			public const string GetById = "expense_GetById";

			/// <summary>
			/// Name of the procedure which gets all expenses for the provided month.
			/// </summary>
			public const string GetByMonth = "expense_GetByMonth";

			/// <summary>
			/// Name of the procedure which gets all expenses for the provided month.
			/// </summary>
			public const string GetByYear = "expense_GetByYear";

			/// <summary>
			/// Name of the procedure which gets max provided amount of latest expenses.
			/// </summary>
			public const string GetLatest = "expense_GetLatest";

			/// <summary>
			/// Name of the procedure which gets filtered expenses.
			/// </summary>
			public const string GetFiltered = "expense_GetFiltered";

			/// <summary>
			/// Name of the procedure which gets expenses by the category id.
			/// </summary>
			public const string GetByCategory = "expense_GetByCategory";

			/// <summary>
			/// Name of the procedure which updates expense from the database.
			/// </summary>
			public const string Update = "expense_Update";
		}
	}
}
