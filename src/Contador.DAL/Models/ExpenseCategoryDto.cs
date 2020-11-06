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
    }
}
