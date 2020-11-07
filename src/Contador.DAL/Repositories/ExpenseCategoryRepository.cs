using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

using Contador.Core.Models;
using Contador.Core.Utils.Extensions;
using Contador.DAL.Models;

using Dapper;

namespace Contador.DAL.Repositories
{
    /// <summary>
    /// Manages expense categories in db.
    /// </summary>
    public class ExpenseCategoryRepository : IExpenseCategoryRepository
    {
        private readonly IDbConnection _dbConnection;

        private static readonly List<ExpenseCategory> _stub = new List<ExpenseCategory>
        {
            new ExpenseCategory("Słodycze")
            {
                Id = 0,
            },
        };

        /// <summary>
        /// Creates instance of <see cref="ExpenseCategoryRepository"/> class.
        /// </summary>
        /// <param name="connection">DbContext.</param>
        public ExpenseCategoryRepository(IDbConnection connection)
        {
            _dbConnection = connection;
        }

        /// <inheritdoc/>
        public async Task<ExpenseCategory> Add(ExpenseCategory expenseCategory)
        {
            var parameter = new DynamicParameters();
            parameter.Add(ExpenseCategoryDto.ParameterName.Name, expenseCategory.Name);

            return await _dbConnection.QuerySingleAsync<ExpenseCategory>(ExpenseCategoryDto.ProcedureName.Add,
                parameter, commandType: CommandType.StoredProcedure)
                .CAF();
        }

        /// <inheritdoc/>
        public async Task<IList<ExpenseCategory>> GetCategories()
        {
            return _stub;
        }

        ///<inheritdoc/>
        public async Task<ExpenseCategory> GetCategoryById(int categoryId)
        {
            return _stub.Find(c => c.Id == categoryId);
        }

        /// <inheritdoc/>
        public async Task<bool> Remove(int id)
        {
            var categoryToRemove = _stub.Find(e => e.Id == id);

            if (categoryToRemove == default)
            {
                return true;
            }

            _stub.Remove(categoryToRemove);

            return !_stub.Contains(categoryToRemove);
        }

        /// <inheritdoc/>
        public async Task<ExpenseCategory> Update(int id, ExpenseCategory expenseCategory)
        {
            var categoryToUpdate = _stub.Find(e => e.Id == id);

            if (categoryToUpdate == default)
            {
                return default;
            }

            categoryToUpdate.Name = expenseCategory.Name;

            return categoryToUpdate;
        }
    }
}
