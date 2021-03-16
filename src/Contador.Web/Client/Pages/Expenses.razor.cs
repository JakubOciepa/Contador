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
using Microsoft.JSInterop;

namespace Contador.Web.Client.Pages
{
	public partial class Expenses
	{
		[Inject] private HttpClient _httpClient { get; set; }
		[Inject] private ILog _logger { get; set; }
		[Inject] private IJSRuntime _jsRuntime { get; set; }

		private IList<Expense> ExpensesList = new List<Expense>();
		private IList<ExpenseCategory> Categories = new List<ExpenseCategory>();
		private ExpenseModel ExpenseModel = new();

		protected override async Task OnInitializedAsync()
		{
			ExpensesList = await GetAndSortExpenses();
			Categories = await GetCategories();
		}

		private async void AddNewExpense()
		{
			var request = new HttpRequestMessage(HttpMethod.Post, "api/expense");

			try
			{
				request.Content = GetHttpStringContent();

				var result = await _httpClient.SendAsync(request);

				if (result is HttpResponseMessage response && response.IsSuccessStatusCode)
				{
					ExpensesList = await GetAndSortExpenses();

					this.StateHasChanged();

				}
				else if (result.StatusCode is HttpStatusCode.Conflict
					|| result.StatusCode is HttpStatusCode.BadRequest)
				{
					_logger.Write(Core.Common.LogLevel.Error, "Cannot add expense!");
					await _jsRuntime.InvokeVoidAsync("alert", "Cannot add expense!");
				}
			}
			catch (Exception ex)
			{
				_logger.Write(Core.Common.LogLevel.Error, $"{ex.Message}:\n{ex.StackTrace}");
				await _jsRuntime.InvokeVoidAsync("alert", "Cannot add expense!");
			}
		}

		private StringContent GetHttpStringContent()
		{
			var body = new
			{
				name = ExpenseModel.Name,
				category = new
				{
					id = ExpenseModel.CategoryId,
					name = Categories.First(c => c.Id == ExpenseModel.CategoryId).Name,
				},
				user = new
				{
					id = 1,
					name = "Kuba",
					email = "kuba@test.com"
				},
				value = ExpenseModel.Value,
				description = ExpenseModel.Description,
				imagePath = "",
				createDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")
			};

			return new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
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
