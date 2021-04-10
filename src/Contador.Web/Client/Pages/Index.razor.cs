using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using Contador.Core.Common;

using Microsoft.AspNetCore.Components;

namespace Contador.Web.Client.Pages
{
	public partial class Index
	{
		[Inject] private HttpClient _httpClient { get; set; }

		public ReportShort MonthlyReport { get; set; }
		public ReportShort YearlyReport { get; set; }

		protected override async Task OnInitializedAsync()
		{
			MonthlyReport = await GetMonthlyReport();
			YearlyReport = await GetYearlyReport();
		}

		private async Task<ReportShort> GetMonthlyReport()
		{
			var result = await _httpClient.GetAsync($"api/report/short/{DateTime.Now.Year}/{DateTime.Now.Month}");

			if (result.IsSuccessStatusCode && result.StatusCode is not HttpStatusCode.NoContent)
			{
				return await result.Content.ReadFromJsonAsync<ReportShort>();
			}

			return ReportShort.Empty;
		}

		private async Task<ReportShort> GetYearlyReport()
		{
			var result = await _httpClient.GetAsync($"api/report/short/{DateTime.Now.Year}");

			if (result.IsSuccessStatusCode && result.StatusCode is not HttpStatusCode.NoContent)
			{
				return await result.Content.ReadFromJsonAsync<ReportShort>();
			}

			return ReportShort.Empty;
		}
	}
}

