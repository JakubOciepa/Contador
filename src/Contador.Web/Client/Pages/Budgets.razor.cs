using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Contador.Core.Models;
using Contador.Services.Interfaces;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Contador.Web.Client.Pages
{
	public partial class Budgets
	{
		[Inject] private HttpClient _httpClient { get; set; }
		[Inject] private ILog _logger { get; set; }
		[Inject] private IJSRuntime _jsRuntime { get; set; }

		public List<Budget> BudgetList { get; set; } = new List<Budget>();

		public Budget AddBudgetModel { get; set; } = new Budget() { StartDate = DateTime.Now };

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();

			BudgetList = await GetBudgets();
		}

		private async void AddNewBudget()
		{
			var request = new HttpRequestMessage(HttpMethod.Post, "api/budget");

			try
			{
				AddBudgetModel.EndDate = AddBudgetModel.StartDate
					.AddDays(DateTime.DaysInMonth(AddBudgetModel.StartDate.Year, AddBudgetModel.StartDate.Month) - 1);

				request.Content = GetHttpStringContent();

				var result = await _httpClient.SendAsync(request);

				if (result is HttpResponseMessage response && response.IsSuccessStatusCode)
				{
					BudgetList = await GetBudgets();

					this.StateHasChanged();
				}
				else if (result.StatusCode is HttpStatusCode.Conflict
					|| result.StatusCode is HttpStatusCode.BadRequest)
				{
					_logger.Write(Core.Common.LogLevel.Error, "Cannot add budget!");
					await _jsRuntime.InvokeVoidAsync("alert", "Cannot add budget!");
				}
			}
			catch (Exception ex)
			{
				_logger.Write(Core.Common.LogLevel.Error, $"{ex.Message}:\n{ex.StackTrace}");
				await _jsRuntime.InvokeVoidAsync("alert", "Cannot add budget!");
			}
		}

		private async Task<List<Budget>> GetBudgets()
		{
			try
			{
				var result = await _httpClient.GetAsync("api/budget");

				if (result.IsSuccessStatusCode && result.StatusCode is not HttpStatusCode.NoContent)
				{
					return (await result.Content.ReadFromJsonAsync<IList<Budget>>())
						.OrderBy(e => e.StartDate).ToList();
				}

				return new List<Budget>();
			}
			catch (Exception ex)
			{
				_logger.Write(Core.Common.LogLevel.Error, $"{ex.Message}:\n{ex.StackTrace}");

				return new List<Budget>();
			}
		}

		private StringContent GetHttpStringContent()
		{
			var body = new
			{
				StartDate = AddBudgetModel.StartDate,
				EndDate = AddBudgetModel.EndDate,
			};

			return new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
		}
	}
}
