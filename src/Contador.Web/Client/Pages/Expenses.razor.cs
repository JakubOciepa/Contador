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
using Contador.Web.Client.Models;

using Microsoft.AspNetCore.Components;

namespace Contador.Web.Client.Pages
{
	public partial class Expenses
	{
		[Inject]
		private HttpClient _httpClient { get; set; }

		private IList<Expense> expenses = new List<Expense>();
		private IList<ExpenseCategory> categories= new List<ExpenseCategory>();
		private ExpenseModel expenseModel = new();

		protected override async Task OnInitializedAsync()
		{
			
				await GetAndSortExpenses();

				categories = await _httpClient.GetFromJsonAsync<IList<ExpenseCategory>>("api/expensecategory");
		}

		private async Task GetAndSortExpenses()
		{
			try
			{
				var result = await _httpClient.GetAsync("api/expense");

				if (result.IsSuccessStatusCode && result.StatusCode is not HttpStatusCode.NoContent)
				{
					expenses = (await result.Content.ReadFromJsonAsync<IList<Expense>>())
						.OrderByDescending(e => e.CreateDate).ToList();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
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

				Console.WriteLine(JsonSerializer.Serialize(body));

				var result = await _httpClient.SendAsync(request);

				if (result is HttpResponseMessage response && response.IsSuccessStatusCode)
				{
					expenses = (await _httpClient.GetFromJsonAsync<IList<Expense>>("api/expense"))
						.OrderByDescending(e => e.CreateDate)
						.ToList();

					this.StateHasChanged();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}

		}
	}
}
