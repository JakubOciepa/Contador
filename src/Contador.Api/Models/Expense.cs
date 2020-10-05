namespace Contador.Api.Models
{
    /// <summary>
    /// Expense info.
    /// </summary>
    public class Expense
    {
        /// <summary>
        /// The name of the expense e.g. what has been bought.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The name of the <see cref="Category"/> that the expense belongs.
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// Id of the <see cref="User"/> which is the creator of this expense.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Value of the expense.
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// Specific description about the expense.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Constructor of the expese.
        /// </summary>
        public Expense(string name, decimal value, int userId, string categoryName)
        {
            Name = name;
            Value = value;
            UserId = userId;
            CategoryName = categoryName;
        }
    }
}
