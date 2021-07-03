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
	public partial class BudgetInfo
	{
		[Parameter] public string Id { get; set; }

		[Inject] private HttpClient _httpClient { get; set; }
		[Inject] private ILog _logger { get; set; }
		[Inject] private IJSRuntime _jsRuntime { get; set; }


		public Budget Budget { get; set; } = new Budget();

		public List<ExpenseCategory> AvailableCategories { get; set; } = new List<ExpenseCategory>();

		private AddBudgetInfo BudgetToAdd { get; set; } = new AddBudgetInfo();

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();

			Budget = await GetBudgetAsync(Convert.ToInt32(Id));
			AvailableCategories = await GetAvailableCategories();
		}

		private async Task AddCategoryBudget()
		{
			var request = new HttpRequestMessage(HttpMethod.Post, "/api/budget/categorybudget");

			try
			{
				request.Content = GetHttpStringContent();

				var result = await _httpClient.SendAsync(request);

				if (result is HttpResponseMessage response && response.IsSuccessStatusCode)
				{
					Budget = await GetBudgetAsync(Convert.ToInt32(Id));
					AvailableCategories = await GetAvailableCategories();
					BudgetToAdd = new AddBudgetInfo();

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


		private async Task<Budget> GetBudgetAsync(int id)
		{
			try
			{
				var result = await _httpClient.GetAsync($"api/budget/{id}");

				if (result.IsSuccessStatusCode && result.StatusCode is not HttpStatusCode.NoContent)
				{
					return (await result.Content.ReadFromJsonAsync<Budget>());
				}

				return new Budget();
			}
			catch (Exception ex)
			{
				_logger.Write(Core.Common.LogLevel.Error, $"{ex.Message}:\n{ex.StackTrace}");

				return new Budget();
			}
		}

		private async Task<List<ExpenseCategory>> GetAvailableCategories()
		{
			try
			{
				var result = await _httpClient.GetAsync($"api/expensecategory");

				if (result.IsSuccessStatusCode && result.StatusCode is not HttpStatusCode.NoContent)
				{
					var categories = (await result.Content.ReadFromJsonAsync<List<ExpenseCategory>>());

					return categories.Where(c => !Budget.Values.ContainsKey(c.Name)).ToList();
				}

				return new List<ExpenseCategory>();
			}
			catch (Exception ex)
			{
				_logger.Write(Core.Common.LogLevel.Error, $"{ex.Message}:\n{ex.StackTrace}");
				return new List<ExpenseCategory>();
			}
		}

		private StringContent GetHttpStringContent()
		{
			var body = new
			{
				BudgetId = Id,
				CategoryId = BudgetToAdd.CategoryId,
				Value = BudgetToAdd.Value
			};

			return new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
		}

		private class AddBudgetInfo
		{
			public int CategoryId { get; set; }
			public decimal Value { get; set; }
		}
	}
}
