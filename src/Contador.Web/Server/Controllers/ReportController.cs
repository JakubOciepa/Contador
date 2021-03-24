﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Contador.Abstractions;
using Contador.Core.Common;
using Contador.Core.Utils.Extensions;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Contador.Web.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ReportController : Controller
	{
		private readonly IReportService _reportService;

		public ReportController(IReportService reportService)
		{
			_reportService = reportService;
		}

		[HttpGet("short/{year}/{month}")]
		[ProducesResponseType(typeof(ReportShort), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		public async Task<ActionResult<ReportShort>> GetMonthlyShort(int year, int month)
		{
			if(year > DateTime.Now.Year ||
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

		[HttpGet("short/{year}")]
		[ProducesResponseType(typeof(ReportShort), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		public async Task<ActionResult<ReportShort>> GetYearlyShort(int year)
		{
			if (year > DateTime.Now.Year)
			{
				return BadRequest("Provided year or month is not valid!");
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
	}
}
