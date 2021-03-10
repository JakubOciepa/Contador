using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Contador.Abstractions;
using Contador.Core.Models;
using Contador.Web.Client.Models;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Contador.Web.Client.Pages
{
	public partial class Expenses
	{
		[Inject] private HttpClient _httpClient { get; set; }
		[Inject] private ILog _logger { get; set; }
		[Inject] private IJSRuntime _jsRuntime { get; set; }

		private IList<Expense> expenses = new List<Expense>();
		private IList<ExpenseCategory> categories = new List<ExpenseCategory>();
		private ExpenseModel expenseModel = new();

		protected override async Task OnInitializedAsync()
		{

			expenses = await GetAndSortExpenses();
			categories = await GetCategories();
		}

		private async void AddNewExpense()
		{
			var request = new HttpRequestMessage(HttpMethod.Post, "api/expense");

			var body = new
			{
				name = expenseModel.Name,
				category = new
				{
					id = expenseModel.CategoryId,
					name = categories.First(c => c.Id == expenseModel.CategoryId).Name,
				},
				user = new
				{
					id = 1,
					name = "Kuba",
					email = "kuba@test.com"
				},
				value = expenseModel.Value,
				description = expenseModel.Description,
				imagePath = "",
				createDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")
			};

			try
			{
				request.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

				var result = await _httpClient.SendAsync(request);

				if (result is HttpResponseMessage response && response.IsSuccessStatusCode)
				{
					await GetAndSortExpenses();

					this.StateHasChanged();

				}
			}
			catch (Exception ex)
			{
				_logger.Write(Core.Common.LogLevel.Error, $"{ex.Message}:\n{ex.StackTrace}");
				await _jsRuntime.InvokeVoidAsync("alert", "Cannot add expense!");
			}
		}

		private async Task<IList<Expense>> GetAndSortExpenses()
		{
			try
			{
				var result = await _httpClient.GetAsync("api/expense");

				if (result.IsSuccessStatusCode && result.StatusCode is not HttpStatusCode.NoContent)
				{
					return (await result.Content.ReadFromJsonAsync<IList<Expense>>())
						.OrderByDescending(e => e.CreateDate).ToList();
				}

				return new List<Expense>();
			}
			catch (Exception ex)
			{
				_logger.Write(Core.Common.LogLevel.Error, $"{ex.Message}:\n{ex.StackTrace}");

				return new List<Expense>();
			}
		}

		private async Task<IList<ExpenseCategory>> GetCategories()
		{
			var result = await _httpClient.GetAsync("api/expensecategory");

			if (result.IsSuccessStatusCode && result.StatusCode is not HttpStatusCode.NoContent)
			{
				return await result.Content.ReadFromJsonAsync<IList<ExpenseCategory>>();
			}

			return new List<ExpenseCategory>();
		}
	}
}
