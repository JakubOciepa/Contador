using System;
using System.Threading.Tasks;

using Contador.Abstractions;
using Contador.Core.Common;
using Contador.Core.Utils.Extensions;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Contador.Web.Server.Controllers
{
	/// <summary>
	/// Provides methods to get reports for expenses and categories.
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public class ReportController : Controller
	{
		private readonly IReportService _reportService;

		/// <summary>
		/// Creates an instance of the <see cref="ReportController"/> class.
		/// </summary>
		/// <param name="reportService"></param>
		public ReportController(IReportService reportService)
		{
			_reportService = reportService;
		}

		/// <summary>
		/// Gets the monthly short report by the provided month.
		/// </summary>
		/// <param name="year">Year for the report.</param>
		/// <param name="month">Month for the report.</param>
		/// <returns><see cref="ReportShort"/> for provided month.</returns>
		[HttpGet("short/{year}/{month}")]
		[ProducesResponseType(typeof(ReportShort), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		public async Task<ActionResult<ReportShort>> GetMonthlyShort(int year, int month)
		{
			if (year > DateTime.Now.Year ||
				(year == DateTime.Now.Year && month > DateTime.Now.Month))
			{
				return BadRequest("Provided year or month is not valid!");
			}

			var result = await _reportService.GetMonthlyShortReportAsync(month, year).CAF();

			return result.ResponseCode switch
			{
				ResponseCode.Error => BadRequest(result.Message),
				ResponseCode.NotFound => NoContent(),
				ResponseCode.Ok => Ok(result.ReturnedObject),
				_ => BadRequest("Something gone wrong..."),
			};
		}

		/// <summary>
		/// Gets the yearly short report by the provided year.
		/// </summary>
		/// <param name="year">Year for the report.</param>
		/// <returns><see cref="ReportShort"/> for provided year.</returns>
		[HttpGet("short/{year}")]
		[ProducesResponseType(typeof(ReportShort), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		public async Task<ActionResult<ReportShort>> GetYearlyShort(int year)
		{
			if (year > DateTime.Now.Year)
			{
				return BadRequest("Provided year is not valid!");
			}

			var result = await _reportService.GetYearlyShortReportAsync(year).CAF();

			return result.ResponseCode switch
			{
				ResponseCode.Error => BadRequest(result.Message),
				ResponseCode.NotFound => NoContent(),
				ResponseCode.Ok => Ok(result.ReturnedObject),
				_ => BadRequest("Something gone wrong..."),
			};
		}

		/// <summary>
		/// Gets the full report for the expense category.
		/// </summary>
		/// <param name="id">Id of the category.</param>
		/// <returns><see cref="CategoryReport"/> for provided category.</returns>
		[HttpGet("category/{id}")]
		[ProducesResponseType(typeof(ReportShort), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		public async Task<ActionResult<CategoryReport>> GetForCategory(int id)
		{
			var result = await _reportService.GetForCategoryAsync(id).CAF();

			return result.ResponseCode switch
			{
				ResponseCode.Error => BadRequest(result.Message),
				ResponseCode.NotFound => NoContent(),
				ResponseCode.Ok => Ok(result.ReturnedObject),
				_ => BadRequest("Something gone wrong..."),
			};
		}

	}
}
