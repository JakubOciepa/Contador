using System;
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
	/// Manages budgets in the db.
	/// </summary>
	public class BudgetRepository : IBudgetRepository
	{
		private readonly IDbConnection _dbConnection;

		/// <summary>
		/// Creates an instance of the <see cref="BudgetRepository"/> class.
		/// </summary>
		/// <param name="dbConnection">Database context.</param>
		public BudgetRepository(IDbConnection dbConnection)
		{
			_dbConnection = dbConnection;
		}

		/// <summary>
		/// Adds the budget to the db.
		/// </summary>
		/// <param name="budgetToAdd">Budget to add.</param>
		/// <returns>Adds budget to the db.</returns>
		public async Task<Budget> AddAsync(Budget budgetToAdd)
		{
			var param = new DynamicParameters();
			param.Add(BudgetDto.ParameterName.StartDate, budgetToAdd.StartDate);
			param.Add(BudgetDto.ParameterName.EndDate, budgetToAdd.EndDate);

			return (await _dbConnection.QuerySingleAsync<BudgetDto>(BudgetDto.ProcedureName.Add,
				param, commandType: CommandType.StoredProcedure)
				.CAF()).AsBudget();
		}


		public async Task<Budget> GetByStartDateAsync(DateTime startDate)
		{
			var param = new DynamicParameters();
			param.Add(BudgetDto.ParameterName.StartDate, startDate);

			return (await _dbConnection.QueryAsync<BudgetDto>(BudgetDto.ProcedureName.GetByStartDate,
				param, commandType: CommandType.StoredProcedure).CAF()).FirstOrDefault()?.AsBudget() ?? null;
		}

		/// <summary>
		/// Gets the budget by the provided id.
		/// </summary>
		/// <param name="id">Id of the budget.</param>
		/// <returns>Budget or null if doesn't exists.</returns>
		public async Task<Budget> GetByIdAsync(int id)
		{
			var param = new DynamicParameters();
			param.Add(BudgetDto.ParameterName.Id, id);

			var budget = (await _dbConnection.QueryAsync<BudgetDto>(BudgetDto.ProcedureName.GetById,
				param, commandType: CommandType.StoredProcedure)
				.CAF()).FirstOrDefault()?.AsBudget() ?? null;

			if (budget is object)
			{
				await _dbConnection
					.QueryAsync<CategoryBudgetDto, ExpenseCategoryDto, CategoryBudgetDto>(CategoryBudgetDto.ProcedureName.GetByBudgetId,
					(bc, category) =>
					{
						budget.Values.Add(category.Name, bc.Value);

						return bc;
					},
					param,
					commandType: CommandType.StoredProcedure)
					.CAF();
			}

			return budget;
		}
	}
}
