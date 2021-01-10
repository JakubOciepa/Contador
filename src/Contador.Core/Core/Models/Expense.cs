﻿using System;

namespace Contador.Core.Models
{
	/// <summary>
	/// Expense info.
	/// </summary>
	public class Expense
	{
		/// <summary>
		/// Id of this expense.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// The name of the expense e.g. what has been bought.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// The <see cref="Category"/> that the expense belongs.
		/// </summary>
		public ExpenseCategory Category { get; set; }

		/// <summary>
		/// The <see cref="Models.User"/> which is the creator of this expense.
		/// </summary>
		public User User { get; set; }

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
		/// <see cref="DateTime"/> when the expense has been created.
		/// </summary>
		public DateTime CreateDate { get; set; }

		/// <summary>
		/// Creates instance of <see cref="Expense"/> class.
		/// <param name="name">Name of the expense.</param>
		/// <param name="value">Value of the expense.</param>
		/// <param name="user">The owner of the expense.</param>
		/// <param name="category">Category of the expense.</param>
		public Expense(string name, decimal value, User user, ExpenseCategory category)
		{
			Name = name;
			Value = value;
			User = user;
			Category = category;
		}
	}
}
