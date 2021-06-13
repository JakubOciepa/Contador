using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

using Contador.Core.Models;
using Contador.Core.Utils.Extensions;
using Contador.DAL.Abstractions;
using Contador.DAL.MySQL.Models;

using Dapper;

namespace Contador.DAL.MySQL.Repositories
{
	/// <summary>
	/// Manages expense categories in db.
	/// </summary>
	public class ExpenseCategoryRepository : IExpenseCategoryRepository
	{
		private readonly IDbConnection _dbConnection;

		/// <summary>
		/// Creates instance of <see cref="ExpenseCategoryRepository"/> class.
		/// </summary>
		/// <param name="connection">DbContext.</param>
		public ExpenseCategoryRepository(IDbConnection connection)
		{
			_dbConnection = connection;
		}

		/// <summary>
		/// Adds expense category to storage.
		/// </summary>
		/// <param name="expenseCategory">Expense category to add.</param>
		/// <returns>Added expense category</returns>
		public async Task<ExpenseCategory> AddAsync(ExpenseCategory expenseCategory)
		{
			var parameter = new DynamicParameters();
			parameter.Add(ExpenseCategoryDto.ParameterName.Name, expenseCategory.Name);

			return (await _dbConnection.QuerySingleAsync<ExpenseCategoryDto>(ExpenseCategoryDto.ProcedureName.Add,
				parameter, commandType: CommandType.StoredProcedure)
				.CAF()).AsExpenseCategory();
		}

		/// <summary>
		/// Gets all available expense categories.
		/// </summary>
		/// <returns><see cref="IList{ExpenseCategory}"/> of all available categories.</returns>
		public async Task<IList<ExpenseCategory>> GetAllAsync()
		{
			return (await _dbConnection.QueryAsync<ExpenseCategoryDto>(ExpenseCategoryDto.ProcedureName.GetAll,
				commandType: CommandType.StoredProcedure).CAF()).Cast<ExpenseCategory>().ToList();
		}

		/// <summary>
		/// Gets category by its id.
		/// </summary>
		/// <param name="categoryId">Id of requested <see cref="ExpenseCategory"/>.</param>
		/// <returns><see cref="ExpenseCategory"/> of requested Id.</returns>
		public async Task<ExpenseCategory> GetByIdAsync(int categoryId)
		{
			var parameter = new DynamicParameters();
			parameter.Add(ExpenseCategoryDto.ParameterName.Id, categoryId);

			return await _dbConnection.QueryFirstOrDefaultAsync<ExpenseCategoryDto>(ExpenseCategoryDto.ProcedureName.GetById,
				parameter, commandType: CommandType.StoredProcedure).CAF();
		}

		/// <summary>
		/// Removes expense category from storage.
		/// </summary>
		/// <param name="id">Id of expense category to remove.</param>
		/// <returns>True if removed, false otherwise.</returns>
		public async Task<bool> RemoveAsync(int id)
		{
			var parameter = new DynamicParameters();
			parameter.Add(ExpenseCategoryDto.ParameterName.Id, id);
			_ = await _dbConnection.ExecuteAsync(ExpenseCategoryDto.ProcedureName.Delete, parameter,
				commandType: CommandType.StoredProcedure).CAF();

			return !(await GetByIdAsync(id).CAF() is object);
		}

		/// <summary>
		/// Updates expense category of provided id.
		/// </summary>
		/// <param name="id">Id of expense category to update.</param>
		/// <param name="expenseCategory">Category info.</param>
		/// <returns>Updated category.</returns>
		public async Task<ExpenseCategory> UpdateAsync(int id, ExpenseCategory expenseCategory)
		{
			var parameter = new DynamicParameters();
			parameter.Add(ExpenseCategoryDto.ParameterName.Id, id);
			parameter.Add(ExpenseCategoryDto.ParameterName.Name, expenseCategory.Name);

			return (await _dbConnection.QuerySingleAsync<ExpenseCategoryDto>(ExpenseCategoryDto.ProcedureName.Update,
				parameter, commandType: CommandType.StoredProcedure).CAF()).AsExpenseCategory();
		}
	}
}
