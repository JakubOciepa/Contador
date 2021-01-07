using SQLite;

namespace Contador.DAL.SQLite.Models
{
    /// <summary>
    /// The category of the expenses.
    /// </summary>
    public class ExpenseCategoryDto
    {
        /// <summary>
        /// The id of this expense category.
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// The name of the expense category.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Creates instance of the <see cref="ExpenseCategory"/> class.
        /// </summary>
        /// <param name="name"></param>
        public ExpenseCategoryDto(string name)
        {
            Name = name;
        }
    }
}
