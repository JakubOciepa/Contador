using Contador.Core.Models;

namespace Contador.DAL.Models
{
    /// <summary>
    /// The category of the expenses.
    /// </summary>
    public class ExpenseCategoryDto : ExpenseCategory
    {
        /// <summary>
        /// Creates instance of <see cref="ExpenseCategoryDto"/> class.
        /// </summary>
        public ExpenseCategoryDto() : base(string.Empty)
        {
        }
    }
}
