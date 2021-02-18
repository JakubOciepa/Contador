namespace Contador.Core.Models
{
	/// <summary>
	/// The category of the expenses.
	/// </summary>
	public class ExpenseCategory
	{
		/// <summary>
		/// The id of this expense category.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// The name of the expense category.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Creates instance of the <see cref="ExpenseCategory"/> class.
		/// </summary>
		/// <param name="name"></param>
		public ExpenseCategory(string name)
		{
			Name = name;
		}
	}
}
