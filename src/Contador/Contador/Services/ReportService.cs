using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Contador.Core.Common;
using Contador.Core.Models;
using Contador.Core.Utils.Extensions;
using Contador.Services.Interfaces;

namespace Contador.Services
{
	/// <summary>
	/// Provides methods to get reports for expenses and categories.
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
					return await GetReportFromExpensesAsync(result.ReturnedObject);
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
		public async Task<Result<ReportShort>> GetYearlyShortReportAsync(int year)
		{
			var report = new ReportShort();

			var result = await _expenseManager.GetByYearAsync(year).CAF();

			switch (result.ResponseCode)
			{
				case ResponseCode.Error:
					_logger.Write(LogLevel.Error, $"{result.Message}");
					return new Result<ReportShort>(ResponseCode.Error, report) { Message = result.Message };
				case ResponseCode.NotFound:
					_logger.Write(LogLevel.Info, $"Couldn't find any expenses for provided year");
					return new Result<ReportShort>(ResponseCode.NotFound, report);
				case ResponseCode.Ok:
					return await GetReportFromExpensesAsync(result.ReturnedObject);
				default:
					_logger.Write(LogLevel.Fatal, "That shouldn't happened...");
					throw new Exception("Holy molly, something gone terribly wrong... (╯°□°）╯︵ ┻━┻");
			}
		}

		/// <summary>
		/// Generates <see cref="CategoryReport"/> for the category of provided id.
		/// </summary>
		/// <param name="categoryId">Category id for the report</param>
		/// <returns><see cref="CategoryReport"/> for the provided category.</returns>
		public async Task<Result<CategoryReport>> GetForCategoryAsync(int categoryId)
		{
			var report = new CategoryReport();
			var result = await _expenseManager.GetByCategory(categoryId);

			switch (result.ResponseCode)
			{
				case ResponseCode.Error:
					_logger.Write(LogLevel.Error, $"{result.Message}");
					return new Result<CategoryReport>(ResponseCode.Error, report) { Message = result.Message };
				case ResponseCode.NotFound:
					_logger.Write(LogLevel.Info, $"Couldn't find any expenses for provided category");
					return new Result<CategoryReport>(ResponseCode.NotFound, report);
				case ResponseCode.Ok:
					return GetReportForCategory(result.ReturnedObject);
				default:
					_logger.Write(LogLevel.Fatal, "That shouldn't happened...");
					throw new Exception("Holy molly, something gone terribly wrong... (╯°□°）╯︵ ┻━┻");
			}
		}

		private async Task<Result<ReportShort>> GetReportFromExpensesAsync(IList<Expense> expenses)
		{
			var report = new ReportShort
			{
				ExpensesTotal = expenses.Sum(e => e.Value),
				CategoriesTotals = await GetCategoriesTotals(expenses),
			};

			report.CategoriesPercentages = GetCategoriesPercentages(report.ExpensesTotal, report.CategoriesTotals);

			return new Result<ReportShort>(ResponseCode.Ok, report);
		}

		private Result<CategoryReport> GetReportForCategory(IList<Expense> expenses)
		{
			var report = new CategoryReport
			{
				AverageMonthly = GetAverageMontlhy(expenses),
				AverageYearly = GetAverageYearly(expenses),
				MonthSpent = expenses.Where(e => e.CreateDate.Month == DateTime.Now.Month).Sum(e => e.Value),
				YearSpent = expenses.Where(e => e.CreateDate.Year == DateTime.Now.Year).Sum(e => e.Value),
				Expenses = expenses
			};

			return new Result<CategoryReport>(ResponseCode.Ok, report);
		}

		private decimal GetAverageYearly(IList<Expense> expenses)
		{
			var yearCount = DateTime.Now.Year - expenses.OrderBy(e => e.CreateDate.Year).First().CreateDate.Year;

			return expenses.Sum(e => e.Value) / (yearCount == 0 ? 1 : yearCount);
		}

		private decimal GetAverageMontlhy(IList<Expense> expenses)
		{
			var expensesInRange = expenses.Where(e => e.CreateDate >= DateTime.Now.AddMonths(-12))
				.OrderBy(e => e.CreateDate);

			var monthCount = (DateTime.Now.Month - expensesInRange.First().CreateDate.Month) % 12;

			return expensesInRange.Sum(e => e.Value) / (monthCount == 0 ? 1 : monthCount);
		}

		private async Task<IDictionary<string, decimal>> GetCategoriesTotals(IList<Expense> expenses)
		{
			var categoriesTotals = new ConcurrentDictionary<string, decimal>();
			var resultCategories = await _expenseCategoryManager.GetAllAsync().CAF();

			if (resultCategories.ResponseCode is ResponseCode.Ok
				&& resultCategories.ReturnedObject is IList<ExpenseCategory> categories)
			{
				Parallel.ForEach(categories, category =>
				{
					var filteredExpenses = expenses.Where(ex => ex.Category.Id == category.Id);
					if (filteredExpenses is IEnumerable<Expense>)
					{
						categoriesTotals.AddOrUpdate(category.Name, filteredExpenses.Sum(ex => ex.Value), (key, value) => value);
					}
				});
			}

			return categoriesTotals;
		}

		private IDictionary<string, int> GetCategoriesPercentages(decimal expensesTotal, IDictionary<string, decimal> categoriesTotals)
		{
			var percentages = new Dictionary<string, int>();

			foreach (var categoryTotal in categoriesTotals)
			{
				percentages.Add(categoryTotal.Key, (int)(categoryTotal.Value * 100 / expensesTotal));
			}

			return percentages;
		}
	}
}
