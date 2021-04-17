using System;

using SQLite;

namespace Contador.DAL.SQLite.Models
{
	public class ExpenseDto
	{
		/// <summary>
		/// Id of this expense.
		/// </summary>
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		/// <summary>
		/// The name of the expense e.g. what has been bought.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Value of the expense.
		/// </summary>
		public decimal Value { get; set; }

		/// <summary>
		/// Specific description about the expense.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Path to the image of the expense receipt.
		/// </summary>
		public string ImagePath { get; set; }

		/// <summary>
		/// The id of the <see cref="Category"/> that the expense belongs.
		/// </summary>
		public int CategoryId { get; set; }

		/// <summary>
		/// Id of the <see cref="UserDto"/> which is the creator of this expense.
		/// </summary>
		public string UserId { get; set; }

		/// <summary>
		/// <see cref="DateTime"/> when the expense has been created.
		/// </summary>
		public DateTime CreateDate { get; set; }

		/// <summary>
		/// Date when the expense has been edited last time.
		/// </summary>
		public DateTime ModifiedDate { get; set; }

		/// <summary>
		/// Initializes instance of the <see cref="ExpenseDto"/> class.
		/// </summary>
		public ExpenseDto()
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
	}
}
