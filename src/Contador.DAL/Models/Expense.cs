using System;

namespace Contador.DAL.Models
{
    /// <summary>
    /// Expense info.
    /// </summary>
    public class Expense
    {
        /// <summary>
        /// Id of the expense.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the expense e.g. what has been bought.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The id of the <see cref="Category"/> that the expense belongs.
        /// </summary>
        public int CategoryId { get; set; }

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
        /// Date when the expense has been created.
        /// </summary>
        public DateTime CreatedDate { get; }

        /// <summary>
        /// Date when the expense has been edited last time.
        /// </summary>
        public DateTime LastEditDate { get; set; }

        /// <summary>
        /// Constructor of the expese.
        /// </summary>
        public Expense(string name, decimal value, int userId, int categoryId)
        {
            Name = name;
            Value = value;
            UserId = userId;
            CategoryId = categoryId;
            CreatedDate = DateTime.Now;
            LastEditDate = DateTime.Now;
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
