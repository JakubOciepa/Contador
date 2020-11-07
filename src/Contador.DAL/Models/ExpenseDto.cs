using System;

using Contador.Core.Models;

namespace Contador.DAL.Models
{
    /// <summary>
    /// Expense info.
    /// </summary>
    public class ExpenseDto : Expense
    {
        public static class ParameterName
        {
            public static readonly string Id = "ip_p";
            public static readonly string Name = "name_p";
            public static readonly string Value = "value_p";
            public static readonly string Description = "description_p";
            public static readonly string CategoryId = "categoryId_p";
            public static readonly string UserId = "userId_p";
            public static readonly string ImagePath = "image_path_p";
        }

        /// <summary>
        /// The id of the <see cref="Category"/> that the expense belongs.
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Id of the <see cref="UserDto"/> which is the creator of this expense.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Date when the expense has been created.
        /// </summary>
        public DateTime CreatedDate { get; }

        /// <summary>
        /// Date when the expense has been edited last time.
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Initializes instance of the <see cref="ExpenseDto"/> class.
        /// </summary>
        public ExpenseDto() : base(string.Empty, 0, null, null)
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
