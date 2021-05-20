using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using Contador.Abstractions;
using Contador.Core.Common;
using Contador.Core.Models;

using Microsoft.AspNetCore.Components;

namespace Contador.Web.Client.Pages
{
	public partial class CategorySummary
	{
		[Parameter] public string Id { get; set; }

		[Inject] private HttpClient _httpClient { get; set; }
		[Inject] private ILog _logger { get; set; }

		private ExpenseCategory Category { get; set; } = new("");
		private CategoryReport Report { get; set; } = new();

		protected override async Task OnInitializedAsync()
		{
			Category = await GetCategoryInfo(Convert.ToInt32(Id));
			Report = await GetCategoryReport();
		}

		private async Task<CategoryReport> GetCategoryReport()
		{
			var result = await _httpClient.GetAsync($"api/report/category/{Category.Id}");

			if (result.IsSuccessStatusCode && result.StatusCode is not System.Net.HttpStatusCode.NoContent)
			{
				return await result.Content.ReadFromJsonAsync<CategoryReport>();
			}

			return new CategoryReport();
		}

		private async Task<ExpenseCategory> GetCategoryInfo(int id)
		{
			var result = await _httpClient.GetAsync($"api/expensecategory/{id}");

			if (result.IsSuccessStatusCode && result.StatusCode is not System.Net.HttpStatusCode.NoContent)
			{
				return await result.Content.ReadFromJsonAsync<ExpenseCategory>();
			}

			return new ExpenseCategory("Not found");
		}
	}
}
