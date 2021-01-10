using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

using Contador.Core.Models;
using Contador.Core.Utils.Extensions;
using Contador.DAL.Abstractions;
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

		/// <summary>
		/// Creates instance of <see cref="ExpenseCategoryRepository"/> class.
		/// </summary>
		/// <param name="connection">DbContext.</param>
		public ExpenseCategoryRepository(IDbConnection connection)
		{
			_dbConnection = connection;
		}

		/// <inheritdoc/>
		public async Task<ExpenseCategory> AddCategoryAsync(ExpenseCategory expenseCategory)
		{
			var parameter = new DynamicParameters();
			parameter.Add(ExpenseCategoryDto.ParameterName.Name, expenseCategory.Name);

			return (await _dbConnection.QuerySingleAsync<ExpenseCategoryDto>(ExpenseCategoryDto.ProcedureName.Add,
				parameter, commandType: CommandType.StoredProcedure)
				.CAF()).AsExpenseCategory();
		}

		/// <inheritdoc/>
		public async Task<IList<ExpenseCategory>> GetCategoriesAsync()
		{
			return (await _dbConnection.QueryAsync<ExpenseCategoryDto>(ExpenseCategoryDto.ProcedureName.GetAll,
				commandType: CommandType.StoredProcedure).CAF()).Cast<ExpenseCategory>().ToList();
		}

		///<inheritdoc/>
		public async Task<ExpenseCategory> GetCategoryByIdAsync(int categoryId)
		{
			var parameter = new DynamicParameters();
			parameter.Add(ExpenseCategoryDto.ParameterName.Id, categoryId);

			return await _dbConnection.QueryFirstOrDefaultAsync<ExpenseCategoryDto>(ExpenseCategoryDto.ProcedureName.GetById,
				parameter, commandType: CommandType.StoredProcedure).CAF();
		}

		/// <inheritdoc/>
		public async Task<bool> RemoveCategoryAsync(int id)
		{
			var parameter = new DynamicParameters();
			parameter.Add(ExpenseCategoryDto.ParameterName.Id, id);
			await _dbConnection.ExecuteAsync(ExpenseCategoryDto.ProcedureName.Delete, parameter,
				commandType: CommandType.StoredProcedure).CAF();

			return !(await GetCategoryByIdAsync(id).CAF() is object);
		}

		/// <inheritdoc/>
		public async Task<ExpenseCategory> UpdateCategoryAsync(int id, ExpenseCategory expenseCategory)
		{
			var parameter = new DynamicParameters();
			parameter.Add(ExpenseCategoryDto.ParameterName.Id, id);
			parameter.Add(ExpenseCategoryDto.ParameterName.Name, expenseCategory.Name);

			return (await _dbConnection.QuerySingleAsync<ExpenseCategoryDto>(ExpenseCategoryDto.ProcedureName.Update,
				parameter, commandType: CommandType.StoredProcedure).CAF()).AsExpenseCategory();
		}
	}
}
