using System;
using System.Collections.Generic;
using System.Linq;
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

		private IList<Expense> expenses;
		private IList<ExpenseCategory> categories;
		private ExpenseModel expenseModel = new ExpenseModel();

		protected override async Task OnInitializedAsync()
		{
			try
			{
				expenses = (await _httpClient.GetFromJsonAsync<IList<Expense>>("api/expense")).OrderByDescending(e => e.CreateDate).ToList();
				categories = await _httpClient.GetFromJsonAsync<IList<ExpenseCategory>>("api/expensecategory");
			}
			catch (Exception ex)
			{

			}
		}

		private async void AddNewExpense()
		{
			// create request object
			var request = new HttpRequestMessage(HttpMethod.Post, "api/expense");

			Console.WriteLine(expenseModel.Value);
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
				value = expenseModel.Value.ToString(),
				description = expenseModel.Description,
				imagePath = "",
				createDate = DateTime.Now.ToString("yyyy-MM-dd")
			};

			try
			{
				request.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
				request.Headers.Add("My-Custom-Header", "foobar");

				var result = await _httpClient.SendAsync(request);

				if (result is HttpResponseMessage response && response.IsSuccessStatusCode)
				{
					expenses = (await _httpClient.GetFromJsonAsync<IList<Expense>>("api/expense")).OrderByDescending(e => e.CreateDate).ToList();
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
