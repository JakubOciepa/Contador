using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Contador.Core.Models;

using SQLite;

namespace Contador.DAL.SQLite.Models
{
    public class ExpenseDto : Expense
    {
        [PrimaryKey, AutoIncrement]
        /// <summary>
        /// Id of this expense.
        /// </summary>
        public new int Id { get; set; }

        /// <summary>
        /// The id of the <see cref="Category"/> that the expense belongs.
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Id of the <see cref="UserDto"/> which is the creator of this expense.
        /// </summary>
        public int UserId { get; set; }

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

        /// <summary>
        /// Provides the same values but packet in the <see cref="Expense"/> object.
        /// </summary>
        /// <returns>Instance of <see cref="Expense"/>.</returns>
        public Expense AsExpense()
        {
            return new Expense(Name, Value, User, Category)
            {
                Id = this.Id,
                Description = Description,
                ImagePath = ImagePath
            };
        }
    }
}
