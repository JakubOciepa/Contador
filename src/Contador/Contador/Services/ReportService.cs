using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Contador.Abstractions;
using Contador.Core.Common;
using Contador.Core.Models;
using Contador.Core.Utils.Extensions;

namespace Contador.Services
{
	/// <summary>
	/// Provides methods for the report creation.
	/// </summary>
	public class ReportService : IReportService
	{
		private readonly IExpenseManager _expenseManager;
		private readonly IExpenseCategoryManager _expenseCategoryManager;
		private readonly ILog _logger;

		/// <summary>
		/// Creates an instance of the <see cref="ReportService"/> class.
		/// </summary>
		/// <param name="expenseManager"></param>
		/// <param name="expenseCategoryManager"></param>
		/// <param name="logger"></param>
		public ReportService(IExpenseManager expenseManager, IExpenseCategoryManager expenseCategoryManager, ILog logger)
		{
			_expenseManager = expenseManager;
			_expenseCategoryManager = expenseCategoryManager;
			_logger = logger;
		}

		/// <summary>
		/// Generates <see cref="ReportShort"/> for the provided month.
		/// </summary>
		/// <param name="month">Month for the report.</param>
		/// <param name="year">Year for the report.</param>
		/// <returns><see cref="ReportShort"/> for the provided month.</returns>
		public async Task<Result<ReportShort>> GetMonthlyShortReportAsync(int month, int year)
		{
			var report = new ReportShort();

			var result = await _expenseManager.GetByMonthAsync(month, year).CAF();

			switch (result.ResponseCode)
			{
				case ResponseCode.Error:
					_logger.Write(LogLevel.Error, $"{result.Message}");
					return new Result<ReportShort>(ResponseCode.Error, report) { Message = result.Message };
				case ResponseCode.NotFound:
					_logger.Write(LogLevel.Info, $"Couldn't find any expenses for provided month");
					return new Result<ReportShort>(ResponseCode.NotFound, report);
				case ResponseCode.Ok:
					return await GetReportFromExpenses(result.ReturnedObject);
				default:
					_logger.Write(LogLevel.Fatal, "That shouldn't happened...");
					throw new Exception("Holy molly, something gone terribly wrong... (╯°□°）╯︵ ┻━┻");
			}
		}

		/// <summary>
		/// Generates <see cref="ReportShort"/> for the provided year.
		/// </summary>
		/// <param name="year">Year for the report.</param>
		/// <returns><see cref="ReportShort"/>for the provided year.</returns>
		public ReportShort GetYearlyShortReport(int year)
		{
			return null;
		}

		private async Task<Result<ReportShort>> GetReportFromExpenses(IList<Expense> expenses)
		{
			var report = new ReportShort
			{
				ExpensesTotal = expenses.Sum(e => e.Value),

				CategoriesTotals = await GetCategoriesTotals(expenses)
			};

			return new Result<ReportShort>(ResponseCode.Ok, report);
		}

		private async Task<IDictionary<string, decimal>> GetCategoriesTotals(IList<Expense> expenses)
		{
			var categoriesTotals = new Dictionary<string, decimal>();
			var resultCategories = await _expenseCategoryManager.GetCategoriesAsync().CAF();

			if (resultCategories.ResponseCode is ResponseCode.Ok
				&& resultCategories.ReturnedObject is IList<ExpenseCategory> categories)
			{
				/// consider parallel this
				foreach (var category in categories)
				{
					var filteredExpenses = expenses.Where(ex => ex.Category.Id == category.Id);
					if (filteredExpenses is IEnumerable<Expense>)
					{
						categoriesTotals.Add(category.Name, filteredExpenses.Sum(ex => ex.Value));
					}
				}
			}

			return categoriesTotals;
		}
	}
}
