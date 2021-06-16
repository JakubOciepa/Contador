using System;
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

		/// <inheritdoc/>
		public async Task<Budget> AddBudgetAsync(Budget budgetToAdd)
		{
			var param = new DynamicParameters();
			param.Add(BudgetDto.ParameterName.StartDate, budgetToAdd.StartDate);
			param.Add(BudgetDto.ParameterName.EndDate, budgetToAdd.EndDate);

			return (await _dbConnection.QuerySingleAsync<BudgetDto>(BudgetDto.ProcedureName.Add,
				param, commandType: CommandType.StoredProcedure)
				.CAF()).AsBudget();
		}

		/// <inheritdoc/>
		public async IAsyncEnumerable<Budget> GetAllBudgetsAsync()
		{
			var budgets = (await _dbConnection.QueryAsync<Budget>(BudgetDto.ProcedureName.GetAll,
				commandType: CommandType.StoredProcedure).CAF()).ToList();

			if (budgets is object)
			{
				foreach (var budget in budgets)
				{
					yield return new Budget()
					{
						Id = budget.Id,
						StartDate = budget.StartDate,
						EndDate = budget.EndDate,
						Values = await GetValuesForBudget(budget.Id)
					};
				}
			}
		}

		/// <inheritdoc/>
		public async Task<Budget> GetBudgetByStartDateAsync(DateTime startDate)
		{
			var param = new DynamicParameters();
			param.Add(BudgetDto.ParameterName.StartDate, startDate);

			return (await _dbConnection.QueryAsync<BudgetDto>(BudgetDto.ProcedureName.GetByStartDate,
				param, commandType: CommandType.StoredProcedure).CAF()).FirstOrDefault()?.AsBudget() ?? null;
		}

		/// <inheritdoc/>
		public async Task<Budget> GetBudgetByIdAsync(int id)
		{
			var param = new DynamicParameters();
			param.Add(BudgetDto.ParameterName.Id, id);

			var budget = (await _dbConnection.QueryAsync<BudgetDto>(BudgetDto.ProcedureName.GetById,
				param, commandType: CommandType.StoredProcedure)
				.CAF()).FirstOrDefault()?.AsBudget() ?? null;

			if (budget is object)
			{
				budget.Values = await GetValuesForBudget(budget.Id);
			}

			return budget;
		}

		/// <inheritdoc/>
		public async Task<CategoryBudget> GetCategoryBudgetByCategoryAndBudgetIdAsync(int budgetId, int categoryId)
		{
			var param = new DynamicParameters();
			param.Add(CategoryBudgetDto.ParameterName.BudgetId, budgetId);
			param.Add(CategoryBudgetDto.ParameterName.CategoryId, categoryId);

			return (await _dbConnection.QueryAsync<CategoryBudgetDto>(CategoryBudgetDto.ProcedureName.GetByCategoryAndBudgetId,
				param, commandType: CommandType.StoredProcedure).CAF()).FirstOrDefault()?.AsCategoryBudget() ?? null;
		}

		/// <inheritdoc/>
		public async Task<CategoryBudget> AddCategoryBudgetAsync(CategoryBudget categoryBudget)
		{
			var param = new DynamicParameters();
			param.Add(CategoryBudgetDto.ParameterName.BudgetId, categoryBudget.BudgetId);
			param.Add(CategoryBudgetDto.ParameterName.CategoryId, categoryBudget.CategoryId);
			param.Add(CategoryBudgetDto.ParameterName.Value, categoryBudget.Value);

			return (await _dbConnection.QueryAsync<CategoryBudgetDto>(CategoryBudgetDto.ProcedureName.Add,
				param, commandType: CommandType.StoredProcedure).CAF()).FirstOrDefault()?.AsCategoryBudget() ?? null;
		}

		/// <inheritdoc/>
		public async Task<CategoryBudget> GetCategoryBudgetByIdAsync(int id)
		{
			var param = new DynamicParameters();
			param.Add(CategoryBudgetDto.ParameterName.Id, id);

			return (await _dbConnection.QueryAsync<CategoryBudgetDto>(CategoryBudgetDto.ProcedureName.GetById,
				param, commandType: CommandType.StoredProcedure)
				.CAF()).FirstOrDefault()?.AsCategoryBudget() ?? null;
		}

		private async Task<Dictionary<string, decimal>> GetValuesForBudget(int id)
		{
			var param = new DynamicParameters();
			param.Add(BudgetDto.ParameterName.Id, id);

			var categoryBudgets = new Dictionary<string, decimal>();
			await _dbConnection
					.QueryAsync<CategoryBudgetDto, ExpenseCategoryDto, CategoryBudgetDto>(CategoryBudgetDto.ProcedureName.GetByBudgetId,
					(bc, category) =>
					{
						categoryBudgets.Add(category.Name, bc.Value);

						return bc;
					},
					param,
					commandType: CommandType.StoredProcedure)
					.CAF();

			return categoryBudgets;
		}
	}
}
